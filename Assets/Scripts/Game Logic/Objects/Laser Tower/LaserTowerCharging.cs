using UnityEngine;

/// <summary>
/// ��������� ��������� �������� ����� � �������� ������� � ��������
/// ���� ������ ������ ��������� � �������� ����� (� �������) ����� ��������� ����, �� ��� ����� ���������� � �������
/// ���������� ������� � ��������� ���������� ��������� ���������, � ����� ��������� ����; ����� �� ����� �����
/// ����������� ����� ��� ��������� ���� � ���� ������ �����
/// </summary>
public class LaserTowerCharging : MonoBehaviour
{
    [SerializeField] private GameObject laserSource;
    [SerializeField] private Material laserSourceChargedState;
    [SerializeField] private Material laserSourceDischargedState;
    [SerializeField] private float chargingDuration;

    // ������� ������������ ��������� ��������� (�� 0 �� 1)
    private float chargeDegree = 0f;
    private Renderer render;
    private LaserTowerAttack behaviourOnAttack;

    private void Awake()
    {
        render = laserSource.GetComponent<Renderer>();
        render.material = laserSourceDischargedState;
        behaviourOnAttack = GetComponent<LaserTowerAttack>();
    }

    private void FixedUpdate()
    {
        if (behaviourOnAttack.TargetInSight)
        {
            LaserSourceCharging();
        }
        else
        {
            LaserSourceDischarging();
        }

        if (IsTowerCharged())
        {
            behaviourOnAttack.TowerCharged = true;
        }
        else
        {
            behaviourOnAttack.TowerCharged = false;
        }
    }

    /// <summary>
    /// ������� ������� ��������� ���������
    /// </summary>
    private void LaserSourceCharging()
    {
        chargeDegree += (1f / chargingDuration) * Time.fixedDeltaTime;
        chargeDegree = Mathf.Clamp(chargeDegree, 0, 1);

        render.material.Lerp(laserSourceDischargedState, laserSourceChargedState, chargeDegree);
    }

    /// <summary>
    /// ������� �������� ��������� ���������
    /// </summary>
    private void LaserSourceDischarging()
    {
        chargeDegree -= (1f / chargingDuration) * Time.fixedDeltaTime;
        chargeDegree = Mathf.Clamp(chargeDegree, 0, 1);

        render.material.Lerp(laserSourceChargedState, laserSourceDischargedState, 1f - chargeDegree);
    }

    /// <summary>
    /// ���������, �������� �� �������� ����� � ������ ������
    /// </summary>
    private bool IsTowerCharged()
    {
        return Mathf.Abs(chargeDegree - 1) < 0.0001f;
    }
}