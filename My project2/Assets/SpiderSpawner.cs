using System.Collections;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject spiderPrefab;
    public int totalSpidersToSpawn = 10;     // -1 = infinite
    public float startDelay = 1f;
    public float timeBetweenSpawns = 2f;
    public float spawnRadius = 2f;           // horizontal range only

    [Header("Spawn Safety")]
    public float minSpawnDistance = 1.2f;    // must be >= spider collider width
    public LayerMask spiderLayer;

    [Header("References")]
    public Transform playerTarget;

    private int spawnedCount = 0;
    private bool isSpawning = false;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning && spiderPrefab != null)
            StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        yield return new WaitForSeconds(startDelay);

        while (totalSpidersToSpawn < 0 || spawnedCount < totalSpidersToSpawn)
        {
            TrySpawnSpider();
            spawnedCount++;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
    }

    private void TrySpawnSpider()
    {
        // Try multiple positions before giving up
        for (int i = 0; i < 10; i++)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.x += Random.Range(-spawnRadius, spawnRadius);

            if (IsSpawnPositionFree(spawnPos))
            {
                GameObject spider = Instantiate(spiderPrefab, spawnPos, Quaternion.identity);

                SpiderAI ai = spider.GetComponent<SpiderAI>();
                if (ai != null && playerTarget != null)
                    ai.player = playerTarget;

                return; // ✅ spawned successfully
            }
        }

        Debug.LogWarning("SpiderSpawner: No free space to spawn spider.");
    }

    private bool IsSpawnPositionFree(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(
            position,
            minSpawnDistance,
            spiderLayer
        );

        return hit == null;
    }

    private void OnDrawGizmosSelected()
    {
        // Spawn range
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transform.position + Vector3.left * spawnRadius,
            transform.position + Vector3.right * spawnRadius
        );

        // Minimum spacing visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
    }
}
