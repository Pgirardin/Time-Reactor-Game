using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���������� �����, � ������� ����� ��������� ����
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new();

    /// <summary>
    /// ������ �������� ������, ������� ����� ��������� � ������ �����
    /// </summary>
    public List<GameObject> EnemyPrefabs { get => enemyPrefabs; }
}