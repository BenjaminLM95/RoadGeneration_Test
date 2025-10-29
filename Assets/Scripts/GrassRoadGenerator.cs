using UnityEngine;
using System.Collections.Generic; 

public class GrassRoadGenerator : MonoBehaviour
{
    public GrassRoad _grassTile;

    public GameObject grassCube;
    public GameObject groundCube;

    public List<GameObject> cubeObj = new List<GameObject>(); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _grassTile = new GrassRoad();
        _grassTile.tile = new bool[3, 3]; 

        GenerateRandomTiles(_grassTile);

        GenerateTheTile(_grassTile); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRandomTiles(GrassRoad grassRoad) 
    {
        for (int i = 0; i < grassRoad.tile.GetLength(0); i++)
        {
            for (int j = 0; j < grassRoad.tile.GetLength(1); j++)
            {
                int rndNum = Random.Range(0, 100);
                if (rndNum > 50)
                {
                    grassRoad.tile[i, j] = true;
                }
                else
                {
                    grassRoad.tile[i, j] = false;
                }
            }
        }
    }

    public void GenerateTheTile(GrassRoad grassRoad) 
    {
        for (int i = 0; i < grassRoad.tile.GetLength(0); i++)
        {
            for (int j = 0; j < grassRoad.tile.GetLength(1); j++)
            {
                if (_grassTile.tile[i, j])
                {
                    var prefabTile = Instantiate(grassCube, new Vector3(i + 0.5f, 0, j + 0.5f), Quaternion.identity);
                    cubeObj.Add(prefabTile); 
                }
                else
                {
                    var prefabTile = Instantiate(groundCube, new Vector3(i + 0.5f, 0, j + 0.5f), Quaternion.identity);
                    cubeObj.Add(prefabTile);
                }
            }
        }
    }

    public void DestroyTile() 
    {

    }
}

public class GrassRoad 
{
    public bool[,] tile;
      
    public GrassRoad() 
    {
        bool[,] tile = new bool[3, 3]; 
    }
        
}
