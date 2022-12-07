using UnityEngine;
using System.Collections;

/// <summary>
/// ��������� ��������� �������� ����� ��� ����� ����
/// </summary>
public class LaserTowerAttack : MonoBehaviour
{
    [SerializeField] private Transform emitter;
    [SerializeField] private string targetName;
    // �������� ����, ��� �������� �������� ����
    [SerializeField] private string nameOfLaserPool;

    [SerializeField] private float reachRadius;
    // �������� ����� ����� ����������� ����� ����
    [SerializeField] private float hitsInterval;
    [SerializeField] private float damagePerSecond;

    private Transform target;
    private Pool laserPool;
    // �������� ��� ��� ������ �������� �����
    private GameObject laser;
    private bool onAttack = false;

    /// <summary>
    /// ��������� �� ���� ��� �������� ����� 
    /// </summary>
    public bool TargetInSight { get; set; }

    /// <summary>
    /// ������ �� � ����� ����� � ������ ������
    /// </summary>
    public bool TowerCharged { get; set; } = false;

    private void Awake()
    {
        var chargingComponent = GetComponent<LaserTowerCharging>();

        // ���� �� �������� ����� ��� ���������� LaserTowerCharging, �� ��� ������
        // ����� �������� � ����� �����, ��� ������ ���� ������ � � ���� ������
        if (chargingComponent == null || !chargingComponent.enabled)
        {
            TowerCharged = true;
        }
    }

    private void Start()
    {
        target = GameObject.Find(targetName).transform;
        laserPool = GameProperties.GeneralPool[nameOfLaserPool];
    }

    private void FixedUpdate()
    {
        if (IsTargetAvailableForAttack())
        {
            TargetInSight = true;
        }
        else
        {
            TargetInSight = false;
        }

        if (TowerCharged)
        {
            TryToAttackTarget();
        }
    }

    /// <summary>
    /// ��������� ������������ ���� ��� �����
    /// </summary>
    private bool IsTargetAvailableForAttack()
    {
        var layerMask = LayerMask.GetMask("Default", "Player");
        var laserDirection = (target.position - emitter.position).normalized;

        if (Physics.Raycast(emitter.position, laserDirection, out RaycastHit hit, reachRadius, layerMask, QueryTriggerInteraction.Ignore)
            && hit.transform.name == targetName)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// �������� ���� ������ ����� ���������� �������
    /// </summary>
    private IEnumerator DealDamageToTarget()
    {
        while (true)
        {
            var entityComponent = target.GetComponent<Entity>();
            if (entityComponent != null)
            {
                entityComponent.TakeDamage(damagePerSecond * hitsInterval);
                yield return new WaitForSeconds(hitsInterval);
            }
        }
    }

    /// <summary>
    /// ���������� �������� ��� ����� ����������� � �����
    /// </summary>
    private void DrawLaserRay()
    {
        var laserLength = Vector3.Distance(target.position, emitter.position);
        var laserDirection = (target.position - emitter.position).normalized;
        var laserRotation = Quaternion.LookRotation(laserDirection);

        laser.transform.position = emitter.position;
        laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, laserLength);
        laser.transform.rotation = laserRotation;
    }

    /// <summary>
    /// ������ ����� ����, ���� ��� ��������
    /// </summary>
    private void TryToAttackTarget()
    {
        if (TargetInSight)
        {
            if (!onAttack)
            {
                laser = laserPool.GetObject();
                onAttack = true;
                StartCoroutine("DealDamageToTarget");
            }

            DrawLaserRay();
        }
        else
        {
            if (!onAttack)
            {
                return;
            }

            laserPool.ReturnObject(laser);
            onAttack = false;
            StopCoroutine("DealDamageToTarget");
        }
    }
}