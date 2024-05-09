using UnityEngine;

/// <summary>
/// ���������� �������������� ����������, ������� ��� ���������� ����� �������� ����������� ����������
/// �������� � ����� �������
/// </summary>
public class Ammo : ObjectWithInformation
{
    [SerializeField] private int count = 10;

    /// <summary>
    /// ���������� ��������, ������� ��������� ��������� � �����
    /// </summary>
    public int Count => count;

    public override string[,] ObjectInfoParameters { get; set; }

    public override string ObjectInfoHeader { get; set; } = "Universal Ammo";

    public override Color ObjectInfoHeaderColor { get; set; } = Color.green;

    private void Awake()
    {
        InitializeInfoPanelPrefab();
        ObjectInfoParameters = new string[1, 2] { { "Bullets:", Count.ToString() } };
    }

    /// <summary>
    /// �������� ���������� ��� ��� ���������� �������
    /// </summary>
    public void PickUp()
    {
        Destroy(gameObject);
    }
}