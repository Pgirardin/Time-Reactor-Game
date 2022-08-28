using UnityEngine;
using System.Collections;

/// <summary>
/// �����, ����������� ������ ������ � ����
/// </summary>
public class Weapon : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private Transform positionInPlayerHand;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform weaponStart;
    [SerializeField]
    private Transform weaponEnd;

    [SerializeField]
    private string weaponName;
    [SerializeField]
    private float intervalBetweenShoots = 0.1f;
    [SerializeField]
    private bool semiAutoShooting = true;
    [SerializeField]
    private float rayDistance = 100f;

    /// <summary>
    /// ��������� ������ � ���� ������
    /// </summary>
    public Transform PositionInPlayerHand { get; private set; }

    /// <summary>
    /// �������� ������
    /// </summary>
    public string WeaponName { get; private set; }
    
    /// <summary>
    /// ����������� �������� ����� ����������
    /// </summary>
    public float IntervalBetweenShoots { get; private set; } = 0.1f;

    /// <summary>
    /// ���� ������� true, �� ������ ����� ����� ������������������ �������� (��������), ����� �������������� (��������)
    /// </summary>
    public bool SemiAutoShooting { get; private set; } = true;

    public void OnBeforeSerialize()
    {
        positionInPlayerHand = PositionInPlayerHand;
        weaponName = WeaponName;
        intervalBetweenShoots = IntervalBetweenShoots;
        semiAutoShooting = SemiAutoShooting;
    }

    public void OnAfterDeserialize()
    {
        PositionInPlayerHand = positionInPlayerHand;
        WeaponName = weaponName;
        IntervalBetweenShoots = intervalBetweenShoots;
        SemiAutoShooting = semiAutoShooting;
    }

    /// <summary>
    /// ���������� ������� �� ������
    /// </summary>
    public void Shoot()
    {
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
        var bulletRotation = Quaternion.FromToRotation(bulletPrefab.transform.forward, bulletDirection);
        var bullet = Instantiate(bulletPrefab, weaponEnd.position, bulletRotation);

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
        yield return new WaitUntil(() => GetComponent<Rigidbody>().velocity == Vector3.zero);
        GetComponent<Rigidbody>().isKinematic = true;
    }
}