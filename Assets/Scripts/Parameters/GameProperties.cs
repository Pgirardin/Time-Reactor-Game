using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ������� ��������� � ��������� �������
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
    public static List<int> PassedFloors { get; } = new();

    /// <summary>
    /// ������, ������� ���������� ������� �� ������ ����� ����� �� ��������������� ��������
    /// </summary>
    public static List<bool> DoorOnFloor { get; } = new();

    /// <summary>
    /// ������� �� ����� ���������������� ���������, ��� ���� - ����� �����, �������� - ������� �� ���� �����
    /// </summary>
    public static Dictionary<int, GameObject> GeneratedRooms { get; } = new();

    /// <summary>
    /// ������ ���������� �������� ������
    /// </summary>
    public static int PlayerWeaponsArsenalSize { get; } = 3;

    /// <summary>
    /// ����� ��� � ���� �������, ��� ���� - ��� �������� ���� ��������, � �������� - ��� ��� ��������
    /// </summary>
    public static Dictionary<string, Pool> GeneralPool { get; } = new();

    /// <summary>
    /// �������� ��� ������� ����������
    /// </summary>
    public static void ResetStatistics()
    {
        TimeScale.SharedInstance.SetTimeScale(1f);
        Time.timeScale = 1f;
        AudioListener.pause = false;

        FloorNumber = 0;
        PassedFloors.Clear();
        DoorOnFloor.Clear();
        GeneratedRooms.Clear();
        GeneralPool.Clear();

        GraphicAnalyzerController.AnalyzerIsActive = false;
        GraphicAnalyzerController.StateChanged = null;
    }
}