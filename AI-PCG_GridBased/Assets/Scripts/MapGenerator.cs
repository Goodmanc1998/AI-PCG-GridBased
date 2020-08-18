using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [Header("Map Settings")]

    //MapSize
    public Vector2 mapSize;

    //Restricting the int for random spawn chance
    [Range(0,100)]
    public int fillPercent;

    public bool createOutsideWalls;

    public int smoothItterations;

    public bool autoGenerate;

    [Header("Smoothing Settings")]
    //How many walls neighbours are needed for this to become a wall
    public int wallsNeeded;

    public bool useNewMap;

    [Header("Gold Settings")]

    public int wallsNeededG;

    [Range(0, 100)]
    public int goldChance;

    [Header("Spawn Settings")]

    public int spwanWallsNeeded;

    [Range(0, 100)]
    public int spawnRoomChance;

    public int distanceBetweenSpawn;

    Vector2 startPosition;

    //Storing the map positions and type(Wall, Air)
    int[,] map;

    int[,] newMap;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Generate New Level"))
        {
            GenerateMap();
        }

        if(GUILayout.Button("Smoothing Map"))
        {
            if (useNewMap)
            {
                map = SmoothMap(map);
            }
            else
                SmoothMap();
            
        }

        if(GUILayout.Button("Generate Gold"))
        {
            GenerateGold();
        }

        if (GUILayout.Button("Generate Start"))
        {
            GenerateStart();
        }

        if (GUILayout.Button("Generate Finish"))
        {
            GenerateFinish();
        }


    }

    void GenerateMap()
    {
        //Creating the new Map size
        map = new int[(int)mapSize.x, (int)mapSize.y];

        RandomFillMap();

        if(autoGenerate)
        {
            for (int i = 0; i < smoothItterations; i++)
            {
                if (useNewMap)
                {
                    map = SmoothMap(map);
                }
                else
                    SmoothMap();
            }

            GenerateGold();

            GenerateStart();

            GenerateFinish();
        }
    }

   

    void RandomFillMap()
    {
        //Running through the length and height of the mapSize
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                //Generating a random number
                int rdmRange = Random.Range(0, 100);

                if (createOutsideWalls)
                {
                    if (x == 0 || x == mapSize.x - 1 || y == 0 || y == mapSize.y - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        //Checking if the random number is less than than the 
                        if (rdmRange < fillPercent)
                        {
                            map[x, y] = 1;
                        }
                        else
                        {
                            map[x, y] = 0;
                        }
                    }
                }
                else
                {
                    //Checking if the random number is less than than the 
                    if (rdmRange < fillPercent)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
    }

    int[,] SmoothMap(int[,] map)
    {
        newMap = new int[(int)mapSize.x, (int)mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                int neighbourWallTiles = getSurroundingNeigbours(x, y);

                //Debug.Log("X : " + x + " Y : " + y + " Count : " + neighbourWallTiles + " No : " + map[x,y] );
                /*
                if (neighbourWallTiles >= wallsNeeded)
                {
                    newMap[x, y] = 1;
                }
                else //if (neighbourWallTiles < wallsNeeded)
                {
                    newMap[x, y] = 0;
                }*/

                if (neighbourWallTiles >= wallsNeeded)
                {
                    newMap[x, y] = 1;
                }
                else if (neighbourWallTiles < wallsNeeded)
                {
                    newMap[x, y] = 0;
                }
            }
        }

        return newMap;
    }

    void SmoothMap()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                int neighbourWallTiles = getSurroundingNeigbours(x, y);

                //Debug.Log("X : " + x + " Y : " + y + " Count : " + neighbourWallTiles + " No : " + map[x,y] );
                

                if(neighbourWallTiles >= wallsNeeded)
                {
                    map[x, y] = 1;
                }
                else //if(neighbourWallTiles < wallsNeeded)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    

    void GenerateGold()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if(x != 0 && x != mapSize.x && y != 0 && y != mapSize.y)
                {
                    int neighbourWallTiles = getSurroundingNeigbours(x, y);

                    int rdmRange = Random.Range(0, 100);

                    if (neighbourWallTiles >= wallsNeededG)
                    {
                        if (rdmRange < goldChance)
                        {
                            map[x, y] = 3;

                        }


                    }
                }
            }
        }
    }

    

    void GenerateStart()
    {
        int spawnCount = 0;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (x != 0 || x != mapSize.x || y != 0 || y != mapSize.y)
                {

                    int rdmRange = Random.Range(0, 100);

                    if (map[x, y] == 0 && spawnCount < 1 && rdmRange < spawnRoomChance) 
                    {

                        int neighbourWallTiles = getBottomNeighbours(x, y);

                        if (neighbourWallTiles >= spwanWallsNeeded)
                        {
                            map[x, y] = 4;
                            spawnCount++;

                            startPosition = new Vector2(x, y);
                        }
                    }
                }
            }
        }

        if (spawnCount == 0)
        {
            GenerateStart();
        }

    }

    void GenerateFinish()
    {
        int spawnCount = 0;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (x != 0 || x != mapSize.x || y != 0 || y != mapSize.y)
                {
                    int rdmRange = Random.Range(0, 100);

                    Vector2 finishPosition = new Vector2(x, y);

                    if (map[x, y] == 0 && spawnCount < 1 && rdmRange < spawnRoomChance && Vector2.Distance(startPosition, finishPosition) > distanceBetweenSpawn)
                    {
                        int neighbourWallTiles = getBottomNeighbours(x, y);

                        if (neighbourWallTiles >= spwanWallsNeeded)
                        {
                            map[x, y] = 5;
                            spawnCount++;
                        }

                    }
                }
            }
        }

        if(spawnCount == 0)
        {
            GenerateFinish();
        }

    }


    int getBottomNeighbours(int positionX, int positionY)
    {
        int wallCount = 0;

        //Looping through a 3 by 3 grid to get neighbours
        for (int neighbourX = positionX - 1; neighbourX <= positionX + 1; neighbourX++)
        {
            int neighbourY = positionY - 1;

            //Making sure the neighboyrs are within the map 
            if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
            {
                //Making sure the center position isnt counted
                if (neighbourX != positionX || neighbourY != positionY)
                {
                    //wallCount += oldMap[positionX, positionY];

                    if (map[neighbourX, neighbourY] == 1)
                        wallCount++;
                }
            }
        }

        return wallCount;

    }


    int getSurroundingNeigbours(int positionX, int positionY)
    {
        int wallCount = 0;

        //Looping through a 3 by 3 grid to get neighbours
        for (int neighbourX = positionX - 1; neighbourX <= positionX + 1; neighbourX++)
        {
            for (int neighbourY = positionY - 1; neighbourY <= positionY + 1; neighbourY++)
            {

                //Making sure the neighboyrs are within the map 
                if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
                {
                    //Making sure the center position isnt counted
                    if (neighbourX != positionX || neighbourY != positionY)
                    {

                        if (map[neighbourX, neighbourY] == 1)
                            wallCount++;
                    }
                }
                else
                {
                    wallCount++;
                }

            }

        }

        return wallCount;

    }




    //Drawing Gizmos for walls and air
    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (map[x, y] == 1)
                    {
                        Gizmos.color = Color.black;
                    }
                    else if(map[x, y] == 0)
                    {
                        Gizmos.color = Color.clear;
                    }
                    else if(map[x,y] == 3)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else if(map[x,y] == 4)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else if(map[x, y] == 5)
                    {
                        Gizmos.color = Color.magenta;
                    }


                    Vector2 pos = new Vector2(x, y);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
