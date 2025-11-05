using UnityEngine;
using System.Collections.Generic; 

public class GrassRoadGenerator : MonoBehaviour
{
    public GrassRoad _grassTile;

    public GameObject grassCube;
    public GameObject groundCube;
    public GameObject parentObject;

    public List<GameObject> cubeObj = new List<GameObject>();
    public GrassRoad[,] grassRoadTiles; 
    

    public int height = 5;
    public int width = 5; 

    public bool[,] tilesConnected; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grassRoadTiles = new GrassRoad[width, height]; 
        GenerateWholeField(width, height);
        GetConnectedTiles();
        tilesConnected = new bool[width, height];
        
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
                if ((i == 0 && (j == 0 || j == grassRoad.tile.GetLength(1))) || (i == grassRoad.tile.GetLength(1) || (j == 0 || j == grassRoad.tile.GetLength(1))))
                {
                    grassRoad.tile[i, j] = true;
                }
                else
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
    }

    public void GenerateTheTile(GrassRoad grassRoad, Vector3 startPos) 
    {
        for (int i = 0; i < grassRoad.tile.GetLength(0); i++)
        {
            for (int j = 0; j < grassRoad.tile.GetLength(1); j++)
            {
                if (_grassTile.tile[i, j])
                {                    
                    var prefabTile = Instantiate(grassCube, new Vector3(i + startPos.x, startPos.y, j + startPos.z), Quaternion.identity);
                    cubeObj.Add(prefabTile);
                    prefabTile.transform.SetParent(parentObject.transform); 
                }
                else
                {
                    var prefabTile = Instantiate(groundCube, new Vector3(i + startPos.x, startPos.y, j + startPos.z), Quaternion.identity);
                    cubeObj.Add(prefabTile);
                    prefabTile.transform.SetParent(parentObject.transform);
                }
            }
        }
    }

    public void GenerateWholeField(int width, int height) 
    {
        for(int i = 0; i < width; i++) 
        {
            for(int j = 0; j < height; j++) 
            {
                _grassTile = new GrassRoad();
                _grassTile.tile = new bool[3, 3];

                GenerateRandomTiles(_grassTile);
                grassRoadTiles[i, j] = _grassTile;  

                GenerateTheTile(_grassTile, new Vector3(0.5f + 3f * i, 0, 0.5f + 3f * j));

            }
        }
    }

    public void DestroyTile()
    {
        for(int i = 0; i < parentObject.transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void GetConnectedTiles() 
    {
        for(int i = 0; i <  width; i++) 
        {
            for(int j = 0; j < height; j++) 
            {
                grassRoadTiles[i, j].AssignConnectionTiles();  
            }
        }
    }

    

   
}

public class GrassRoad 
{
    public bool[,] tile;

    public bool centreTopCon;
    public bool centreLeftCon;
    public bool centreRightCon;
    public bool centreDownCon; 
      
    public GrassRoad() 
    {
        bool[,] tile = new bool[3, 3]; 
    }

    public bool isRoad(int x, int y) 
    {
        return tile[x, y]; 
    }

    public void AssignConnectionTiles() 
    {
        centreTopCon = tile[1, 2];
        centreLeftCon = tile[0, 1];
        centreRightCon = tile[2, 1];
        centreDownCon = tile[1, 0]; 
    }
        
}
