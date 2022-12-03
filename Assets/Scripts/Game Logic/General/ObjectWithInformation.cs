using UnityEngine;
using TMPro;

/// <summary>
/// ���������� �������, �������� ���������� � ����, ������� ����� ���������� ����� ����������� ����������
/// � ������ Awake() ������-���������� ����� ������� ����� InitializePanelPrefab()
/// </summary>
public abstract class ObjectWithInformation : MonoBehaviour
{
    protected GameObject infoPanelPrefab;
    protected bool panelWasCreated = false;
    protected GameObject createdPanel = null;

    /// <summary>
    /// ��� �������� �������
    /// </summary>
    public abstract string ObjectInfoHeader { get; set; }

    /// <summary>
    /// ���� ���� �������� �������
    /// </summary>
    public abstract Color ObjectInfoHeaderColor { get; set; }

    /// <summary>
    /// ��������� ������ � ����������� �� ������� �������, ���������� null ��� �� 1 �� 5 ����������� (��������� �������),
    /// ������ �� ������� �������� 2 �������� - �������� ��������� ������� � �������� ����� ���������
    /// </summary>
    public abstract string[,] ObjectInfoParameters { get; set; }

    /// <summary>
    /// �������� ������� �������������� ������ �� ��������
    /// </summary>
    public void InitializeInfoPanelPrefab()
    {
        infoPanelPrefab = Resources.Load<GameObject>("Prefabs/Object Information Panel");
    }

    /// <summary>
    /// �������� ������ � ����������� �� �������
    /// </summary>
    public void ShowInfoPanel()
    {
        if (!panelWasCreated)
        {
            createdPanel = Instantiate(infoPanelPrefab);
            panelWasCreated = true;

            createdPanel.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            createdPanel.GetComponent<Canvas>().worldCamera = Camera.main;

            var header = createdPanel.transform.Find("Header").Find("Type Of Object").GetComponent<TextMeshProUGUI>();
            header.text = ObjectInfoHeader;
            header.color = ObjectInfoHeaderColor;

            string[] parametersNumbers = new string[5] { "First", "Second", "Third", "Fourth", "Fifth" };
            for (int i = 0; i < 5; i++)
            {
                var parameter = createdPanel.transform.Find(parametersNumbers[i] + " Parameter");
                if (ObjectInfoParameters == null || i >= ObjectInfoParameters.GetLength(0))
                {
                    parameter.gameObject.SetActive(false);
                    continue;
                }
                parameter.Find("Parameter Text").GetComponent<TextMeshProUGUI>().text = ObjectInfoParameters[i, 0];
                parameter.Find("Parameter Value").GetComponent<TextMeshProUGUI>().text = ObjectInfoParameters[i, 1];
            }

            var canvasRect = createdPanel.GetComponent<RectTransform>();
            var headerHeight = header.rectTransform.rect.height;

            // ������� ������ �������������� ������ � ����������� �� ���������� ���������� �������� �������
            if (ObjectInfoParameters == null)
            {
                canvasRect.sizeDelta = new Vector2(canvasRect.rect.width, headerHeight);
                return;
            }
            canvasRect.sizeDelta = new Vector2(canvasRect.rect.width, headerHeight + (canvasRect.rect.height - headerHeight) / 5 *
                ObjectInfoParameters.GetLength(0));
        }

        createdPanel.SetActive(true);
    }

    /// <summary>
    /// ������ ������ � ����������� �� �������
    /// </summary>
    public void HideInfoPanel()
    {
        if (!panelWasCreated)
        {
            return;
        }

        createdPanel.SetActive(false);
    }

    /// <summary>
    /// �������� ������ � ����������� �� �������
    /// </summary>
    public GameObject GetInfoPanel()
    {
        return createdPanel;
    }

    /// <summary>
    /// ��������� ��������� Transform ������ � ����������� �� �������
    /// </summary>
    /// <returns>true, ���� ������ ������� ���������, ����� false</returns>
    public bool SetInfoPanelTransform(Vector3 setterPosition, Quaternion setterRotation, Vector3 setterScale)
    {
        if (createdPanel == null)
        {
            return false;
        }

        createdPanel.transform.position = setterPosition;
        createdPanel.transform.rotation = setterRotation;
        createdPanel.transform.localScale = setterScale;
        return true;
    }
}