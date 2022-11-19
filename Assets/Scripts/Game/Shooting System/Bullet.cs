using UnityEngine;

/// <summary>
/// ���������� ����� ����, ����� �� ��, �������������� � ��������� � � �����������
/// </summary>
public class Bullet : MonoBehaviour, ISerializationCallbackReceiver
{
    private Rigidbody rigidBody;
    Vector3 previousPosition;

    [SerializeField] private int damage = 1;
    [SerializeField] private int velocity = 15;
    // ��������� ����, ���������� � �������� ������� �� ���������� ����(��� ��������� ��������� ����� �� ������� ������, ��� 0.5f)
    [SerializeField] private float backRayDistance = 0.5f;

    /// <summary>
    /// ��� ��������, �������� ����������� ������ ������
    /// </summary>
    public PoolOfBullets Pool { get; set; }

    /// <summary>
    /// ���������� ����������� ��������� �����
    /// </summary>
    public int Damage { get; private set; }

    /// <summary>
    /// �������� ����� ����
    /// </summary>
    public int Velocity { get; private set; }

    public void OnBeforeSerialize()
    {
        velocity = Velocity;
        damage = Damage;
    }

    public void OnAfterDeserialize()
    {
        Velocity = velocity;
        Damage = damage;
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        var hitInfo = CheckCollision();
        if (hitInfo != null)
        {
            Pool.ReturnBullet(gameObject);
            PerformCollisionEffects(((RaycastHit)hitInfo).collider);
        }
        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Pool.ReturnBullet(gameObject);
        PerformCollisionEffects(other);
    }

    /// <summary>
    /// ������� ���� ������������ �������
    /// </summary>
    public void GiveBulletKineticEnergy(Vector3 bulletDirection)
    {
        rigidBody.velocity = bulletDirection * velocity;
    }

    /// <summary>
    /// �������� �� ������������ � ������� ��������� (��� �������� � ������� ���������)
    /// </summary>
    private RaycastHit? CheckCollision()
    {
        Vector3 currentTrajectory = (transform.position - previousPosition) / Vector3.Distance(transform.position, previousPosition);
        var backRay = new Ray(transform.position, -currentTrajectory);
        RaycastHit hit;
        int defaultLayerMask = 1;

        if (Physics.Raycast(backRay, out hit, backRayDistance, defaultLayerMask, QueryTriggerInteraction.Ignore))
        {
            return hit;
        }
        return null;
    }

    /// <summary>
    /// ���������� �������� ��� �������� ����� ������������ ���� � ���
    /// </summary>
    private void PerformCollisionEffects(Collider hitObjectCollider)
    {
        var entity = hitObjectCollider.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage);
        }
    }
}