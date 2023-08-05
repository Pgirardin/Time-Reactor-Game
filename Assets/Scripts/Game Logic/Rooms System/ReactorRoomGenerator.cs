using UnityEngine;

/// <summary>
/// ��������� ������� � ��������� ���������, ������� ����� ���������� � ����� ���� ��������
/// </summary>
public class ReactorRoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    // ������� �������� ��������� ������� (����������� � ������� �������) ��������� � ���� ������� GameObject
    // � ������ ���������� ��������� ("Lower Structure")
    [SerializeField] private Transform spawnPoint;

    // ������� ��� ��������� ������ ������ ������� ����� �������� (�������������)
    [SerializeField] private int minBoundOfLastFloorNumber;
    [SerializeField] private int maxBoundOfLastFloorNumber;

    private System.Random random = new System.Random();
    private bool finalRoomWasGenerated = false;

    private void Awake()
    {
        GenerateGeneralFloorCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��������� ������� �������� �� ��������� ����� ��������, ����� ����� ���������
        // �� ������������� ����� � �������� ������� ����������
        if (GameProperties.FloorNumber == GameProperties.LastFloorNumber + 1 && !finalRoomWasGenerated)
        {
            GenerateFinalRoom();
            finalRoomWasGenerated = true;
        }
    }

    /// <summary>
    /// ������������� ���������� ������ �� ��������
    /// </summary>
    private void GenerateGeneralFloorCount()
    {
        int generatedFloorCount = random.Next(minBoundOfLastFloorNumber, maxBoundOfLastFloorNumber + 1);
        GameProperties.LastFloorNumber = generatedFloorCount;
    }

    /// <summary>
    /// ������� � ���������������� ��������� ������� � ��������� ����� ��������
    /// </summary>
    private void GenerateFinalRoom()
    {
        var room = Instantiate(roomPrefab);
        room.transform.position = spawnPoint.position;
    }
}