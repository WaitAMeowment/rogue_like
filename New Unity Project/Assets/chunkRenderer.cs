using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ChunkRenderer : MonoBehaviour
{

    class Chunk
    {
        public Tile[] tileArray;
        public Vector3Int[] positionArray;

        public Chunk()
        {
            tileArray = new Tile[0];
            positionArray = new Vector3Int[0];
        }

        public Chunk(Tile[] tiles, Vector3Int[] positions)
        {
            tileArray = tiles;
            positionArray = positions;
        }


    }

    class LevelChunk
    {
        public Vector3Int position;
        public int layout;

        public LevelChunk(int Layout, Vector3Int Position)
        {
            layout = Layout;
            position = Position;
        }
    }

    Chunk[,] GenerateLevel (int chunks)
    {
        //determines layout of level
        //then determines entrance/exit layouts of chunks
        //then assigns chunks in position
        //returns the layout of the level to be used


        //Create 2d map of layout

        //Initialise variables

        //size of level
        int mapSize = chunks / 2 + 4;

        //position of initial chunk
        Vector2Int origin = new Vector2Int(mapSize / 2, mapSize * 3 / 4);

        //options used to hold options for the placement of the next chunk that are currently being considered by the program
        List<Vector2Int> options = new List<Vector2Int>();
        options.Add(origin);

        //reserve used to hold in reserve locations that may be viable
        List< Vector2Int > reserve = new List<Vector2Int>();

        //map displaying whether or not there will be a chunk in each position
        bool[,] level = new bool[mapSize,mapSize];


        //Set edges of levels as occupied so that the level isn't generated off the edge of the map
        for (int x = 0; x < mapSize; x++)
        {
            level[x, 0] = true;
            level[x, mapSize - 1] = true;
        }
        for (int y = 1; y < mapSize-1; y++)
        {
            level[0, y] = true;
            level[mapSize - 1, y] = true;
        }


        //Add each new chunk sequentially for list of suitable options
        //After each chunk is added, certain adjacent tiles are added to list of suitable options
        for (int i = 0; i < chunks; i++)
        {
            //If there are no suitable options for chunk placement in 'options' list, a new option from the 'reserve' list is
            //tested for suitability and then added to 'options' list
            while (options.Count == 0)
            {
                int randomIndex = Random.Range(0, reserve.Count - 1);
                Vector2Int option = reserve[randomIndex];
                reserve.RemoveAt(randomIndex);
                if (!level[option.x, option.y])
                {
                    options.Add(option);
                }
            }

            //Pick random new chunk position to be added
            int newPositionIndex = Random.Range(0, options.Count - 1);
            Vector2Int newPosition = options[newPositionIndex];

            //sets chosen location on level map as full
            level[newPosition.x, newPosition.y] = true;

            //Test to see if adjacent squares are empty, if so, they are added to the options list
            //If the potential options are already surrounded on at least one side by a confirmed chunk, then they are discarded
            //up
            if (!level[newPosition.x, newPosition.y + 1] && !level[newPosition.x + 1, newPosition.y + 1] && !level[newPosition.x - 1, newPosition.y + 1])
            {
                options.Add(new Vector2Int(newPosition.x, newPosition.y + 1));
            }
            //down
            if (!level[newPosition.x, newPosition.y - 1] && !level[newPosition.x + 1, newPosition.y - 1] && !level[newPosition.x - 1, newPosition.y - 1])
            {
                options.Add(new Vector2Int(newPosition.x, newPosition.y - 1));
            }
            //right
            if (!level[newPosition.x + 1, newPosition.y] && !level[newPosition.x + 1, newPosition.y + 1] && !level[newPosition.x + 1, newPosition.y - 1])
            {
                options.Add(new Vector2Int(newPosition.x + 1, newPosition.y));
            }
            //left
            if (!level[newPosition.x - 1, newPosition.y] && !level[newPosition.x - 1, newPosition.y + 1] && !level[newPosition.x - 1, newPosition.y - 1])
            {
                options.Add(new Vector2Int(newPosition.x - 1, newPosition.y));
            }


            //remove currently considered option from list
            options.RemoveAt(newPositionIndex);

            //remove random options in order to encourage long paths but keep some branching
            //comparative indicies are picked to produce nice looking options but can be changed
            if (options.Count > 1)
            {
                //remove random option, but place it into reserve in case options run out
                int newReserveIndex = Random.Range(0, options.Count - 1);
                reserve.Add(options[newReserveIndex]);
                options.RemoveAt(newReserveIndex);
            }
            if (options.Count > 3)
            {
                //remove random option, but place it into reserve in case options run out
                int newReserveIndex = Random.Range(0, options.Count - 1);
                reserve.Add(options[newReserveIndex]);
                options.RemoveAt(newReserveIndex);
            }
            if (options.Count > 5)
            {
                //remove random option, but place it into reserve in case options run out
                int newReserveIndex = Random.Range(0, options.Count - 1);
                reserve.Add(options[newReserveIndex]);
                options.RemoveAt(newReserveIndex);
            }

        }


        //Reset edges of levels as unoccupied 
        for (int x = 0; x < mapSize; x++)
        {
            level[x, 0] = false;
            level[x, mapSize - 1] = false;
        }
        for (int y = 1; y < mapSize - 1; y++)
        {
            level[0, y] = false;
            level[mapSize - 1, y] = false;
        }

        //2d Boolean array level now shows a map of where chunks of the level are to be placed



        //Assign chunks of appropriate shapes into appropriate positions

        //Initialise variables
        Chunk[,] chunkLayout = new Chunk[mapSize, mapSize];

        //Iterate through each position in level and add chunks of appropriate shapes
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                //if there is no chunk to be placed at a position, add an empty chunk
                if (!level[x, y])
                {
                    chunkLayout[x, y] = new Chunk();
                }
                else
                {
                    //add a random chunk
                    //TODO make actual meaningful chunks
                    Chunk tempChunk = RandomChunk(50);
                    for (int i = 0; i < tempChunk.positionArray.Length; i++)
                    {
                        tempChunk.positionArray[i] = new Vector3Int(tempChunk.positionArray[i].x + 16 * x, tempChunk.positionArray[i].y + 16 * y, tempChunk.positionArray[i].z);
                    }

                    chunkLayout[x, y] = tempChunk;
                }
            }
        }

        return chunkLayout;

    }

    Chunk RandomChunk(int num)
    {
        Tile[] tileArray = new Tile[num];
        Vector3Int[] positionArray = new Vector3Int[num];
        for (int i = 0; i < num; i++)
        {
            tileArray[i] = wall;
            positionArray[i] = new Vector3Int(Random.Range(0, 16), Random.Range(0, 16), 0);
        }
        return new Chunk(tileArray, positionArray);
    }

    public Tilemap level;
    public Vector3Int levelLayout;
    public Tile wall;
    Tile[] tileArray;
    Vector3Int[] positionArray;

    private Chunk[] chunks;
    // Start is called before the first frame update
    void Start()
    {
        level.ClearAllTiles();

        Chunk[] chunks = new Chunk[6];
        for (int i = 0; i < 6; i++)
        {
            chunks[i] = RandomChunk(15);
        }

        Chunk[,] chunkLayout = GenerateLevel(20);

        foreach (Chunk chunk in chunkLayout)
        {
            level.SetTiles(chunk.positionArray, chunk.tileArray);
        }

        //tileArray = chunks[0].tileArray;
        //positionArray = chunks[0].positionArray;
    }

    // Update is called once per frame
    void Update()
    {
        //frame += 1;
        
        //level.SetTile(new Vector3Int((3 * frame)%35 - (2*frame)%44, (5 * frame)%37 - (2*frame)%44, frame % 30 - 15), wall);

    }
}
