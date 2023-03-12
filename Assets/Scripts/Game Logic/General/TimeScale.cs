using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���������� �������� ������� ������� (������ ��� Time.timeScale)
/// </summary>
public class TimeScale : MonoBehaviour
{
    private AudioSource timeSlowdownSound;
    private AudioSource timeAccelerationSound;

    // ����� ���������� �����, �� ������� �� ��������� ������ ����������/��������� �������
    private List<string> sourcesWithoutPitchChange = new()
    {
        "Player Steps Sound", "Gun Shot", "Gun Reloading", "Weapon Picking Up",
        "Assault Riffle Shot", "Assault Riffle Reloading", "Activation", "Deactivation",
        "Time Slowdown", "Time Acceleration"
    };

    /// <summary>
    /// �������� ������� �������. ��� ��������, ������� �������� � ������� ���������� �������
    /// (��������, �������� ��� �������������� �������� �������), � ����� ����������� ����
    /// (��������, ������ ��� ������������ ��������) ������ ����������� �� TimeScale.Scale,
    /// ����� ����� ����������� �� ������� �������
    /// </summary>
    public float Scale { get; private set; } = 1f;

    /// <summary>
    /// ����� ��������� ������ ��� ������ �������
    /// </summary>
    public static TimeScale SharedInstance { get; private set; }

    private void Awake()
    {
        SharedInstance = this;

        foreach (var audioSource in GetComponents<AudioSource>())
        {
            var clipName = audioSource.clip.name;
            if (clipName == "Time Slowdown")
            {
                timeSlowdownSound = audioSource;
            }
            else if (clipName == "Time Acceleration")
            {
                timeAccelerationSound = audioSource;
            }
        }
    }

    /// <summary>
    /// ��������� ������� ����������/��������� � ������ ��������
    /// </summary>
    private void ApplyTimeEffectsToObjectPhysics(float newScale)
    {
        foreach (var rigidBody in GameObject.FindObjectsOfType<Rigidbody>(true))
        {
            if (rigidBody.gameObject.name == "Player")
            {
                continue;
            }
            rigidBody.velocity *= newScale / Scale;
            rigidBody.angularVelocity *= newScale / Scale;
        }
        foreach (var gravitationComponent in GameObject.FindObjectsOfType<GravitationController>(true))
        {
            if (gravitationComponent.gameObject.name == "Player")
            {
                continue;
            }
            // ��� ��� ��������� �������� ������� ����������� ������� �� ��������� ��������� ���������� �������
            gravitationComponent.GravityScale = newScale * newScale;
        }
    }

    /// <summary>
    /// ��������/�������� ������ ����� � ������
    /// </summary>
    private void ApplyTimeEffectsToAudioSources(float newScale)
    {
        foreach (var audioSource in GameObject.FindObjectsOfType<AudioSource>(true))
        {
            if (!sourcesWithoutPitchChange.Contains(audioSource.clip.name))
            {
                audioSource.pitch *= newScale / Scale;
            }
        }
    }

    /// <summary>
    /// �������� ������� ������� � ��������� ������� �� ������� ������� � �����
    /// </summary>
    public void SetTimeScale(float newScale)
    {
        if (newScale <= 0)
        {
            return;
        }

        ApplyTimeEffectsToObjectPhysics(newScale);
        ApplyTimeEffectsToAudioSources(newScale);

        if (newScale <= Scale)
        {
            timeSlowdownSound.Play();
        }
        else
        {
            timeAccelerationSound.Play();
        }

        Scale = newScale;
    }

    /// <summary>
    /// ��������� ������������ WaitForSeconds() ��� ���������� ������ ��� ��������� ������� �������
    /// � ������� ������� ������ (��� ��� Time.timeScale �� ����������, ����������� WaitForSeconds()
    /// ������ ����� ����������� ��������� ���������� ������ ��������� �������, ��� �� �������� ��� ���������� ������)
    /// </summary>
    public IEnumerator WaitForSeconds(float seconds)
    {
        float numberOfSecondsElapsed = 0f;

        while (numberOfSecondsElapsed < seconds)
        {
            // ����, ��������, Scale = 0.1f (����� ��� � 10 ��� ���������), �� ��������� ������� ������ ������
            // � 10 ��� ������, ������ ��� numberOfSecondsElapsed ������ >= seconds
            numberOfSecondsElapsed += Time.deltaTime * Scale; // ����� ������������ ������ Time.deltaTime, � �� Time.fixedDeltaTime
            yield return null;
        }
    }
}