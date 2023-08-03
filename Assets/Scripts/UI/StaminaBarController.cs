using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���������� ����� ������������
/// </summary>
public class StaminaBarController : MonoBehaviour
{
    [SerializeField] private StaminaController staminaController;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image staminaBarBackground;

    [SerializeField] private Sprite barSpriteForNormalState;
    [SerializeField] private Sprite barBackgroundSpriteForNormalState;
    [SerializeField] private Sprite barSpriteForTiredState;
    [SerializeField] private Sprite barBackgroundSpriteForTiredState;

    /// <summary>
    /// ������������ ����� ������������
    /// </summary>
    public void RedrawStaminaBar(float stamina)
    {
        staminaBar.fillAmount = staminaController.Stamina / staminaController.MaxStamina;
    }

    /// <summary>
    /// �������� ����� ������������
    /// </summary>
    public void ShowStaminaBar()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ������ ����� ������������
    /// </summary>
    public void HideStaminaBar()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �������� ����� ����� ������������ �� ������� �����
    /// </summary>
    public void SwapBarToNormalState()
    {
        staminaBar.sprite = barSpriteForNormalState;
        staminaBarBackground.sprite = barBackgroundSpriteForNormalState;
    }

    /// <summary>
    /// �������� ����� ����� ������������ �� ����� �� ����� ��������� ������
    /// </summary>
    public void SwapBarToTiredState()
    {
        staminaBar.sprite = barSpriteForTiredState;
        staminaBarBackground.sprite = barBackgroundSpriteForTiredState;
    }
}