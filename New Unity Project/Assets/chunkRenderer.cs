using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class chunkRenderer : MonoBehaviour
{
    public Tilemap level;
    public Tile wall;
    private int frame = 0;
    private Tile[] tileArray;
    // Start is called before the first frame update
    void Start()
    {
        level.ClearAllTiles();
        tileArray = new Tile[2];
        tileArray[0] = wall;
        tileArray[1] = wall;
        Vector3Int[] positionArray = {Vector3Int.up, Vector3Int.left};
        level.SetTiles(positionArray, tileArray);
    }

    // Update is called once per frame
    void Update()
    {
        frame += 1;
        
        //level.SetTile(new Vector3Int((3 * frame)%35 - (2*frame)%44, (5 * frame)%37 - (2*frame)%44, frame % 30 - 15), wall);

    }
}
