using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���������� ��������� �� �������� ����
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// ��������� ������� �����
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// ������� ���� ��������
    /// </summary>
    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene("Settings Menu");
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void QuitFromGame()
    {
        Application.Quit();
    }
}