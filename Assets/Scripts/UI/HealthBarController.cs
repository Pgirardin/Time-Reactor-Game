using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���������� ����� �������� � ������ ��������
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Entity entity;

    private Image healthBar;
    private TextMeshProUGUI healthCount;

    private void Awake()
    {
        healthBar = transform.Find("Health Bar").Find("Health Bar Background").Find("Health Bar Foreground").GetComponent<Image>();
        healthCount = transform.Find("Health Bar").Find("Health Count").GetComponent<TextMeshProUGUI>();
        entity.HealthChanged += RedrawHealthInfo;
        GraphicAnalyzerController.StateChanged += ShowOrHideHealthBar;
        ShowOrHideHealthBar(GraphicAnalyzerController.AnalyzerIsActive);
    }

    private void FixedUpdate()
    {
        SetRotationOfHealthBar();
    }

    private void OnDestroy()
    {
        GraphicAnalyzerController.StateChanged -= ShowOrHideHealthBar;
    }

    /// <summary>
    /// ������������ ������� � �������� ��������� �������� �������� ��������
    /// </summary>
    private void RedrawHealthInfo(float health)
    {
        healthBar.fillAmount = health / entity.MaxHealth;
        healthCount.text = health.ToString() + " HP";
    }

    /// <summary>
    /// ��������� ������ �������� �������� ���, ����� ��� ���� ����������� ��������� ������ ������
    /// </summary>
    private void SetRotationOfHealthBar()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// �������� ��� ������ ����� �������� �������� � ����������� �� ������ ������������ ����������� ������
    /// </summary>
    private void ShowOrHideHealthBar(bool graphicAnalyzerIsActive)
    {
        if (entity.name != "Player")
        {
            gameObject.SetActive(graphicAnalyzerIsActive);
        }
    }
}