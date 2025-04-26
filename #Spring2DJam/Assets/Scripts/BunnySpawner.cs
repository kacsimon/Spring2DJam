using UnityEngine;

public class BunnySpawner : MonoBehaviour
{
    [SerializeField] Transform bunnyPrefab;
    [SerializeField] float spawnTimer = 5f;

    void Update()
    {

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            Instantiate(bunnyPrefab, transform.position, transform.rotation);
            spawnTimer = 5f;
        }
    }
}
