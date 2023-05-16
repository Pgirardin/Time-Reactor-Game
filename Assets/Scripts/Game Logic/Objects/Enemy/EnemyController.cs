using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��������� �����
/// </summary>
public class EnemyController : Entity, ISerializationCallbackReceiver
{
    [SerializeField] private string enemyType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private string targetName;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private bool followsTheTarget;
    [SerializeField] private bool killTargetOnTouch;

    private AudioSource backgroundMusic;
    private GameObject target;
    private Queue<Vector3> targetTrajectory = new Queue<Vector3>();

    /// <summary>
    /// ��� ��� ������ ����
    /// </summary>
    public string TargetName { get; set; }

    /// <summary>
    /// ����� �� ���� ��������� �� �����
    /// </summary>
    public bool FollowsTheTarget { get; private set; }

    public override string[,] ObjectInfoParameters { get; set; } = null;

    public override string ObjectInfoHeader { get; set; } = null;

    public override Color ObjectInfoHeaderColor { get; set; } = Color.red;

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
        targetName = TargetName;
        followsTheTarget = FollowsTheTarget;
    }

    public override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
        TargetName = targetName;
        FollowsTheTarget = followsTheTarget;
    }

    private void Awake()
    {
        ObjectInfoHeader = enemyType;
        string infoAboutDamage;
        if (killTargetOnTouch)
        {
            infoAboutDamage = "One punch to death";
        }
        else
        {
            infoAboutDamage = damagePerSecond.ToString() + " HP/s";
        }
        ObjectInfoParameters = new string[3, 2] { { "Max health:", MaxHealth.ToString() + " HP" },
                                                  { "Damage:", infoAboutDamage },
                                                  { "Movement speed:", moveSpeed.ToString() + " m/s" } };
        InitializeInfoPanelPrefab();

        foreach (var audioSource in GetComponents<AudioSource>())
        {
            audioSource.pitch = TimeScale.SharedInstance.Scale;
        }

        target = GameObject.Find(TargetName);
        StartCoroutine(TrackPlayerTrajectory());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == TargetName)
        {
            if (killTargetOnTouch)
            {
                collision.gameObject.GetComponent<Entity>().OnDeath();
            }
            else
            {
                var damagePerTouch = damagePerSecond * Time.fixedDeltaTime * TimeScale.SharedInstance.Scale;
                collision.gameObject.GetComponent<Entity>().TakeDamage(damagePerTouch);
            }
        }
    }

    private void FixedUpdate()
    {
        if (FollowsTheTarget)
        {
            FollowTheTarget();
        }

        transform.Rotate(new Vector3(0f, rotationSpeed * Time.fixedDeltaTime * TimeScale.SharedInstance.Scale, 0f));
    }

    /// <summary>
    /// ����������� ���������� ������ � ��������� ����� ��������� ��� � ��������� �����������
    /// </summary>
    private IEnumerator TrackPlayerTrajectory()
    {
        var lastAddedPoint = Vector3.zero;
        while (true)
        {
            if (targetTrajectory.Count == 0 || targetTrajectory.Count > 0 && Vector3.Distance(target.transform.position, lastAddedPoint) >= 0.25f)
            {
                // + new Vector3(...) - �������, ��� ��� ����� ����� ��������� ��� ���, ������� ���� ������
                // ���������� � �����, ������� ���� ������ ������
                targetTrajectory.Enqueue(target.transform.position + new Vector3(0, -1.5f, 0));
                lastAddedPoint = target.transform.position;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// ��������� �� �����
    /// </summary>
    private void FollowTheTarget()
    {
        if (targetTrajectory.Count > 0)
        {
            // ���������� ��� ���������� ������ ����� ���������� ������ �� �������, ��� ��� ����� ���� ��������� � ����� � �� ����� ������� �� �����
            if (Vector3.Distance(transform.position, targetTrajectory.Peek()) <= 1f)
            {
                targetTrajectory.Dequeue();
            }
            if (targetTrajectory.Count > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTrajectory.Peek(), moveSpeed * Time.fixedDeltaTime * TimeScale.SharedInstance.Scale);
            }
        }
    }
}