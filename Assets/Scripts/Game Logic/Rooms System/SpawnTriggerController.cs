using UnityEngine;

/// <summary>
/// ���������� �������� ��� ����� � �������, ������� ������ ������
/// </summary>
public class SpawnTriggerController : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    private bool triggerWasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !triggerWasActivated)
        {
            spawner.InstantiateRandomEnemies();
            triggerWasActivated = true;
        }
    }
}