using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public Vector2Int mapSize;
    string[,] tileType;

    // Edge objects are stored in a string seperated by ',', this is for grid objects that can overlap
    string[,] horizontalEdges;
    string[,] verticalEdges;

    List<GridObject> gridObjects = new List<GridObject>();
    List<ResourceGrid> resourceGrids = new List<ResourceGrid>();

    public GameObject mapHolder;
    public Camera gamecam;
    

    void Start()
    {
        StaticManager.curGridManager = this;
        InitializeMap();
        TestAddingObjects();
        TestDrawMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestPlaceEdge();
        }

        //Debug.Log(objects[1].RequirementMet() + " building 1");
        //Debug.Log(objects[2].RequirementMet() + " building 2");
    }

    void TestAddingObjects()
    {
        TryAddObject(new Vector2Int(0, 2), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> { new PowerBuildingResource(15, 0) });
        TryAddObject(new Vector2Int(4, 0), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> { new PowerBuildingResource(0, 10) });
        TryAddObject(new Vector2Int(4, 4), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> { new PowerBuildingResource(0, 10) });
    }

    /// <summary>
    /// Finds the jobs and population count from each building that has their requirements met
    /// </summary>
    /// <returns>An array containing the two dynamic resource values. dynamicResources[0] = jobs, dynamicResources[1] = populationCap</returns>
    public int[] GetGlobalDynamicResources()
    {
        int jobs = 0;
        int populationCap = 0;

        foreach(GridObject go in gridObjects)
        {
            if (go.RequirementMet())
            {
                foreach (DynamicBuildingResource dbr in go.resources.OfType<DynamicBuildingResource>())
                {
                    ResourceChange rc = dbr.GetDynamicResourceValue()[0];
                    if (rc.name == "Jobs") jobs += rc.valueChange;
                    if (rc.name == "PopulationCap") populationCap += rc.valueChange;
                }
            }
        }

        int[] dynamicResources = { jobs, populationCap };
        return dynamicResources;
    }

    // Spawns prefabs to represent the existing gridObjects in visual form, z position should be negated
    public void TestDrawMap()
    {
        foreach (Transform t in mapHolder.transform) Destroy(t.gameObject);
        foreach(GridObject obj in gridObjects)
        {
            Vector2 tempPos = Vector2.Lerp(obj.position, obj.position + obj.size + new Vector2Int(-1, -1), 0.5f);
            GameObject temp = (GameObject)Instantiate(Resources.Load("Building"), new Vector3(tempPos.x, 1, -tempPos.y), Quaternion.identity, mapHolder.transform);
            temp.transform.localScale = new Vector3(obj.size.x-0.1f, 2, obj.size.y-0.1f);
        }
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.x; y++)
            {
                GameObject ground = (GameObject)Instantiate(Resources.Load("Floor"), new Vector3(x, 0, -y), Quaternion.identity, mapHolder.transform);
                ground.transform.localScale = new Vector3(1, 0.01f, 1);
                Material tempMat = ground.GetComponent<MeshRenderer>().material;
                tempMat.color = new Color((y+1) / 6f, (x+1) / 6f, 0);
                if (x < mapSize.x - 1)
                {
                    if(GetEdgesOnSpace(new Vector2Int(x,y),true).Count > 0)
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Edge"), new Vector3(x + 0.5f, 0.05f, -y), Quaternion.identity, mapHolder.transform);
                        temp.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                    }
                }
                if (y < mapSize.y - 1)
                {
                    if (GetEdgesOnSpace(new Vector2Int(x, y), false).Count > 0)
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Edge"), new Vector3(x, 0.05f, -(y + 0.5f)), Quaternion.identity, mapHolder.transform);
                        temp.transform.localScale = new Vector3(1, 0.1f, 0.1f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Places an edge object at the current mouse position
    /// </summary>
    void TestPlaceEdge()
    {
        RaycastHit rayHit;
        Physics.Raycast(gamecam.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity);
        if (rayHit.collider != null) // if raycast doesn't hit a ground tile, don't continue
        {
            Vector3 mouseWorldPos = rayHit.point;
            Vector2 gridPos = new Vector2(mouseWorldPos.x, -mouseWorldPos.z);
            bool placingVertical = false;
            if (Mathf.Abs((gridPos.x % 1f) - 0.5f) < Mathf.Abs((gridPos.y % 1f) - 0.5f)) placingVertical = true; //if the mouse is closer to the vertical edge, place the edge as vertical edge object
            Vector2Int gridEdgePos;
            if (placingVertical)
            {
                gridEdgePos = new Vector2Int(Mathf.RoundToInt(gridPos.x - 0.5f), Mathf.RoundToInt(gridPos.y));
                if (gridEdgePos.x > -1 && gridEdgePos.x < mapSize.x - 1 && gridEdgePos.y > -1 && gridEdgePos.y < mapSize.y)
                {
                    TryAddEdge(gridEdgePos, "test", true);
                    TestDrawMap();
                }
            }
            else
            {
                gridEdgePos = new Vector2Int(Mathf.RoundToInt(gridPos.x), Mathf.RoundToInt(gridPos.y - 0.5f));
                if (gridEdgePos.x > -1 && gridEdgePos.x < mapSize.x && gridEdgePos.y > -1 && gridEdgePos.y < mapSize.y - 1)
                {
                    TryAddEdge(gridEdgePos, "test", false);
                    TestDrawMap();
                }
            }
        }
    }

    //Initializes the map variables
    public void InitializeMap()
    {
        tileType = new string[mapSize.x, mapSize.y];
        horizontalEdges = new string[mapSize.x, mapSize.y - 1];
        verticalEdges = new string[mapSize.x - 1, mapSize.y];
        for(int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                tileType[x, y] = "grass";
                if (x < mapSize.x - 1) verticalEdges[x, y] = "";
                if (y < mapSize.y - 1) horizontalEdges[x, y] = "";
            }
        }
        gridObjects = new List<GridObject>();
    }

    /// <summary>
    /// Attempts to add a building object
    /// </summary>
    /// <param name="location">x, z position to place building</param>
    /// <param name="size">The size of the building object</param>
    /// <param name="objType">The type of building to place</param>
    /// <param name="resources"></param>
    /// <returns>False if the placement failed</returns>
    public bool TryAddObject(Vector2Int location, Vector2Int size, string objType, List<ResourceBuildingType> resources)
    {
        GridObject tempObject = new GridObject(objType, size, location, resources);

        if(size.x > 1) //check for overlaping vertical edges if building is wide on the x axis
        {
            for (int y = location.y; y < location.y + size.y; y++) 
            {
                for (int x = location.x; x < location.x + (size.x - 1); x++)
                {
                    if(GetEdgesOnSpace(new Vector2Int(x, y), true).Count > 0)
                    {
                        Debug.Log("can't place object due to edge object present");
                        return false;
                    }
                }
            }
        }
        if(size.y > 1) //check for overlaping horizontal edges if building is wide on the y axis
        {
            for (int x = location.x; x < location.x + size.x; x++)
            {
                for (int y = location.y; y < location.y + (size.y - 1); y++)
                {
                    if (GetEdgesOnSpace(new Vector2Int(x, y), false).Count > 0)
                    {
                        Debug.Log("can't place object due to edge object present");
                        return false;
                    }
                }
            }
        }
        foreach(GridObject obj in gridObjects) //check for overlap with existing buildings
        {
            if (obj.DoesObjectOverlap(tempObject))
            {
                Debug.Log("can't place object due to overlap with existing object");
                return false;
            }
        }
        //check for blocking tile types here

        gridObjects.Add(tempObject);
        foreach(ResourceGrid rg in resourceGrids)
        {
            rg.TryAddObject(tempObject);
        }
        return true;
    }

    //Attempts to add edge object, requires specification of whether the edge is vertical or not
    /// <summary>
    /// Attempt to add an edge object
    /// </summary>
    /// <param name="location">x, z coordinate of placement</param>
    /// <param name="edgeType">Type of edge to place</param>
    /// <param name="isVertical">Is the edge vertical or not</param>
    /// <returns>False if unable to place edge</returns>
    public bool TryAddEdge(Vector2Int location, string edgeType, bool isVertical)
    {
        if(!(groundEdges.Contains(edgeType) || undergroundEdges.Contains(edgeType) || elevatedEdges.Contains(edgeType))) //check if edge object is an valid type
        {
            Debug.Log(edgeType + " is not a valid edge object");
            return false;
        }
        int edgeSuperType = 0; //Get the edge's supertype, edges of different supertypes can overlap. 0: ground, 1: underground, 2: elevated
        if (undergroundEdges.Contains(edgeType)) edgeSuperType = 1;
        if (elevatedEdges.Contains(edgeType)) edgeSuperType = 2;
        List<string> edgesPresent = GetEdgesOnSpace(location, isVertical);
        if (edgesPresent.Contains(edgeType)) //check if exact edge already exists
        {
            Debug.Log(edgeType + " edge object is already present at " + location + (isVertical ? " vertical edge" : " horizontal edge"));
            return false;
        }
        foreach(string edge in edgesPresent) //check for supertype conflicts
        {
            if(groundEdges.Contains(edge) && edgeSuperType == 0)
            {
                Debug.Log("there's no space for another ground edge at " + location + (isVertical ? " vertical edge" : " horizontal edge"));
                return false;
            }else if (undergroundEdges.Contains(edge) && edgeSuperType == 1)
            {
                Debug.Log("there's no space for another underground edge at " + location + (isVertical ? " vertical edge" : " horizontal edge"));
                return false;
            }else if (elevatedEdges.Contains(edge) && edgeSuperType == 2)
            {
                Debug.Log("there's no space for another elevated edge at " + location + (isVertical ? " vertical edge" : " horizontal edge"));
                return false;
            }
        }
        foreach(GridObject obj in gridObjects) //check if object is in the way
        {
            if (obj.IsEdgeOverObject(location, isVertical))
            {
                Debug.Log("cannot place edge at " + location + " due to object in the way " + (isVertical ? " vertical edge" : " horizontal edge"));
                return false;
            }
        }

        //check for blocking tile types here
        if (isVertical) verticalEdges[location.x, location.y] += (edgesPresent.Count > 0 ? "," : "") + edgeType;
        else horizontalEdges[location.x, location.y] += (edgesPresent.Count > 0 ? "," : "") + edgeType;
        EdgePoint newEdge = new EdgePoint(location, isVertical);
        List<ResourceGrid> gridsConnecting = new List<ResourceGrid>();
        foreach(ResourceGrid rg in resourceGrids)  //try to add the edge object to every existing resourceGrid
        {
            if (rg.TryAddEdge(newEdge, edgeType))
            {
                gridsConnecting.Add(rg);
                foreach(GridObject go in gridObjects)
                {
                    rg.TryAddObject(go);
                }
            }
        }
        if(gridsConnecting.Count == 0) //if no grids connected, make a new grid for this new edge
        {
            //Make new grid
            ResourceGrid tempGrid = new ResourceGrid(newEdge, edgeType);
            foreach (GridObject obj in gridObjects) tempGrid.TryAddObject(obj);
            resourceGrids.Add(tempGrid);
        }
        else if(gridsConnecting.Count > 1) //if connected to more than one grid, merge all grids connected to
        {
            ResourceGrid tempGrid = ResourceGrid.CombineResourceGrids(gridsConnecting);
            foreach (ResourceGrid rg in gridsConnecting) resourceGrids.Remove(rg);
            resourceGrids.Add(tempGrid);
        }
        return true;
    }

    //Gets every edge object at position
    public List<string> GetEdgesOnSpace(Vector2Int location, bool isVertical)
    {
        string edge;
        try
        {
            if (isVertical)
            {
                edge = verticalEdges[location.x, location.y];
            }
            else
            {
                edge = horizontalEdges[location.x, location.y];
            }
        }
        catch 
        {
            return new List<string>();
        }
        if (edge.Replace(" ", "") == "") return new List<string>();
        return edge.Replace(" ", "").Split(new char[] { ',' }).ToList();
    }

    //edge objects that take up the ground space
    static string[] groundEdges = new string[]
    {
        "test"
    };
    //edge objects that take up the underground space
    static string[] undergroundEdges = new string[]
    {

    };
    //edge objects that take up the above ground space
    static string[] elevatedEdges = new string[]
    {

    };
}