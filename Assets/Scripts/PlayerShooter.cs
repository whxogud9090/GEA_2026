using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20, fireCooldown = .2f;
    float lastFireTime = -999;
    public void OnAttack(InputValue value)
    {
        if (!value.isPressed || Time.timeScale == 0 || Time.time < lastFireTime + fireCooldown) return;
        if (bulletPrefab == null || firePoint == null) return;
        lastFireTime = Time.time;
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = firePoint.forward * bulletSpeed;
    }
}
