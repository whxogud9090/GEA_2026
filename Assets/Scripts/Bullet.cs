using UnityEngine;
public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 2;
    public string targetTag = "Enemy", ownerTag = "Player";
    bool hit;
    void Start() { Destroy(gameObject, lifeTime); }
    void OnTriggerEnter(Collider other)
    {
        if (hit || other.isTrigger || other.CompareTag(ownerTag)) return;
        if (other.CompareTag(targetTag)) { var hp = other.GetComponent<Health>(); if (hp != null) hp.TakeDamage(damage); }
        hit = true;
        Destroy(gameObject);
    }
}
