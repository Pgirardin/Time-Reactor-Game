using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

/// <summary>
/// ���������� ���������� �������� (������� �������� � ����)
/// </summary>
public class TimeManagerController : MonoBehaviour
{
    // ����������� ���������� ������� �� ������
    [SerializeField] private TextMeshProUGUI TSFCounter;
    [SerializeField] private Image timeSlowdownTimer;

    private float timeSlowdownFactor = 1f;
    // ������������ ����������� ��������� �����
    [SerializeField] private float slowdownDuration;
    // ����� ����������� ����������� ��������� ����� (� ��������)
    [SerializeField] private float abilityCooldown;

    private bool abilityCanBeUsed = true;

    /// <summary>
    /// �� ������� ��� ����� ���� ��������� ����� (����������� ����� ����� �� 1 �� 20)
    /// </summary>
    public float TimeSlowdownFactor
    {
        get { return timeSlowdownFactor; }
        set
        {
            timeSlowdownFactor = (float)Math.Round(value, 2);
            TSFCounter.text = value.ToString();
        }
    }

    /// <summary>
    /// ����� ��������� ������ ��� ������ �������
    /// </summary>
    public static TimeManagerController SharedInstance { get; private set; }

    private void Awake()
    {
        SharedInstance = this;
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
        if (TimeSlowdownFactor == 1f)
        {
            return;
        }

        TimeScale.SharedInstance.SetTimeScale(1 / TimeSlowdownFactor);
        abilityCanBeUsed = false;
        StartCoroutine(RevertToStandardTimescaleAfterAbilityPassing());
        StartCoroutine(DisplayTimeSlowdownTimer());
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

    /// <summary>
    /// ���������� ���������� �������� ������ �� ����� ���������� �������
    /// </summary>
    private IEnumerator DisplayTimeSlowdownTimer()
    {
        float timeLeft = slowdownDuration;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            timeSlowdownTimer.fillAmount = timeLeft / slowdownDuration;
            yield return null;
        }
    }
}