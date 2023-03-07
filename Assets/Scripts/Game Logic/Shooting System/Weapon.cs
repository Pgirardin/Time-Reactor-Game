using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// �����, ����������� ������ ������ � ����
/// </summary>
public class Weapon : ObjectWithInformation, ISerializationCallbackReceiver
{
    [SerializeField] private Transform positionInPlayerHand;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Pool pool;
    [SerializeField] private TextMeshProUGUI ammoScreen;
    [SerializeField] private Transform weaponStart;
    [SerializeField] private Transform weaponEnd;

    private AudioSource shotSound;
    private AudioSource reloadingSound;
    private List<AudioSource> weaponHitingOnSurfaceSounds = new List<AudioSource>();

    private System.Random random = new System.Random();

    [SerializeField] new private string name;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float intervalBetweenShoots;
    [SerializeField] private bool semiAutoShooting;
    [SerializeField] private float reloadingDuration;
    [SerializeField] private int magazinCapacity;
    [SerializeField] private int bulletsCountInMagazine;
    [SerializeField] private int bulletsCountInReserve;
    [SerializeField] private float rayDistance;

    /// <summary>
    /// ��������� ������ � ���� ������
    /// </summary>
    public Transform PositionInPlayerHand { get; private set; }

    /// <summary>
    /// �������� ������
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// ����������� ������ � �������� ������
    /// </summary>
    public Sprite Sprite { get; private set; }

    /// <summary>
    /// ����������� �������� ����� ����������
    /// </summary>
    public float IntervalBetweenShoots { get; private set; }

    /// <summary>
    /// ������������ ����������� ������
    /// </summary>
    public float ReloadingDuration { get; private set; }

    /// <summary>
    /// ���� ������� true, �� ������ ����� ����� ������������������ �������� (��������), ����� �������������� (��������)
    /// </summary>
    public bool SemiAutoShooting { get; private set; } = true;

    /// <summary>
    /// ���� ���������� ������
    /// </summary>
    public AudioSource PickUpSound { get; private set; }

    /// <summary>
    /// ������� ���������� �������� � ������
    /// </summary>
    public int BulletsCountInMagazine
    {
        get { return bulletsCountInMagazine; }
        set
        {
            if (value > magazinCapacity)
            {
                return;
            }

            var oldBulletsCountInMagazine = bulletsCountInMagazine;
            bulletsCountInMagazine = value;
            if (oldBulletsCountInMagazine != bulletsCountInMagazine)
            {
                RedrawAmmoScreen();
            }
        }
    }

    /// <summary>
    /// ���������� �������� � ������
    /// </summary>
    public int BulletsCountInReserve
    {
        get { return bulletsCountInReserve; }
        set
        {
            var oldBulletsCountInReserve = bulletsCountInReserve;
            bulletsCountInReserve = value;
            if (oldBulletsCountInReserve != bulletsCountInReserve)
            {
                RedrawAmmoScreen();
            }
        }
    }

    public override string[,] ObjectInfoParameters { get; set; }

    public override string ObjectInfoHeader { get; set; } = "Weapon";

    public override Color ObjectInfoHeaderColor { get; set; } = Color.yellow;

    public void OnBeforeSerialize()
    {
        positionInPlayerHand = PositionInPlayerHand;
        name = Name;
        sprite = Sprite;
        intervalBetweenShoots = IntervalBetweenShoots;
        reloadingDuration = ReloadingDuration;
        semiAutoShooting = SemiAutoShooting;
        bulletsCountInMagazine = BulletsCountInMagazine;
        bulletsCountInReserve = BulletsCountInReserve;
    }

    public void OnAfterDeserialize()
    {
        PositionInPlayerHand = positionInPlayerHand;
        Name = name;
        Sprite = sprite;
        IntervalBetweenShoots = intervalBetweenShoots;
        ReloadingDuration = reloadingDuration;
        SemiAutoShooting = semiAutoShooting;
        BulletsCountInMagazine = bulletsCountInMagazine;
        BulletsCountInReserve = bulletsCountInReserve;
    }

    private void Awake()
    {
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            var clipName = audioSource.clip.name;
            if (clipName.EndsWith("Shot"))
            {
                shotSound = audioSource;
            }
            else if (clipName.EndsWith("Reloading"))
            {
                reloadingSound = audioSource;
            }
            else if (clipName.StartsWith("Weapon Hitting On Surface"))
            {
                weaponHitingOnSurfaceSounds.Add(audioSource);
            }
            else if (clipName == "Weapon Picking Up")
            {
                PickUpSound = audioSource;
            }
        }

        InitializeInfoPanelPrefab();
        ObjectInfoParameters = new string[5, 2] { { "Name:", Name },
                                                  { "Shooting type:", SemiAutoShooting ? "Semi-Automatic" : "Automatic" },
                                                  { "Firing Frequency:", Math.Round(1 / IntervalBetweenShoots).ToString() + " per sec." },
                                                  { "Bullet velocity:", bulletPrefab.GetComponent<Bullet>().Velocity.ToString() + " m/s" },
                                                  { "Damage:", bulletPrefab.GetComponent<Bullet>().Damage.ToString() + " HP" } };

        if (BulletsCountInMagazine > magazinCapacity)
        {
            BulletsCountInMagazine = magazinCapacity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����� ��������� ���� ����� � ������������, ������ ����� �������� ���� Default
        if (collision.gameObject.layer == 0)
        {
            // ���� �����-�� ���� ��� �������������, �� �� ������ ���������� �� �����
            foreach (var sound in weaponHitingOnSurfaceSounds)
            {
                if (sound.isPlaying)
                {
                    return;
                }
            }

            var randomIndex = random.Next(weaponHitingOnSurfaceSounds.Count);
            weaponHitingOnSurfaceSounds[randomIndex].Play();
        }
    }

    /// <summary>
    /// ������������ ����� � ����������� � ���������� ��������
    /// </summary>
    private void RedrawAmmoScreen()
    {
        ammoScreen.text = BulletsCountInMagazine.ToString() + " / " + BulletsCountInReserve.ToString();
    }

    /// <summary>
    /// ���������� ������� �� ������
    /// </summary>
    public void Shoot()
    {
        if (BulletsCountInMagazine == 0)
        {
            return;
        }
        BulletsCountInMagazine--;

        shotSound.Play();

        Ray rayToScreenCenter = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        int defaultLayerMask = 1;

        // ��� ���������� �������� �� ����������� ���� � ������ ������ (�������);
        // �� ������ ������ ����������� ������������� ���, ���������� �����-�� ������, � ������� ����������� �� ���� ������ �� �����
        // ��������������� ���� � ������������ � ���� ���������� �������
        Vector3 bulletDirection;

        if (Physics.Raycast(rayToScreenCenter, out hit, rayDistance, defaultLayerMask, QueryTriggerInteraction.Ignore))
        {
            bulletDirection = (hit.point - weaponEnd.position) / Vector3.Distance(hit.point, weaponEnd.position);
        }
        else
        {
            bulletDirection = (rayToScreenCenter.origin + rayToScreenCenter.direction * rayDistance - weaponEnd.position) /
                Vector3.Distance(weaponEnd.position, rayToScreenCenter.origin + rayToScreenCenter.direction * rayDistance);
        }

        FireABullet(bulletDirection);
    }

    /// <summary>
    /// ������� ���� ��� ��������
    /// </summary>
    private void FireABullet(Vector3 bulletDirection)
    {
        var bullet = pool.GetObject();
        if (bullet != null)
        {
            var bulletRotation = Quaternion.FromToRotation(bulletPrefab.transform.forward, bulletDirection);
            bullet.transform.position = weaponEnd.position;
            bullet.transform.rotation = bulletRotation;
        }

        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.GiveBulletKineticEnergy(bulletDirection);
    }

    /// <summary>
    /// �������� ������ �����, ���� ��� �������� � �����
    /// </summary>
    public void PushOutWeaponFromWall(float distanceFromWhichToPushWeapon)
    {
        var layerMask = 1;
        var weaponDisplacementDistance = UsefulFeatures.CalculateDepthOfObjectEntryIntoNearestSurface(weaponStart.position, weaponEnd.position, layerMask);
        if (weaponDisplacementDistance > distanceFromWhichToPushWeapon)
        {
            transform.position += -transform.forward * weaponDisplacementDistance;
        }
    }

    /// <summary>
    /// ���������� ���� ������ ������ ����������� ���� � ��������/��������� ���������� (������������ ��� ������������/���������� ������)
    /// </summary>
    public void SetUpWeaponPartsLayersAndColliders(string layerName, bool collidersEnabled)
    {
        Transform[] weaponParts = GetComponentsInChildren<Transform>();
        foreach (Transform part in weaponParts)
        {
            var partCollider = part.GetComponent<Collider>();
            if (partCollider != null)
            {
                partCollider.enabled = collidersEnabled;
            }
            part.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    /// <summary>
    /// ��������� �������� � ����������� ������� ����� ����, ��� ��� ����������� ����� �������
    /// </summary>
    public IEnumerator PerformActionsAfterFallOfEjectedWeapon()
    {
        yield return new WaitUntil(() => GetComponent<Rigidbody>().velocity.magnitude <= 0.001f);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// ����� �� ���� ��������� ����������� ������
    /// </summary>
    public bool ReloadingCanBePerformed()
    {
        return BulletsCountInMagazine != magazinCapacity && BulletsCountInReserve != 0;
    }

    /// <summary>
    /// ������������ ������
    /// </summary>
    public void ReloadWeapon()
    {
        if (!ReloadingCanBePerformed())
        {
            return;
        }

        var bulletsCountToFillMagazine = magazinCapacity - BulletsCountInMagazine;
        if (BulletsCountInReserve < bulletsCountToFillMagazine)
        {
            BulletsCountInMagazine += BulletsCountInReserve;
            BulletsCountInReserve = 0;
        }
        else
        {
            BulletsCountInReserve -= bulletsCountToFillMagazine;
            BulletsCountInMagazine = magazinCapacity;
        }
    }

    /// <summary>
    /// ��������� ���� ����������� ������
    /// </summary>
    public void PlayReloadingSound()
    {
        if (reloadingSound != null)
        {
            reloadingSound.Play();
        }
    }

    /// <summary>
    /// �������� ���� ����������� ������
    /// </summary>
    public void StopReloadingSound()
    {
        if (reloadingSound != null)
        {
            reloadingSound.Stop();
        }
    }
}