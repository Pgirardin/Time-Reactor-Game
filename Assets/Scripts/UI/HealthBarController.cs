using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

/// <summary>
/// ���������� ����� �������� � ������ ��������
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthCount;

    private void Awake()
    {
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
        healthCount.text = Math.Ceiling(health).ToString() + " HP";
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