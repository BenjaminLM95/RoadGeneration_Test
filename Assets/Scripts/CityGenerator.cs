using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public int cityWidth = 25;
    public int cityHeight = 25;
    public float blockSize = 5f;
    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    public GameObject parkPrefab;
    public GameObject sidewalkPrefab; 

    public int[,] cityMap;
    public int[,] tempCityMap; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cityMap = new int[cityWidth, cityHeight];

        GenerateMap(); 
        tempCityMap = new int[cityWidth, cityHeight];
        GettingACopyOfMap(cityMap, tempCityMap); 
        AddingSideWalk(cityMap, tempCityMap); 
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
                

                switch (cityMap[x, z]) 
                {
                    case 0:
                        Instantiate(roadPrefab, position, Quaternion.identity);                        
                        break;
                    case 1:
                        Instantiate(buildingPrefab, position, Quaternion.identity);                        
                        break;
                    case 2:
                        Instantiate(parkPrefab, position, Quaternion.identity);                        
                        break;
                    case 3:
                        Instantiate(sidewalkPrefab, position, Quaternion.identity);                        
                        break; 
                    default:
                        Instantiate(roadPrefab, position, Quaternion.identity);                        
                        break;
                }
                                
                
            }
        }
    }

    void GenerateMap() 
    {
        for (int i = 0; i < cityWidth; i++) 
        {
            for(int j = 0; j < cityHeight; j++) 
            {
                if(i % 6 == 0 || i % 6 == 1 || j % (cityHeight/3) == 0 || j % (cityHeight / 3) == 1) 
                {
                    cityMap[i, j] = 0; 
                }
                else 
                {
                    int rnd = Random.Range(0, 100);
                    if(rnd < 80) 
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
            //Debug.Log(newPos); 
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
                    int rndHeight = Random.Range(6, 13);
                    GenerateBuildingHeight(rndHeight, new Vector3(i, 0, j));
                }
                
            }
        }
    }

    public int CountingNeighbours(int[,] map, int x, int y) 
    {
        int count = 0; 

        for(int i = x - 1; i <= x + 1; i++) 
        {
            for(int j = y - 1; j <= y + 1; j++) 
            {
                if(i >= 0 && j >= 0 && i <  map.GetLength(0) && j < map.GetLength(1))  // Avoid getting out of bounds of the array
                {
                    if ((x == i || y == j) && !(i == x && j == y))  // Selecting only the neighbours
                    {
                        if (map[i, j] == 1 || map[i, j] == 2)  // The result should be equal to building or park tile (1 or 2)
                        {
                            
                            count++;
                        }
                    }
                }
            }
        }
        
        return count; 
    }

    public void GettingACopyOfMap(int[,] map, int[,] copyMap) 
    { 
        for(int i = 0; i < map.GetLength(0); i++) 
        {
            for(int j = 0; j < map.GetLength(1); j++) 
            {
                copyMap[i,j] = map[i, j];
            }
        }
    }

    public void AddingSideWalk(int[,] _map, int[,] _tempMap) 
    {
        
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                if((_map[i,j] == 1 || _map[i,j] == 2)) 
                {   
                   if(CountingNeighbours(_map, i, j) < 4) 
                    {                        
                        _tempMap[i, j] = 3;
                    }
                }

            }
        }

        GettingACopyOfMap(_tempMap, _map);        

    }

}
