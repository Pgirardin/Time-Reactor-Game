using UnityEngine;

/// <summary>
/// ���������� �������� ���������� ��� �������� � Rigidbody
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GravitationController : MonoBehaviour
{
    [SerializeField] private float gravityScale = 1f;

    private Rigidbody rigidBody;

    /// <summary>
    /// ������� ��������� ���������� �������
    /// </summary>
    public static float GlobalGravity => -9.81f;

    /// <summary>
    /// ��������� ���������� ��� ������� �������
    /// </summary>
    public float GravityScale
    {
        get { return gravityScale; }
        set { gravityScale = value; }
    }

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }

    private void FixedUpdate()
    {
        Vector3 gravityVector = GlobalGravity * GravityScale * Vector3.up;
        rigidBody.AddForce(gravityVector, ForceMode.Acceleration);
    }
}