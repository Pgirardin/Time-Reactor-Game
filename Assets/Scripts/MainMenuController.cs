using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ���������� ��������� �� ���������� ����
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI probabilityText;

    /// <summary>
    /// ���������� ������� ����������� ��������� ����� �� ��������
    /// </summary>
    public void DisplayProbabilityFromSlider()
    {
        probabilityText.text = slider.value.ToString() + " %";
    }

    /// <summary>
    /// ��������� ������� �����
    /// </summary>
    public void StartGame()
    {
        GameProperties.EnemyAppearanceChance = slider.value / 100f;
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void QuitFromGame()
    {
        Application.Quit();
    }
}