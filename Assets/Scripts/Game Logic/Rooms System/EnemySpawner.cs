using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���������� �������� ��������� ������ � ����������� ������ ������
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    // ����� � �������, ��� ����� ���������� �����
    [SerializeField] private List<SpawnPoint> spawnPositions = new();
    // ���������� ������, ������� ������ ��������� � ����� ������� ��� ������ � ��
    [SerializeField] private int enemyCount;

    private System.Random random = new();

    private void Awake()
    {
        ShuffleSpawnPositions();
    }

    /// <summary>
    /// ���������� ������� ������ ������ � ������ spawnPositions
    /// </summary>
    private void ShuffleSpawnPositions()
    {
        for (int i = spawnPositions.Count - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            var temp = spawnPositions[j];
            spawnPositions[j] = spawnPositions[i];
            spawnPositions[i] = temp;
        }
    }

    /// <summary>
    /// ������� ��������� ������ � ��������� �������� ������� (����� �������� �������� ������� � spawnPositions)
    /// </summary>
    public void InstantiateRandomEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // ������ �������� ������, ������� ����� �������� � ������ �����
            var pointPrefabs = spawnPositions[i].EnemyPrefabs;
            var randomEnemyPrefab = pointPrefabs[random.Next(pointPrefabs.Count)];
            Instantiate(randomEnemyPrefab, spawnPositions[i].transform.position, Quaternion.identity);
        }
    }
}