using UnityEngine;

/// <summary>
/// ���������� �������� ������ ������ � �������� ��������������� � ������������� ��� �������
/// </summary>
public class GroundCheckerController : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private StaminaController staminaController;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float staminaCostPerJump;

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
        CheckJump();
    }

    /// <summary>
    /// ���������� �������� ������ � �������� �� ��� �����������
    /// </summary>
    private void CheckJump()
    {
        if (staminaController.Tired)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space) && IsGrounded)
        {
            IsGrounded = false;
            // ������� �������� ����, ���������� � ����������� ����������� �����
            player.velocity = new Vector3(player.velocity.x, Mathf.Sqrt(2f * -GravitationController.GlobalGravity * jumpHeight), player.velocity.z);
            staminaController.Stamina -= staminaCostPerJump;
        }
    }
}