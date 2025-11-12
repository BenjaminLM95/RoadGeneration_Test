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
    public GameObject parentObj;
    public GameObject lightTrafficObj;

    public int blockWidth;
    public int blockHeight; 

    public int[,] cityMap;
    public int[,] tempCityMap;
    public float[,] buildingHeightPatron; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cityMap = new int[cityWidth, cityHeight];
        buildingHeightPatron = new float[cityWidth, cityHeight];        
        tempCityMap = new int[cityWidth, cityHeight];

        blockWidth = Random.Range(5, Mathf.Max(7, cityWidth / 2));
        


        BuildingTheCity(cityMap, tempCityMap, buildingHeightPatron);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyCity();
            BuildingTheCity(cityMap, tempCityMap, buildingHeightPatron);
        }
    }

    void BuildingTheCity(int[,] _cityMap, int[,] _copyCityMap, float[,] _heighRanges) 
    {
        blockWidth = Random.Range(5, Mathf.Max(7, cityWidth / 2));
        GenerateMap();        
        GettingACopyOfMap(_cityMap, _copyCityMap);
        AddingSideWalk(_cityMap, _copyCityMap);
        GenerateCity();
        GetRandomHeight(_heighRanges);
        InsertBuilding(_cityMap, _heighRanges);
        AddTrafficLight(_cityMap); 
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
                        Instantiate(roadPrefab, position, Quaternion.identity, parentObj.transform);                        
                        break;
                    case 1:
                        //Instantiate(buildingPrefab, position, Quaternion.identity);                      
                        break;
                    case 2:
                        Instantiate(parkPrefab, position, Quaternion.identity, parentObj.transform);                        
                        break;
                    case 3:
                        Instantiate(sidewalkPrefab, position, Quaternion.identity, parentObj.transform);                        
                        break; 
                    default:
                        Instantiate(roadPrefab, position, Quaternion.identity, parentObj.transform);                        
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
                if(i % blockWidth == 0 || i % blockWidth == 1 || j % (cityHeight/3) == 0 || j % (cityHeight / 3) == 1) 
                {
                    cityMap[i, j] = 0; 
                }
                else 
                {
                    int rnd = Random.Range(0, 100);
                    if(rnd < 20) 
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


    public void InsertBuilding(int[,] _map, float[,] heightMap) 
    {
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                if (_map[i,j] == 1) 
                {
                    GameObject buildingObj = buildingPrefab;
                    //buildingObj = buildingPrefab;
                    buildingObj.transform.localScale = new Vector3(buildingObj.transform.localScale.x , 25 * Random.Range(1, 10 * (int)heightMap[i,j]) * heightMap[i,j], buildingObj.transform.localScale.z);                      
                    Instantiate(buildingObj, new Vector3(i * blockSize, buildingObj.transform.localScale.y / 2, j * blockSize), Quaternion.identity, parentObj.transform);
                   
                }

            }
        }
    }

    public void GetRandomHeight(float[,] arrayHeight)
    {
        for (int i = 0; i < arrayHeight.GetLength(0); i++)
        {
            for (int j = 0; j < arrayHeight.GetLength(1); j++) 
            {
                float centreX = (float)arrayHeight.GetLength(0) / 2f;                
                float centreY = (float)arrayHeight.GetLength(1) / 2f;                

                float numDisX = 1f - (Mathf.Abs((float)i - centreX)) / centreX;    // This calculates how close it is from the centre of X
                float numDisY = 1f - (Mathf.Abs((float)j - centreY))/ centreY;  // This calculates how close it is from the centre of Y
                float numDis = (numDisX + numDisY)/2;    

                //Debug.Log($"({i}, {j}): {numDis}"); 
                arrayHeight[i, j] = numDis;                 
            }
        }
    }


    public void AddTrafficLight(int[,] _cityMap) 
    {
        for(int i = 0; i < _cityMap.GetLength(0); i++) 
        {
            for(int j = 0; j < _cityMap.GetLength(1); j++) 
            {
                if (_cityMap[i,j] == 3 && CountingNeighbours(_cityMap, i, j) == 0) 
                {
                    Instantiate(lightTrafficObj, new Vector3(i * blockSize + 2, 4.5f, j * blockSize + 2), Quaternion.identity, parentObj.transform);
                    Instantiate(lightTrafficObj, new Vector3(i * blockSize - 2, 4.5f, j * blockSize - 2), Quaternion.Euler(0f, 90f, 0f), parentObj.transform);
                }
            }
        }
    }

    public void DestroyCity() 
    {
        for (int i = parentObj.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentObj.transform.GetChild(i);

            // Destroy in Play Mode
            Destroy(child.gameObject);

            // If you want it to work in Edit Mode too, use:
            // DestroyImmediate(child.gameObject);
        }
    }


}
