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

    [SerializeField] private int minFloorCount;
    [SerializeField] private int maxFloorCount;

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
        if (GameProperties.FloorNumber == GameProperties.GeneralFloorCount - 1 && !finalRoomWasGenerated)
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
        int generatedFloorCount = random.Next(minFloorCount, maxFloorCount + 1);
        GameProperties.GeneralFloorCount = generatedFloorCount;
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