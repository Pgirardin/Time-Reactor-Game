using UnityEngine;
using System.Collections;

/// <summary>
/// ���������� ���������� �������� (������� �������� � ����)
/// </summary>
public class TimeManagerController : MonoBehaviour
{
    // �� ������� ��� ����� ���� ��������� ����� (����������� ����� ����� �� 1 �� 20)
    [SerializeField] private float timeSlowdownFactor;
    // ������������ ����������� ��������� �����
    [SerializeField] private float slowdownDuration;
    // ����� ����������� ����������� ��������� ����� (� ��������)
    [SerializeField] private float abilityCooldown;

    private float standardFixedDeltaTime;
    private bool abilityCanBeUsed = true;

    private void Awake()
    {
        standardFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        CheckForAbilityUsing();
    }

    /// <summary>
    /// ������������ ������� ������� ���� ��� ����������� ������������� �����������
    /// </summary>
    private void CheckForAbilityUsing()
    {
        if (Input.GetMouseButtonDown(1) && abilityCanBeUsed)
        {
            SlowdownTime();
        }
    }

    /// <summary>
    /// ��������� ����� �� ��������� ������
    /// </summary>
    private void SlowdownTime()
    {
        TimeScale.SharedInstance.SetTimeScale(1 / timeSlowdownFactor);
        abilityCanBeUsed = false;
        StartCoroutine(RevertToStandardTimescaleAfterAbilityPassing());
    }

    /// <summary>
    /// �� ���������� ������� �������� ����������� ������� ����������� ������� �������
    /// </summary>
    private IEnumerator RevertToStandardTimescaleAfterAbilityPassing()
    {
        yield return new WaitForSeconds(slowdownDuration);
        TimeScale.SharedInstance.SetTimeScale(1f);
        StartCoroutine(ImplementCooldownMechanic());
    }

    /// <summary>
    /// ���������� ������� ����� ������������ ����� ������������ �����������
    /// </summary>
    /// <returns></returns>
    private IEnumerator ImplementCooldownMechanic()
    {
        yield return new WaitForSeconds(abilityCooldown);
        abilityCanBeUsed = true;
    }
}