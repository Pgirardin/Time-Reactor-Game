using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��������� �����
/// </summary>
public class EnemyController : Entity
{
    private float moveSpeed = 3.5f;
    private float rotationSpeed = -100f;

    private AudioSource backgroundMusic;
    private GameObject target;
    private Queue<Vector3> targetTrajectory = new Queue<Vector3>();

    /// <summary>
    /// �������� �����
    /// </summary>
    public override float Health { get; protected set; } = 1f;

    /// <summary>
    /// ��� ��� ������ ����
    /// </summary>
    public string TargetName { get; set; } = "Player";

    /// <summary>
    /// ����� �� ���� ��������� �� �����
    /// </summary>
    public bool FollowsTheTarget { get; private set; } = true;

    private void Awake()
    {
        var backgroundMusicObject = GameObject.Find("Background Music");
        backgroundMusic = backgroundMusicObject.GetComponent<AudioSource>();
        backgroundMusic.Stop();

        target = GameObject.Find(TargetName);
        StartCoroutine(TrackPlayerTrajectory());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().OnDeath();
        }
    }

    private void FixedUpdate()
    {
        if (FollowsTheTarget)
        {
            FollowTheTarget();
        }

        transform.Rotate(new Vector3(0f, rotationSpeed * Time.fixedDeltaTime, 0f));
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
                targetTrajectory.Enqueue(target.transform.position);
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
            if (Vector3.Distance(transform.position, targetTrajectory.Peek()) <= 0.25f)
            {
                targetTrajectory.Dequeue();
            }
            if (targetTrajectory.Count > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTrajectory.Peek(), moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    /// <summary>
    /// �������� ��� ������ �����
    /// </summary>
    public override void OnDeath()
    {
        Destroy(gameObject);
        backgroundMusic.Play();
    }
}