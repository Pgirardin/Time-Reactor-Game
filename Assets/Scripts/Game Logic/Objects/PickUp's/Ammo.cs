using UnityEngine;

/// <summary>
/// ���������� �������������� ����������, ������� ��� ���������� ����� �������� ����������� ����������
/// �������� � ����� �������
/// </summary>
public class Ammo : ObjectWithInformation
{
    [SerializeField] private int ammoCount = 5;
    [SerializeField] private AudioClip pickUpSound;

    /// <summary>
    /// ���������� ��������, ������� ��������� ��������� � �����
    /// </summary>
    public int AmmoCount => ammoCount;

    public override string[,] ObjectInfoParameters { get; set; }

    public override string ObjectInfoHeader { get; set; } = "Universal Ammo";

    public override Color ObjectInfoHeaderColor { get; set; } = Color.green;

    private void Awake()
    {
        InitializeInfoPanelPrefab();
        ObjectInfoParameters = new string[1, 2] { { "Bullets:", AmmoCount.ToString() } };
    }

    /// <summary>
    /// �������� ���������� ��� ��� ���������� �������
    /// </summary>
    public void PickUp()
    {
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        Destroy(gameObject);
    }
}