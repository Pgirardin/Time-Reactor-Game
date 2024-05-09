using UnityEngine;

/// <summary>
/// ���������� �����, � ������� � �������� ������������ �������� ����������
/// (��������� � Weapon_Box_1/Weapon_Box_2 � "Added Files To Imported Assets")
/// </summary>
public class AmmoPoint : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private float probability = 0.75f;

    private static System.Random random = new();

    private void Awake()
    {
        TryToGenerateAmmo();
    }

    /// <summary>
    /// ���������� ��������� ����������� � �����, ���� ��������� ����� �� ������� [0; 1] ������ �������� �����������
    /// </summary>
    private void TryToGenerateAmmo()
    {
        var randonFloat = (float)random.NextDouble();
        if (randonFloat < probability)
        {
            Instantiate(ammoPrefab, transform.position, transform.rotation);
        }
    }
}