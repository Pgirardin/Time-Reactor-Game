using UnityEngine;
using System;

/// <summary>
/// ���������� ����������� ����� ��������
/// </summary>
public abstract class Entity : ObjectWithInformation, ISerializationCallbackReceiver
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float maxHealth = 100f;

    private void Start()
    {
        // ������������� ����� ������� �������� ���������� �������� � ������������ ����������� ��������
        Health = MaxHealth;
    }

    /// <summary>
    /// ������� �������� ��������
    /// </summary>
    public virtual float Health
    {
        get { return health; }
        protected set
        {
            if (value <= 0f)
            {
                OnDeath();
            }
            else if (value > MaxHealth)
            {
                return;
            }
            health = value;
            if (HealthChanged != null)
            {
                HealthChanged(Health);
            }
        }
    }

    /// <summary>
    /// ������������ �������� ��������
    /// </summary>
    public virtual float MaxHealth
    {
        get { return maxHealth; }
        protected set { maxHealth = value; }
    }

    /// <summary>
    /// ������� ��������� �������� ��������
    /// </summary>
    public Action<float> HealthChanged { get; set; }

    public virtual void OnBeforeSerialize()
    {
        health = Health;
        maxHealth = MaxHealth;
    }

    public virtual void OnAfterDeserialize()
    {
        Health = health;
        MaxHealth = maxHealth;
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
    }

    /// <summary>
    /// �������� ��� ������ ��������
    /// </summary>
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}