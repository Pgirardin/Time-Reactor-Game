using UnityEngine;

/// <summary>
/// ���������� ��������� �������� ������� �� ������������ ������
/// </summary>
public class ShootingOfPlayer : MonoBehaviour
{
    public new Camera camera;
    public GameObject bulletPrefab;
    public Transform gunEnd;

    private float rayDistance = 100f;

    private bool semiAutoShooting = true;
    private bool stopShooting = false;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !stopShooting)
        {
            Shoot();

            if (semiAutoShooting)
            {
                stopShooting = true;
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            stopShooting = false;
        }

        // ��� ������ (���������� �������)
/*        if (Input.GetMouseButton(1))
        {
            Time.timeScale = 0.02f;
            Time.fixedDeltaTime = 0.02f * 0.05f;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }*/
    }

    /// <summary>
    /// ���������� ������� �� ������
    /// </summary>
    private void Shoot()
    {
        Ray rayToScreenCenter = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        int defaultLayerMask = 1;

        // ��� ���������� �������� �� ����������� ���� � ������ ������ (�������);
        // �� ������ ������ ����������� ������������� ���, ���������� �����-�� ������, � ������� ����������� �� ���� ������ �� �����
        // ��������������� ���� � ������������ � ���� ���������� �������
        Vector3 bulletDirection;

        if (Physics.Raycast(rayToScreenCenter, out hit, rayDistance, defaultLayerMask, QueryTriggerInteraction.Ignore))
        {
            bulletDirection = (hit.point - gunEnd.position) / Vector3.Distance(hit.point, gunEnd.position);
        }
        else
        {
            bulletDirection = (rayToScreenCenter.origin + rayToScreenCenter.direction * rayDistance - gunEnd.position) /
                Vector3.Distance(gunEnd.position, rayToScreenCenter.origin + rayToScreenCenter.direction * rayDistance);
        }

        FireABullet(bulletDirection);
    }

    /// <summary>
    /// ������� ���� ��� ��������
    /// </summary>
    private void FireABullet(Vector3 bulletDirection)
    {
        var bulletRotation = Quaternion.FromToRotation(bulletPrefab.transform.forward, bulletDirection);
        var bullet = Instantiate(bulletPrefab, gunEnd.position, bulletRotation);

        var bulletController = bullet.GetComponent<BulletController>();
        bulletController.GiveBulletKineticEnergy(bulletDirection);
    }
}