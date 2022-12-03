using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// ���������� ������� � ��������� ����-���������
/// </summary>
public class PostProcessCameraController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<PostProcessLayer>().enabled = SettingsOptions.ImprovedGraphicsIsOn;
        GetComponent<PostProcessVolume>().enabled = SettingsOptions.ImprovedGraphicsIsOn;
    }
}