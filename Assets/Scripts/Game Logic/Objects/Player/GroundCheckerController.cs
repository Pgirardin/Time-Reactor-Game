using UnityEngine;

/// <summary>
/// ���������� �������� ������ ������ � �������� ��������������� � ������������� ��� �������
/// </summary>
public class GroundCheckerController : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private float jumpHeight;

    /// <summary>
    /// ��������� �� ����� � ������ ������ �� �����-���� �����������
    /// </summary>
    public bool IsGrounded { get; private set; } = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.name != "Player")
        {
            IsGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }
    }

    /// <summary>
    /// ��������� ������
    /// </summary>
    private void Jump()
    {
        IsGrounded = false;
        // ������� �������� ����, ���������� � ����������� ����������� �����
        player.velocity = new Vector3(player.velocity.x, Mathf.Sqrt(2f * -GravitationController.GlobalGravity * jumpHeight), player.velocity.z);
    }
}