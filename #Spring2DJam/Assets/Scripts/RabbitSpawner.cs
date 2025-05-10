using UnityEngine;

public class RabbitSpawner : MonoBehaviour
{
    [SerializeField] Transform bunnyPrefab;
    [SerializeField] float minSpawnTime, maxSpawnTime;
    float spawnTime, spawnMultiplier = 1f, minSpawnMultiplier = .4f;

    void Start()
    {
        spawnTime = 13f * spawnMultiplier;
    }
    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            Instantiate(bunnyPrefab, transform.position, transform.rotation);
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime) * spawnMultiplier;
            if (spawnMultiplier > minSpawnMultiplier) spawnMultiplier -= .05f;
        }
    }
}
