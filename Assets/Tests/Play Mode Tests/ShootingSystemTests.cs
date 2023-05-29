using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// ����� ������, ����������� ������������ ������ ������� �������� ������
/// </summary>
public class ShootingSystemTests
{
    /// <summary>
    /// ����������� �� ����������� ��� ������ �� �������� �� ������
    /// </summary>
    /// <returns>player - ������ ������, laserPistol - ��������� "Weapon" � ��������� ���������, 
    /// poolObjects - ������ �������� ����</returns>
    private (GameObject player, Weapon laserPistol, GameObject pool, List<GameObject> poolObjects) PrepareSystemForFiring()
    {
        // �������� ������ � ��������� ��������� � ����
        var player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Assets For Tests/Player"),
            new Vector3(0, 0, 0), Quaternion.identity);
        // �������� ���������� "Weapon" � ��������� ��������� ������ (��� ��� ����� �������� GameObject'��
        // � ����������� "Weapon" ������ �� �������)
        var laserPistol = player.GetComponentsInChildren<Weapon>(false)[0];

        // �������� ���� ��� �������� ���������
        var pool = new GameObject();
        pool.transform.name = "Pool";
        var poolComponent = pool.AddComponent<Pool>();

        // ��������� ���� (� �������������� ���������)
        var poolClassType = typeof(Pool);
        FieldInfo bulletPrefabField = poolClassType.GetField("prefab", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo bulletAmountfField = poolClassType.GetField("amount", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo poolObjectsField = poolClassType.GetField("objects", BindingFlags.NonPublic | BindingFlags.Instance);
        var bulletPrefab = Resources.Load<GameObject>("Assets For Tests/Laser Pistol Bullet");
        bulletPrefabField.SetValue(poolComponent, bulletPrefab);
        bulletAmountfField.SetValue(poolComponent, 1);
        var poolObjects = (List<GameObject>)poolObjectsField.GetValue(poolComponent);

        // ��������� ���������� "Pool" ��� ���������
        var weaponClassType = typeof(Weapon);
        FieldInfo pistolPoolField = weaponClassType.GetField("pool", BindingFlags.NonPublic | BindingFlags.Instance);
        pistolPoolField.SetValue(laserPistol, poolComponent);

        return (player, laserPistol, pool, poolObjects);
    }

    [UnityTest]
    public IEnumerator AfterShotBulletIsCreatedAndFliesWithCorrectSpeed()
    {
        (var player, var laserPistol, var pool, var poolObjects) = PrepareSystemForFiring();

        // ��������, ����� ����� ��������� ������� ������ ��������� � ����
        yield return new WaitForSeconds(0.1f);
        laserPistol.Shoot();

        yield return null;

        var firedBullet = poolObjects[0];
        // ��������, ��� ���� ������������� ���� �������� (� ���� ����� ��������)
        Assert.True(firedBullet.activeSelf);
        // ��������, ��� �������� ���������� ���� �����, ����� ������ ���� � � ���������������
        Assert.That((int)firedBullet.GetComponent<Rigidbody>().velocity.magnitude == firedBullet.GetComponent<Bullet>().Velocity);

        GameProperties.GeneralPool.Clear();
        MonoBehaviour.Destroy(player);
        MonoBehaviour.Destroy(pool);
    }

    [UnityTest]
    public IEnumerator ShotMustNotBePerformedIfNoBulletsInMagazine()
    {
        (var player, var laserPistol, var pool, var poolObjects) = PrepareSystemForFiring();

        var weaponClassType = typeof(Weapon);
        FieldInfo bulletsCountInMagazineField = weaponClassType.GetField("bulletsCountInMagazine",
            BindingFlags.NonPublic | BindingFlags.Instance);
        bulletsCountInMagazineField.SetValue(laserPistol, 0);

        // ��������, ����� ����� ��������� ������� ������ ��������� � ����
        yield return new WaitForSeconds(0.1f);
        laserPistol.Shoot();

        yield return null;

        var bulletToFire = poolObjects[0];
        // ��������, ��� ���� �� ���� ��������
        Assert.False(bulletToFire.activeSelf);
        // ��������, ��� ���-�� �������� � �������� �� ����� ������ ����
        Assert.AreEqual(0, bulletsCountInMagazineField.GetValue(laserPistol));

        GameProperties.GeneralPool.Clear();
        MonoBehaviour.Destroy(player);
        MonoBehaviour.Destroy(pool);
    }

    [UnityTest]
    public IEnumerator WeaponMustNotBeReloadedIfMagazinIsFull()
    {
        // �������� ������ � ��������� ��������� � ����
        var player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Assets For Tests/Player"),
            new Vector3(0, 0, 0), Quaternion.identity);
        // �������� ���������� "Weapon" � ��������� ��������� ������ (��� ��� ����� �������� GameObject'��
        // � ����������� "Weapon" ������ �� �������)
        var laserPistol = player.GetComponentsInChildren<Weapon>(false)[0];

        // ��������� � ������� ��������� ���������� �������� � �������� � � ������
        var weaponClassType = typeof(Weapon);
        FieldInfo bulletsCountInMagazineField = weaponClassType.GetField("bulletsCountInMagazine", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo bulletsCountInReserveField = weaponClassType.GetField("bulletsCountInReserve", BindingFlags.NonPublic | BindingFlags.Instance);
        var bulletsCountInMagazin = bulletsCountInMagazineField.GetValue(laserPistol);
        var bulletsCountInReserve = bulletsCountInReserveField.GetValue(laserPistol);

        laserPistol.ReloadWeapon();
        yield return null;

        // �������� �� ��, ��� ���������� �������� �� ����������
        Assert.AreEqual(bulletsCountInMagazin, bulletsCountInMagazineField.GetValue(laserPistol));
        Assert.AreEqual(bulletsCountInReserve, bulletsCountInReserveField.GetValue(laserPistol));

        MonoBehaviour.Destroy(player);
    }

    [UnityTest]
    public IEnumerator ReloadingMustBePerformedIfMagazinIsNotFullAndThereAreBulletsInReserve()
    {
        // �������� ������ � ��������� ��������� � ����
        var player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Assets For Tests/Player"),
            new Vector3(0, 0, 0), Quaternion.identity);
        // �������� ���������� "Weapon" � ��������� ��������� ������ (��� ��� ����� �������� GameObject'��
        // � ����������� "Weapon" ������ �� �������)
        var laserPistol = player.GetComponentsInChildren<Weapon>(false)[0];

        // ��������� � ������� ��������� ���������� �������� � �������� � � ������
        var weaponClassType = typeof(Weapon);
        FieldInfo bulletsCountInMagazineField = weaponClassType.GetField("bulletsCountInMagazine", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo bulletsCountInReserveField = weaponClassType.GetField("bulletsCountInReserve", BindingFlags.NonPublic | BindingFlags.Instance);

        bulletsCountInMagazineField.SetValue(laserPistol, 3);
        bulletsCountInReserveField.SetValue(laserPistol, 30);

        laserPistol.ReloadWeapon();
        yield return null;

        FieldInfo magazinCapacityField = weaponClassType.GetField("magazinCapacity", BindingFlags.NonPublic | BindingFlags.Instance);
        var magazinCapacity = magazinCapacityField.GetValue(laserPistol);
        Assert.AreEqual(magazinCapacity, bulletsCountInMagazineField.GetValue(laserPistol));
        Assert.AreEqual(23, bulletsCountInReserveField.GetValue(laserPistol));

        MonoBehaviour.Destroy(player);
    }

    [UnityTest]
    public IEnumerator ReloadingMustNotBePerformedIfBulletsCountInReserveIsZero()
    {
        // �������� ������ � ��������� ��������� � ����
        var player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Assets For Tests/Player"),
            new Vector3(0, 0, 0), Quaternion.identity);
        // �������� ���������� "Weapon" � ��������� ��������� ������ (��� ��� ����� �������� GameObject'��
        // � ����������� "Weapon" ������ �� �������)
        var laserPistol = player.GetComponentsInChildren<Weapon>(false)[0];

        // ��������� � ������� ��������� ���������� �������� � �������� � � ������
        var weaponClassType = typeof(Weapon);
        FieldInfo bulletsCountInMagazineField = weaponClassType.GetField("bulletsCountInMagazine", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo bulletsCountInReserveField = weaponClassType.GetField("bulletsCountInReserve", BindingFlags.NonPublic | BindingFlags.Instance);

        bulletsCountInMagazineField.SetValue(laserPistol, 5);
        bulletsCountInReserveField.SetValue(laserPistol, 0);

        laserPistol.ReloadWeapon();
        yield return null;

        Assert.AreEqual(5, bulletsCountInMagazineField.GetValue(laserPistol));
        Assert.AreEqual(0, bulletsCountInReserveField.GetValue(laserPistol));

        MonoBehaviour.Destroy(player);
    }
}