using UnityEngine;

/// <summary>
/// ���������� ��������� ���������� ��������� �������������� ��������� �������
/// </summary>
public class PickerController : MonoBehaviour
{
    [SerializeField] private Entity player;
    [SerializeField] private GameObject graphicAnalyzer;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            PickUpItem();
        }
    }

    /// <summary>
    /// ��������� �������, �� ������� ������� �����, � � ����������� �� ���� �������� ��������� ����������� ��������
    /// </summary>
    private void PickUpItem()
    {
        (var objectAhead, var _) = graphicAnalyzer.GetComponent<GraphicAnalyzerController>().GetObjectPlayerIsLookingAt();
        if (objectAhead == null)
        {
            return;
        }

        var medkitComponent = objectAhead.GetComponent<Medkit>();
        if (medkitComponent != null)
        {
            player.Heal(medkitComponent.RecoveryPoints);
            medkitComponent.PickUp();
        }
    }
}