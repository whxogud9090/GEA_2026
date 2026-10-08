using UnityEngine;
public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 2, keepDistance = 8, fireRange = 12, fireInterval = 2, bulletSpeed = 10;
    Transform player;
    float lastFireTime;
    void Start() { var p = GameObject.FindWithTag("Player"); if (p != null) player = p.transform; lastFireTime = Time.time; }
    void Update()
    {
        if (player == null || Time.timeScale == 0) return;
        var delta = player.position - transform.position; delta.y = 0;
        float distance = delta.magnitude;
        if (distance > .001f) transform.forward = delta.normalized;
        if (distance > keepDistance) transform.position += delta.normalized * Mathf.Min(moveSpeed * Time.deltaTime, distance - keepDistance);
        if (distance < fireRange && Time.time >= lastFireTime + fireInterval && bulletPrefab != null && firePoint != null)
        {
            lastFireTime = Time.time;
            var dir = (player.position + Vector3.up - firePoint.position).normalized;
            var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
            bullet.GetComponent<Rigidbody>().linearVelocity = dir * bulletSpeed;
        }
    }
}
