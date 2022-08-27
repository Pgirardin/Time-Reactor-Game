using UnityEngine;

/// <summary>
/// ���������� ����������� ����� ��������
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// �������� ��������
    /// </summary>
    public abstract float Health { get; protected set; }

    /// <summary>
    /// �������� ����
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// �������� ��� ������ ��������
    /// </summary>
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}