using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ���������� ������������ �����������, ������� ����� ����� �������� � �������
/// � ��� ������� ����� ������ ��������� ������, ���������, � �����
/// �������������� ���������� �� ������ ������
/// </summary>
public class GraphicAnalyzerController : MonoBehaviour
{
    [SerializeField] private GameObject analyzerInfoPanel;

    // ������������ ���������, �� ������� ����� ����� ����������������� � ���������
    [SerializeField] private float interactionDistance;
    // ����������� ����� ����� ������ ������� ������������/���������
    [SerializeField] private float intervalBetweenStateChanging;

    // ����������� �� ������/������ ������, ������� ���������� �� ������/������, � ������� ������ ����� �������,
    // ����� ��� �� ������������ ������
    [SerializeField] private float panelOffsetCoefficientInPlane;
    // ����������� ���������� ����� ��������� � �������, ������� ����������, ��������� ������ ������ ����� � ������
    // ������ ���� �� 0f �� 1f
    [SerializeField] private float panelOffsetCoefficientToPlayer;

    private static bool analyzerIsActive = false;
    private bool stateChangingIsAllowed = true;

    private ObjectWithInformation objectPlayerCurrentlyLookingAt = null;

    private AudioSource activationSound;
    private AudioSource deactivationSound;

    public void FixedUpdate()
    {
        CheckStateChanging();
        HideInformationIfPlayerStoppedLookingAtObject();
        DisplayInformationIfPlayerLookingAtObject();
    }

    /// <summary>
    /// ������� �� ����������� ���������� � ������ ������
    /// </summary>
    public static bool AnalyzerIsActive
    {
        get { return analyzerIsActive; }
        set
        {
            analyzerIsActive = value;
            if (StateChanged != null)
            {
                StateChanged(analyzerIsActive);
            }
        }
    }

    /// <summary>
    /// �������, ������� ���������� ��� ���������/����������� ������������ �����������
    /// </summary>
    public static Action<bool> StateChanged { get; set; }

    private void Awake()
    {
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            var clipName = audioSource.clip.name;
            if (clipName == "Activation")
            {
                activationSound = audioSource;
            }
            else if (clipName == "Deactivation")
            {
                deactivationSound = audioSource;
            }
        }
    }

    /// <summary>
    /// �������� �� ������� ������ ��� ����� ������
    /// </summary>
    private void CheckStateChanging()
    {
        if (Input.GetKey(KeyCode.G))
        {
            if (stateChangingIsAllowed)
            {
                if (!AnalyzerIsActive)
                {
                    activationSound.Play();
                }
                else
                {
                    deactivationSound.Play();
                }

                AnalyzerIsActive = !AnalyzerIsActive;
                analyzerInfoPanel.SetActive(AnalyzerIsActive);
                if (!AnalyzerIsActive)
                {
                    if (objectPlayerCurrentlyLookingAt != null)
                    {
                        objectPlayerCurrentlyLookingAt.HideInfoPanel();
                        objectPlayerCurrentlyLookingAt = null;
                    }
                }

                stateChangingIsAllowed = false;
                StartCoroutine(AllowStateChangingAfterIntervalPassing());
            }
        }
    }

    /// <summary>
    /// ����� ��������� �������� ����� ����� ���������� ��������� �������
    /// </summary>
    private IEnumerator AllowStateChangingAfterIntervalPassing()
    {
        yield return new WaitForSeconds(intervalBetweenStateChanging);
        stateChangingIsAllowed = true;
    }

    /// <summary>
    /// �������� ��������� ������, �� ������� ������� ����� � ������ ������, ���� �� ��������� �� ���������� ���������,
    /// � ����� ����� ��������� ���� � ������
    /// </summary>
    public (GameObject, Vector3) GetObjectPlayerIsLookingAt()
    {
        Ray rayToScreenCenter = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.Log(rayToScreenCenter);
        RaycastHit hit;
        int defaultLayerMask = 1;

        if (Physics.Raycast(rayToScreenCenter, out hit, interactionDistance, defaultLayerMask, QueryTriggerInteraction.Ignore))
        {
            return (hit.transform.gameObject, hit.point);
        }
        return (null, Vector3.zero);
    }

    /// <summary>
    /// ������ ��������� ���������� Trasform � �������������� ������ ���, ����� ��� ���� ������ ����� ������
    /// </summary>
    /// <param name="hitPoint">�����, � ������� ��� ����� � ������, ��� �������� ����� ���������� ����������</param>
    private void SetTransformOfInfoPanel(Vector3 hitPoint)
    {
        var panelRect = objectPlayerCurrentlyLookingAt.GetInfoPanel().GetComponent<RectTransform>();
        var panelWidth = panelRect.rect.width * panelRect.localScale.x;
        var panelHeight = panelRect.rect.height * panelRect.localScale.y;

        Vector3 setterPosition = hitPoint;
        setterPosition -= Camera.main.transform.right * (panelWidth * panelOffsetCoefficientInPlane);
        setterPosition += Camera.main.transform.up * (panelHeight * panelOffsetCoefficientInPlane);
        setterPosition -= Camera.main.transform.forward * (Vector3.Distance(hitPoint, Camera.main.transform.position) * panelOffsetCoefficientToPlayer);
        objectPlayerCurrentlyLookingAt.SetInfoPanelTransform(setterPosition, Camera.main.transform.rotation, panelRect.localScale);
    }

    /// <summary>
    /// ���������� ���������� �� �������, �� ������� � ������ ������ ������� �����, ���� ������
    /// �������� ����������� ObjectWithInformation
    /// </summary>
    private void DisplayInformationIfPlayerLookingAtObject()
    {
        if (!AnalyzerIsActive)
        {
            return;
        }

        (var objectAhead, var hitPoint) = GetObjectPlayerIsLookingAt();
        if (objectAhead != null && objectAhead.GetComponent<ObjectWithInformation>() != null)
        {
            objectPlayerCurrentlyLookingAt = objectAhead.GetComponent<ObjectWithInformation>();
            objectPlayerCurrentlyLookingAt.ShowInfoPanel();
            SetTransformOfInfoPanel(hitPoint);
        }
    }

    /// <summary>
    /// ������ ���������� �� �������, �� ������� ������ ��� ������� �����, ���� ������
    /// �������� ����������� ObjectWithInformation
    /// </summary>
    private void HideInformationIfPlayerStoppedLookingAtObject()
    {
        // ���� ����� ������ ������ �� ������ ������
        if (objectPlayerCurrentlyLookingAt != null && GetObjectPlayerIsLookingAt().Item1 != objectPlayerCurrentlyLookingAt)
        {
            objectPlayerCurrentlyLookingAt.HideInfoPanel();
        }
    }
}