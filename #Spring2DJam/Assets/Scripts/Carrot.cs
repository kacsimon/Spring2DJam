using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Carrot")]
public class Carrot : ScriptableObject
{
    public TileBase seedlingPrefab;
    public TileBase plantPrefab;
    public TileBase carrotPrefab;
    //public enum State
    //{
    //    Seedling,
    //    Plant,
    //    Carrot
    //}
    //public State state = State.Seedling;
}