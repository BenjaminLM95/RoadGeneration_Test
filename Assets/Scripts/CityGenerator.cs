using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public int cityWidth = 25;
    public int cityHeight = 25;
    public float blockSize = 5f;
    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    public GameObject parkPrefab;

    public int[,] cityMap; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cityMap = new int[cityWidth, cityHeight];

        GenerateMap(); 
        GenerateCity();
        PutTheBuildings(cityMap); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCity()
    {
        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityHeight; z++)
            {
                Vector3 position = new Vector3(x * blockSize, 0, z * blockSize);

                GameObject tempObj = new GameObject(); 

                switch (cityMap[x, z]) 
                {
                    case 0:
                        tempObj = roadPrefab;
                        break;
                    case 1: 
                        tempObj = buildingPrefab;
                        break;
                    case 2:
                        tempObj = parkPrefab;
                        break;
                    default:
                        tempObj = roadPrefab;
                        break;
                }

                Instantiate(tempObj, position, Quaternion.identity, transform);
                
            }
        }
    }

    void GenerateMap() 
    {
        for (int i = 0; i < cityWidth; i++) 
        {
            for(int j = 0; j < cityHeight; j++) 
            {
                if(i % 3 == 0 || j % (cityHeight/3) == 0) 
                {
                    cityMap[i, j] = 0; 
                }
                else 
                {
                    int rnd = Random.Range(0, 100);
                    if(rnd < 50) 
                    {
                        cityMap[i, j] = 1; 
                    }
                    else 
                    {
                        cityMap[i, j] = 2; 
                    }
                }
            }
        }
    }

    public int ObtainNum(int[,] map, int xCoord, int yCoord) 
    {
        return map[xCoord, yCoord];
    }

    public void GenerateBuildingHeight(int _height, Vector3 pos) 
    {
        for(int i = 0; i < _height; i++) 
        {
            Vector3 newPos = new Vector3(pos.x * blockSize, pos.y + i, pos.z * blockSize); 

            Instantiate(buildingPrefab, newPos, Quaternion.identity);
            Debug.Log(newPos); 
        }
    }

    public void PutTheBuildings(int[,] _map) 
    {
        for(int i = 0; i < _map.GetLength(0); i++) 
        {
            for(int j = 0; j < _map.GetLength(1); j++) 
            {
                int detNum = ObtainNum(_map, i, j);

                if (detNum != 1)
                {
                    //nothing
                }
                else
                {
                    int rndHeight = Random.Range(1, 6);
                    GenerateBuildingHeight(rndHeight, new Vector3(i, 0, j));
                }
                
            }
        }
    }
}
