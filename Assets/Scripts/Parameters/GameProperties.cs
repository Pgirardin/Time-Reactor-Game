using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ������� ���������
/// </summary>
public static class GameProperties
{
    /// <summary>
    /// ����, �� ������� ������ ��������� �����
    /// </summary>
    public static int FloorNumber { get; set; } = 0;

    /// <summary>
    /// ������ ���������� ������� ������
    /// </summary>
    public static List<int> PassedFloors { get; } = new List<int>();

    /// <summary>
    /// ����������� ��������� �����
    /// </summary>
    public static float EnemyAppearanceChance { get; set; } = 0.5f;

    /// <summary>
    /// ������ ���������� �������� ������
    /// </summary>
    public static int PlayerWeaponsArsenalSize { get; } = 3;

    /// <summary>
    /// �������� ��� ������� ����������
    /// </summary>
    public static void ResetStatistics()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        FloorNumber = 0;
        PassedFloors.Clear();
    }
}