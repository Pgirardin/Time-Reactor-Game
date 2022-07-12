using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ���������� ��������� �� ���� ��������
/// </summary>
public class SettingsMenuController : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValue;

    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI mouseSensitivityValue;

    public Toggle improvedGraphicsToggle;

    private void Start()
    {

        volumeSlider.value = SettingsOptions.GeneralVolume * 100;
        volumeValue.text = (SettingsOptions.GeneralVolume * 100).ToString();

        mouseSensitivitySlider.value = SettingsOptions.MouseSensitivity;
        mouseSensitivityValue.text = SettingsOptions.MouseSensitivity.ToString();

        improvedGraphicsToggle.isOn = SettingsOptions.ImprovedGraphicsIsOn;
    }

    /// <summary>
    /// �������� �������� ��������� (��� ��������� ��������)
    /// </summary>
    public void ChangeVolumeValue()
    {
        SettingsOptions.GeneralVolume = volumeSlider.value / 100f;
        volumeValue.text = volumeSlider.value.ToString();
    }

    /// <summary>
    /// �������� �������� ���������������� ���� (��� ��������� ��������)
    /// </summary>
    public void ChangeMouseSensitivityValue()
    {
        SettingsOptions.MouseSensitivity = mouseSensitivitySlider.value;
        mouseSensitivityValue.text = mouseSensitivitySlider.value.ToString();
    }

    /// <summary>
    /// �������� ��� ������� (��� ��������� ��������� ������)
    /// </summary>
    public void ChangeGraphics()
    {
        SettingsOptions.ImprovedGraphicsIsOn = improvedGraphicsToggle.isOn;
    }

    /// <summary>
    /// ��������� � ������� ����
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}