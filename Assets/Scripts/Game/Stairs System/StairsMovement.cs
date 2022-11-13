using UnityEngine;

/// <summary>
/// ������������ �������� �������� ��� �������� ����������� ��������
/// </summary>
public class StairsMovement : MonoBehaviour
{
    [SerializeField] private GameObject currentStairsStrucure;
    [SerializeField] private GameObject otherStairsStructure;
    [SerializeField] private GameObject centralWall;
    [SerializeField] private PlayerController playerControllerScript;

    private const float STRUCTURE_OFFSET = 2.425f;
    
    // ����������� ������ �� ��� Z (����� ��� ���� �� ��������), ����� �� ������ � ������� ������������ �������� ��������
    private float onColliderEnterZAxisValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            onColliderEnterZAxisValue = playerControllerScript.PlayerVelocity.z;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            // �������� �� ����������� ������ ����������� ����� � ������ (����� ��������� ������ �������)
            if (playerControllerScript.PlayerVelocity.z > 0f && onColliderEnterZAxisValue > 0f)
            {
                if (currentStairsStrucure.name == "Lower Structure")
                {
                    otherStairsStructure.transform.position = new Vector3(otherStairsStructure.transform.position.x,
                        currentStairsStrucure.transform.position.y - STRUCTURE_OFFSET, otherStairsStructure.transform.position.z);
                    centralWall.transform.position = new Vector3(centralWall.transform.position.x,
                        centralWall.transform.position.y - STRUCTURE_OFFSET, centralWall.transform.position.z);
                }
                else if (currentStairsStrucure.name == "Upper Structure")
                {
                    otherStairsStructure.transform.position = new Vector3(otherStairsStructure.transform.position.x,
                        currentStairsStrucure.transform.position.y + STRUCTURE_OFFSET, otherStairsStructure.transform.position.z);
                    centralWall.transform.position = new Vector3(centralWall.transform.position.x,
                        centralWall.transform.position.y + STRUCTURE_OFFSET, centralWall.transform.position.z);
                }
            }
            else if (playerControllerScript.PlayerVelocity.z < 0f && onColliderEnterZAxisValue < 0f)
            {
                if (currentStairsStrucure.name == "Lower Structure")
                {
                    otherStairsStructure.transform.position = new Vector3(otherStairsStructure.transform.position.x,
                        currentStairsStrucure.transform.position.y + STRUCTURE_OFFSET, otherStairsStructure.transform.position.z);
                    centralWall.transform.position = new Vector3(centralWall.transform.position.x,
                        centralWall.transform.position.y + STRUCTURE_OFFSET, centralWall.transform.position.z);
                }
                else if (currentStairsStrucure.name == "Upper Structure")
                {
                    otherStairsStructure.transform.position = new Vector3(otherStairsStructure.transform.position.x,
                        currentStairsStrucure.transform.position.y - STRUCTURE_OFFSET, otherStairsStructure.transform.position.z);
                    centralWall.transform.position = new Vector3(centralWall.transform.position.x,
                        centralWall.transform.position.y - STRUCTURE_OFFSET, centralWall.transform.position.z);
                }
            }
        }
    }
}