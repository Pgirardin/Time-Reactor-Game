using UnityEngine;
using TMPro;

/// <summary>
/// ���������� �������� ������, ����� ����� ���������� ���� ��� ����������� ���� �������� �����
/// </summary>
public class FloorNumberUpdating : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI floorNumberText;
    [SerializeField]
    private PlayerController playerControllertScript;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (playerControllertScript.PlayerVelocity.z > 0f)
            {
                GameProperties.FloorNumber--;
            }
        }
        floorNumberText.text = GameProperties.FloorNumber.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (playerControllertScript.PlayerVelocity.z < 0f)
            {
                GameProperties.FloorNumber++;
            }
        }
        floorNumberText.text = GameProperties.FloorNumber.ToString();
    }
}