using UnityEngine;

public class Withering : MonoBehaviour
{
    Vector3Int position;
    TileData data;
    WitheringManager witheringManager;
    float witheringTimeCounter, spreadIntervalCounter/*, spreadDelay*/;

    public void StartWithering(Vector3Int position, TileData data, WitheringManager witheringManager)
    {
        this.position = position;
        this.data = data;
        this.witheringManager = witheringManager;

        witheringTimeCounter = data.witheringTime;
        spreadIntervalCounter = data.spreadInterval;
        //spreadDelay = data.spreadDelay;
    }
    void Update()
    {
        witheringTimeCounter -= Time.deltaTime;
        if (witheringTimeCounter <= 0)
        {
            witheringManager.FinishedWithering(position);
            Destroy(gameObject);
        }
        
        spreadIntervalCounter -= Time.deltaTime;
        if(spreadIntervalCounter <= 0)
        {
            spreadIntervalCounter = data.spreadInterval;
            //spreadDelay -= Time.deltaTime;
            //if (spreadDelay <= 0)
            //{
            //    spreadDelay = data.spreadDelay;
            //    //witheringManager.TryToSpread(position);
            //}
        }
    }
}