using UnityEngine;

/// <summary>
/// ���������� �������, �������� ���������� � ����, ������� ����� ���������� ����� ����������� ����������
/// </summary>
public abstract class ObjectWithInformation : MonoBehaviour
{
    protected GameObject infoPanelPrefab;
    protected bool panelWasCreated = false;
    protected GameObject createdPanel = null;

    /// <summary>
    /// �������� ������ � �����������
    /// </summary>
    public abstract void ShowInfoPanel();

    /// <summary>
    /// ������ ������ � �����������
    /// </summary>
    public abstract void HideInfoPanel();

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