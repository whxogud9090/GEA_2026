using UnityEngine;
public class EnemyChaser : MonoBehaviour
{
    public float moveSpeed = 2, detectRange = 15, attackRange = 1.5f, attackCooldown = 1;
    public int damage = 1;
    Transform player;
    Health playerHp;
    float lastAttackTime = -999;
    void Start() { var p = GameObject.FindWithTag("Player"); if (p != null) { player = p.transform; playerHp = p.GetComponent<Health>(); } }
    void Update()
    {
        if (player == null || Time.timeScale == 0) return;
        var dir = player.position - transform.position; dir.y = 0;
        float distance = dir.magnitude;
        if (distance > detectRange) return;
        if (distance > .001f) transform.forward = dir.normalized;
        if (distance > attackRange) transform.position += dir.normalized * Mathf.Min(moveSpeed * Time.deltaTime, distance - attackRange);
        else if (Time.time >= lastAttackTime + attackCooldown && playerHp != null) { lastAttackTime = Time.time; playerHp.TakeDamage(damage); }
    }
}
