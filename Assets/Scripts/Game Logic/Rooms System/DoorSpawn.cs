using UnityEngine;

/// <summary>
/// ����� ����� � ��������� ������ �� ������ �����
/// </summary>
public class DoorSpawn : MonoBehaviour
{
    [SerializeField] private float doorSpawnChance;
    [SerializeField] private GameObject wallWithDoor;
    [SerializeField] private GameObject wallWithoutDoor;

    // ����������� ������ �� ��� Z (����� ��� ���� �� ��������), ����� �� ������ � ������� ������������ �������� ��������
    private float onColliderEnterZAxisValue;
    private System.Random random = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            onColliderEnterZAxisValue = playerController.PlayerVelocity.z;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();

            // �������� �� ����������� ������ ����������� ����� � ������ (����� ��������� ������ �������)
            if (playerController.PlayerVelocity.z > 0f && onColliderEnterZAxisValue > 0f)
            {
                CreateOrDestroyDoorDependingFloorNumber(-GameProperties.FloorNumber - 1);
            }
            else if (playerController.PlayerVelocity.z < 0f && onColliderEnterZAxisValue < 0f)
            {
                DetermineIfDoorWillOnFloor();
                CreateOrDestroyDoorDependingFloorNumber(-GameProperties.FloorNumber);
            }
        }
    }

    /// <summary>
    /// ���������� ������� ����� �� �����, ��� ��������� �����
    /// </summary>
    private void DetermineIfDoorWillOnFloor()
    {
        // ���� ����� ������� �������� ������� �� ������� �����, �� ��� ������� �����
        // � ��������� ������ ������������ ������� ����� (����� ������� �����)
        if (GameProperties.DoorOnFloor.Count == -GameProperties.FloorNumber)
        {
            float generatedFloat = (float)random.NextDouble();
            if (generatedFloat < doorSpawnChance)
            {
                GameProperties.DoorOnFloor.Add(true);
            }
            else
            {
                GameProperties.DoorOnFloor.Add(false);
            }
        }
    }

    /// <summary>
    /// ������� ��� ������ ����� � ����������� �� ������ �����
    /// </summary>
    private void CreateOrDestroyDoorDependingFloorNumber(int floorNumber)
    {
        if (floorNumber < 0 || floorNumber >= GameProperties.DoorOnFloor.Count)
        {
            return;
        }

        if (GameProperties.DoorOnFloor[floorNumber])
        {
            wallWithDoor.SetActive(true);
            wallWithoutDoor.SetActive(false);
        }
        else
        {
            wallWithDoor.SetActive(false);
            wallWithoutDoor.SetActive(true);
        }
    }
}