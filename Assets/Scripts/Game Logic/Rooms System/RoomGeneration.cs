using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� ��������� ������� ���������� ���� �� ������ �����
/// </summary>
public class RoomGeneration : MonoBehaviour
{
    // ������� ������ ������� ���� (�����������, ����� � �.�.)
    [SerializeField] private List<GameObject> differentTypeRooms = new List<GameObject>();
    // ����� �����, � ������� ������ "���������" �������
    [SerializeField] private Transform centerOfDoor;

    private System.Random random = new System.Random();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            // ������� ��������, ���� ��� ������� ����� �������������� ������� ���� � �� ������ ����� �����
            // �� ���� ������������� �������
            if (GameProperties.DoorOnFloor[-GameProperties.FloorNumber] &&
                !GameProperties.GeneratedRooms.ContainsKey(GameProperties.FloorNumber))
            {
                var randomRoomPrefab = ChooseRandomRoom();
                CreateAndPlaceRoom(randomRoomPrefab);
            }
        }
    }

    /// <summary>
    /// ������� ������� ���������� ���� �� ������ ��������
    /// </summary>
    /// <returns>��������� ������ ��������� �������</returns>
    private GameObject ChooseRandomRoom()
    {
        var randomRoomNumber = random.Next(differentTypeRooms.Count);
        return differentTypeRooms[randomRoomNumber];
    }

    /// <summary>
    /// ������� �� ������� ������� � ���������� � ������ � �����
    /// </summary>
    private void CreateAndPlaceRoom(GameObject roomPrefab)
    {
        var room = Instantiate(roomPrefab);
        GameProperties.GeneratedRooms.Add(GameProperties.FloorNumber, room);

        var roomEntranceCenter = room.transform.Find("Center Of Entrance");
        // ����� ����� � ����� �������� ����������� � ������� ����� � �������
        room.transform.position = centerOfDoor.position + (roomPrefab.transform.position - roomEntranceCenter.position);
    }
}