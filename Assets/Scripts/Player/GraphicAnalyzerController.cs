using UnityEngine;
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
    [SerializeField] private float interactionDistance = 2.5f;

    // ����������� �� ������/������ ������, ������� ���������� �� ������/������, � ������� ������ ����� �������,
    // ����� ��� �� ������������ ������
    [SerializeField] private float panelOffsetCoefficientInPlane = 0.5f;
    // ����������� ���������� ����� ��������� � �������, ������� ����������, ��������� ������ ������ ����� � ������
    // ������ ���� �� 0f �� 1f
    [SerializeField] private float panelOffsetCoefficientToPlayer = 0.5f;

    private bool analyzerIsActive = false;
    // ����������� ����� ����� ������ ������� ������������/���������
    private float intervalBetweenStateChanging = 0.5f;
    private bool stateChangingIsAllowed = true;

    private ObjectWithInformation objectPlayerCurrentlyLookingAt = null;

    public void FixedUpdate()
    {
        CheckStateChanging();
        HideInformationIfPlayerStoppedLookingAtObject();
        DisplayInformationIfPlayerLookingAtObject();
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
                analyzerIsActive = !analyzerIsActive;
                analyzerInfoPanel.SetActive(analyzerIsActive);
                if (!analyzerIsActive)
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
        if (!analyzerIsActive)
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