using UnityEngine;

public class Bunny : MonoBehaviour
{
    float velocity = 3f;
    Vector3Int targetCarrot;
    bool isHungry = true;

    void Start()
    {
        if (GetOGCarrotPosition()) return;
        else if (GetCarrotPosition()) return;
        else Debug.Log("Rabbit is angry!!");
    }
    void FixedUpdate()
    {
        //Move
        Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        //Eat when reach destination
        if (targetPosition == transform.position) Eat();
        //Go back
        if (!isHungry) transform.position = Vector3.MoveTowards(transform.position, new Vector3(13f, 0f, 0f), velocity * Time.deltaTime);

    }
    bool GetCarrotPosition()
    {
        if (GameManager.Instance.carrotPositions.Count == 0) return false;
        int randomIndex = Random.Range(0, GameManager.Instance.carrotPositions.Count);
        targetCarrot = GameManager.Instance.carrotPositions[randomIndex];
        return true;
    }
    bool GetOGCarrotPosition()
    {
        if (GameManager.Instance.ogCarrotPositions.Count == 0) return false;
        int randomIndex = Random.Range(0, GameManager.Instance.ogCarrotPositions.Count);
        targetCarrot = GameManager.Instance.ogCarrotPositions[randomIndex];
        return true;
    }
    void Eat()
    {
        if (!isHungry) return;
        GameManager.Instance.carrotPositions.Remove(targetCarrot);
        isHungry = false;
    }
}