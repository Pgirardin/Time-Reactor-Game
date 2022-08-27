using UnityEngine;

/// <summary>
/// ���������� ��������� ��� ����������, ����� � ������ ������������ ��� ��� ��� �� ��� ����� �� ����� �����
/// </summary>
public class DeathPlatformController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<Entity>().OnDeath();
        }
    }
}