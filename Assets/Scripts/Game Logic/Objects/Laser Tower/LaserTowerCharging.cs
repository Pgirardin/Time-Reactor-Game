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

    private AudioSource chargingSound;

    private void Awake()
    {
        render = laserSource.GetComponent<Renderer>();
        render.material = laserSourceDischargedState;
        behaviourOnAttack = GetComponent<LaserTowerAttack>();

        foreach (var audioSource in GetComponents<AudioSource>())
        {
            var clipName = audioSource.clip.name;
            if (clipName == "Laser Tower Charging")
            {
                chargingSound = audioSource;
            }
        }
    }

    private void FixedUpdate()
    {
        if (behaviourOnAttack.TargetInSight)
        {
            LaserSourceCharging();
            PlayCharhingSound();
        }
        else
        {
            LaserSourceDischarging();
            PlayCharhingSound();
        }

        if (IsTowerCharged())
        {
            behaviourOnAttack.TowerCharged = true;
            chargingSound.Stop();
        }
        else
        {
            behaviourOnAttack.TowerCharged = false;
        }

        if (IsTowerDischarged())
        {
            chargingSound.Stop();
        }
    }

    /// <summary>
    /// ������� ������� ��������� ���������
    /// </summary>
    private void LaserSourceCharging()
    {
        chargeDegree += (1f / chargingDuration) * Time.fixedDeltaTime * TimeScale.Scale;
        chargeDegree = Mathf.Clamp(chargeDegree, 0, 1);

        render.material.Lerp(laserSourceDischargedState, laserSourceChargedState, chargeDegree);
    }

    /// <summary>
    /// ������� �������� ��������� ���������
    /// </summary>
    private void LaserSourceDischarging()
    {
        chargeDegree -= (1f / chargingDuration) * Time.fixedDeltaTime * TimeScale.Scale;
        chargeDegree = Mathf.Clamp(chargeDegree, 0, 1);

        render.material.Lerp(laserSourceChargedState, laserSourceDischargedState, 1f - chargeDegree);
    }

    /// <summary>
    /// ���������, ��������� �� �������� �������� ����� � ������ ������
    /// </summary>
    private bool IsTowerCharged()
    {
        return Mathf.Abs(chargeDegree - 1) < 0.0001f;
    }

    /// <summary>
    /// ���������, ��������� �� ��������� �������� ����� � ������ ������
    /// </summary>
    private bool IsTowerDischarged()
    {
        return chargeDegree < 0.0001f;
    }

    /// <summary>
    /// ��������� ���� �������/�������� �������� �����
    /// </summary>
    private void PlayCharhingSound()
    {
        if (!chargingSound.isPlaying)
        {
            chargingSound.Play();
        }
    }
}