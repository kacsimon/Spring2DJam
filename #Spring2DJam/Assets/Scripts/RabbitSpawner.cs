using UnityEngine;

public class RabbitSpawner : MonoBehaviour
{
    [SerializeField] Transform bunnyPrefab;
    [SerializeField] float minSpawnTime, maxSpawnTime;
    float spawnTime;

    void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            Instantiate(bunnyPrefab, transform.position, transform.rotation);
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}
