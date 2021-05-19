using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{

    public GameObject tile;

    public GameObject[,] tileMap;

    public Camera mainCamera;

    public float offset;

    public void CreateMap(int[,] newMap, Vector2 mapSize)
    {
        if(tileMap != null)
        {
            ClearMap();
        }

        tileMap = new GameObject[(int)mapSize.x, (int)mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {

                GameObject nTile = Instantiate(tile, new Vector3(x, 0, y), transform.rotation);

                if(newMap[x,y] == 0)
                {
                    nTile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                }
                else if(newMap[x,y] == 1)
                {
                    nTile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                }
                else if(newMap[x,y] == 3)
                {
                    nTile.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
                }

                nTile.transform.SetParent(this.transform);

                tileMap[x, y] = nTile;

            }
        }

        mainCamera.transform.position = new Vector3((mapSize.x / 2) + offset, 125, mapSize.y / 2);
        

    }

    void ClearMap()
    {
        foreach (GameObject tile in tileMap)
        {
            Destroy(tile.gameObject);
        }
    }
}

