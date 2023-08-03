using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ���������� �������� ������������ � ��������� � ������
/// </summary>
public class StaminaController : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegeneratingPerSecond;
    private float stamina;

    /// <summary>
    /// ���������� �� ������ ������ ���������� ��� ������ (���� Stamina = 0, �� ���������� ������ Tired,
    /// � ����� �� ����� ������ � ������� �� �������, ����� ������ ����� �� ������������� �� ����. ��������)
    /// </summary>
    public float Stamina
    {
        get { return stamina; }
        set
        {
            if (Stamina == MaxStamina && value < MaxStamina)
            {
                StaminaBecameLessThanMax.Invoke();
            }

            stamina = value;
            if (stamina <= 0)
            {
                stamina = 0;
                Tired = true;
                StaminaExhausted.Invoke();
            }
            if (stamina >= MaxStamina)
            {
                stamina = MaxStamina;
                Tired = false;
                StaminaRestored.Invoke();
            }

            StaminaChanged.Invoke(Stamina);
        }
    }

    /// <summary>
    /// ������� ��������� ����� ������������
    /// </summary>
    public UnityEvent<float> StaminaChanged = new UnityEvent<float>();

    /// <summary>
    /// ����������, ����� �������� ������������ ���������� ������ �������������
    /// </summary>
    public UnityEvent StaminaBecameLessThanMax = new UnityEvent();

    /// <summary>
    /// ����������, ����� ������������ ��������� �����������
    /// </summary>
    public UnityEvent StaminaExhausted = new UnityEvent();

    /// <summary>
    /// ����������, ����� ������������ ��������� �����������������
    /// </summary>
    public UnityEvent StaminaRestored = new UnityEvent();

    /// <summary>
    /// ������������ �������� ����� ������������
    /// </summary>
    public float MaxStamina => maxStamina;

    /// <summary>
    /// ����� �� ����������������� ����� ������������ � ������ ������
    /// </summary>
    public bool StaminaCanRegenerating { get; set; } = true;

    /// <summary>
    /// ������ �� ����� � ������ ������
    /// </summary>
    public bool Tired { get; private set; } = false;

    private void Awake()
    {
        Stamina = MaxStamina;
    }

    private void FixedUpdate()
    {
        TryToRegenerateStamina();
    }

    /// <summary>
    /// ��������������� ����� ������������, ���� ��� ��������
    /// </summary>
    private void TryToRegenerateStamina()
    {
        if (StaminaCanRegenerating)
        {
            Stamina += staminaRegeneratingPerSecond * Time.fixedDeltaTime;
        }
    }
}