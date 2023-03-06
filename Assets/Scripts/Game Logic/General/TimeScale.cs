using UnityEngine;
using System.Collections;

/// <summary>
/// ���������� �������� ������� ������� (������ ��� Time.timeScale)
/// </summary>
public class TimeScale : MonoBehaviour
{
    /// <summary>
    /// �������� ������� �������. ��� ��������, ������� �������� � ������� ���������� �������
    /// (��������, �������� �������), ������ ����������� �� TimeScale.Scale, ����� ����� �����������
    /// �� ������� �������
    /// </summary>
    public static float Scale { get; private set; } = 1f;

    /// <summary>
    /// �������� ������� ������� � ���������/�������� ������ ��������
    /// </summary>
    public static void SetTimeScale(float scale)
    {
        Scale = scale;
        // pass
    }

    /// <summary>
    /// ��������� ������������ WaitForSeconds() ��� ���������� ������ ��� ��������� ������� �������
    /// � ������� ������� ������ (��� ��� Time.timeScale �� ����������, ����������� WaitForSeconds()
    /// ������ ����� ����������� ��������� ���������� ������ ��������� �������, ��� �� �������� ��� ���������� ������)
    /// </summary>
    public static IEnumerator WaitForSeconds(float seconds)
    {
        float numberOfSecondsElapsed = 0f;

        while (numberOfSecondsElapsed < seconds)
        {
            // ����, ��������, Scale = 0.1f (����� ��� � 10 ��� ���������), �� ��������� ������� ������ ������
            // � 10 ��� ������, ������ ��� numberOfSecondsElapsed ������ >= seconds
            numberOfSecondsElapsed += Time.deltaTime * Scale; // ����� ������������ ������ Time.deltaTime, � �� Time.fixedDeltaTime
            yield return null;
        }
    }
}