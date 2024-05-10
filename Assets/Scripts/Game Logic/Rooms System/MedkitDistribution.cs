using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���������� ������������� ���������� ���������� ������� � ����������� ������ �������
/// </summary>
public class MedkitDistribution : MonoBehaviour
{
    [SerializeField] private GameObject medkitPrefab;
    // ����� � �������, ��� ����� ���������� �������
    [SerializeField] private List<Transform> medkitPositions = new();
    // ����������� � ������������ ���������� �������, ������� ����� ��������� � ������ �������
    [SerializeField] private int minCount = 0;
    [SerializeField] private int maxCount = 2;

    private System.Random random = new();

    private void Awake()
    {
        ShuffleMedkitPositions();
    }

    private void Start()
    {
       DistributeMedkits();
    }

    /// <summary>
    /// ���������� ������� ��������� ������� � ������ medkitPositions
    /// </summary>
    private void ShuffleMedkitPositions()
    {
        for (int i = medkitPositions.Count - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            var temp = medkitPositions[j];
            medkitPositions[j] = medkitPositions[i];
            medkitPositions[i] = temp;
        }
    }

    /// <summary>
    /// ������������ ������� � ��������� �������� ������� (����� �������� �������� ������� � medkitPositions)
    /// </summary>
    private void DistributeMedkits()
    {
        int resultCount = random.Next(minCount, maxCount + 1);
        for (int i = 0; i < resultCount; i++)
        {
            Instantiate(medkitPrefab, medkitPositions[i].position, medkitPositions[i].rotation);
        }
    }
}