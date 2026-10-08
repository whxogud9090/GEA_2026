using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3;
    public int maxAlive = 10;
    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || spawnPoints == null || spawnPoints.Length == 0) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxAlive) return;
        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (prefab != null && point != null) Instantiate(prefab, point.position, Quaternion.identity);
    }
}
