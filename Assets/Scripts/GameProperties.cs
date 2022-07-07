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
    /// ����������� ��������� �����
    /// </summary>
    public static float AppearanceChance { get; set; } = 0.5f;

    /// <summary>
    /// ������ ���������� ������� ������
    /// </summary>
    public static List<int> PassedFloors { get; } = new List<int>();
}