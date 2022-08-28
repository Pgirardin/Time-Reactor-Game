using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���������� ��������� �� ���� ����� � ����
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject elements;

    private bool gameOnPause = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameOnPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    /// <summary>
    /// �������� ��� ��������� ���� �� �����
    /// </summary>
    public void Pause()
    {
        elements.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        AudioListener.pause = true;
        gameOnPause = true;
    }

    /// <summary>
    /// �������� ��� ����������� ���� ����� �����
    /// </summary>
    public void Resume()
    {
        elements.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
        gameOnPause = false;
    }

    /// <summary>
    /// ��������� ������� ����
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        GameProperties.ResetStatistics();
    }

    /// <summary>
    /// ����� �� ����
    /// </summary>
    public void QuitFromGame()
    {
        Application.Quit();
    }
}