using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int initialPoolSize = 10;
    public float spawnRate = 1f;
    public float spawnDistance = 5f;
    public float minXSpawn = -2.82f;
    public float maxXSpawn = 2.82f;

    private float lastSpawnTime = 0f;
    private float nextSpawnHeight = 0f;
    private float highestPlatformHeight = 0f;

    private void Start()
    {
        // Spawn initial platforms
        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform(i * spawnDistance);
        }
        highestPlatformHeight = 4 * spawnDistance; // Inicializa con la altura de la ?ltima plataforma inicial
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > spawnRate)
        {
            // Solo spawnea si el jugador ha subido lo suficiente
            if (Camera.main.transform.position.y + 10f > highestPlatformHeight)
            {
                highestPlatformHeight += spawnDistance;
                SpawnPlatform(highestPlatformHeight);
                lastSpawnTime = Time.time;
            }
        }
    }

    private void SpawnPlatform(float height)
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(minXSpawn, maxXSpawn),
            height,
            0f
        );

        Quaternion spawnRot = Quaternion.identity;

        PoolObject platform = StaticObjectPooling.Instance.SpawnFromPool("Platform", spawnPos, spawnRot);

        // Return platform to pool if it goes below view
        StartCoroutine(ReturnPlatformAfterDelay(platform, 10f));
    }

    private IEnumerator ReturnPlatformAfterDelay(PoolObject platform, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (platform.gameObject.activeInHierarchy)
        {
            StaticObjectPooling.Instance.ReturnToPool(platform);
        }
    }
}