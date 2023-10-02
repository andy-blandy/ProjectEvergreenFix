using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public Vector2Int mapSize;
    string[,] tileType;
    //edge objects are stored in a string seperated by ',', this is for grid objects that can overlap
    string[,] horEdges;
    string[,] verEdges;
    List<GridObject> objects = new List<GridObject>();
    List<ResourceGrid> rGrids = new List<ResourceGrid>();

    //static player resource variables
    public int money;
    public int happiness;
    public int citizens;
    public int impact;

    //dynamic global player resource variables
    public int jobs;
    public int populationCap;
    
    public GameObject testHolder;
    public Camera gamecam;
    

    // Start is called before the first frame update
    void Start()
    {
        StaticManager.curGridManager = this;
        mapSize = new Vector2Int(6, 6);
        InitializeMap();
        TryAddObject(new Vector2Int(0, 2), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> {new PowerBuildingResource(15, 0)});

        TryAddObject(new Vector2Int(4, 0), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> { new PowerBuildingResource(0, 10) });

        TryAddObject(new Vector2Int(4, 4), new Vector2Int(2, 2), "thing", new List<ResourceBuildingType> { new PowerBuildingResource(0, 10) });
        TestDrawMap();
    }

    //updates calculated Jobs and population count from buildings with their requirements met
    public void GetGlobalDynamicResources()
    {
        jobs = 0;
        populationCap = 0;

        foreach(GridObject go in objects)
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
    }

    //spawns prefabs to represent the existing gridObjects in visual form, z position should be negated
    public void TestDrawMap()
    {
        foreach (Transform t in testHolder.transform) Destroy(t.gameObject);
        foreach(GridObject obj in objects)
        {
            Vector2 tempPos = Vector2.Lerp(obj.position, obj.position + obj.size + new Vector2Int(-1, -1), 0.5f);
            GameObject temp = (GameObject)Instantiate(Resources.Load("Building"), new Vector3(tempPos.x, 1, -tempPos.y), Quaternion.identity, testHolder.transform);
            temp.transform.localScale = new Vector3(obj.size.x-0.1f, 2, obj.size.y-0.1f);
        }
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.x; y++)
            {
                GameObject ground = (GameObject)Instantiate(Resources.Load("Floor"), new Vector3(x, 0, -y), Quaternion.identity, testHolder.transform);
                ground.transform.localScale = new Vector3(1, 0.01f, 1);
                Material tempMat = ground.GetComponent<MeshRenderer>().material;
                tempMat.color = new Color((y+1) / 6f, (x+1) / 6f, 0);
                if (x < mapSize.x - 1)
                {
                    if(GetEdgesOnSpace(new Vector2Int(x,y),true).Count > 0)
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Edge"), new Vector3(x + 0.5f, 0.05f, -y), Quaternion.identity, testHolder.transform);
                        temp.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                    }
                }
                if (y < mapSize.y - 1)
                {
                    if (GetEdgesOnSpace(new Vector2Int(x, y), false).Count > 0)
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Edge"), new Vector3(x, 0.05f, -(y + 0.5f)), Quaternion.identity, testHolder.transform);
                        temp.transform.localScale = new Vector3(1, 0.1f, 0.1f);
                    }
                }
            }
        }
    }

    //Places an edge object from the current mouse position.
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
        horEdges = new string[mapSize.x, mapSize.y - 1];
        verEdges = new string[mapSize.x - 1, mapSize.y];
        for(int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.x; y++)
            {
                tileType[x, y] = "grass";
                if (x < mapSize.x - 1) verEdges[x, y] = "";
                if (y < mapSize.y - 1) horEdges[x, y] = "";
            }
        }
        objects = new List<GridObject>();
    }

    //Attempts to add building object, returns false if placement failed
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
        foreach(GridObject obj in objects) //check for overlap with existing buildings
        {
            if (obj.DoesObjectOverlap(tempObject))
            {
                Debug.Log("can't place object due to overlap with existing object");
                return false;
            }
        }
        //check for blocking tile types here

        objects.Add(tempObject);
        foreach(ResourceGrid rg in rGrids)
        {
            rg.TryAddObject(tempObject);
        }
        return true;
    }

    //Attempts to add edge object, requires specification of whether the edge is vertical or not
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
        foreach(GridObject obj in objects) //check if object is in the way
        {
            if (obj.IsEdgeOverObject(location, isVertical))
            {
                Debug.Log("cannot place edge at " + location + " due to object in the way " + (isVertical ? " vertical edge" : " horizontal edge"));
                return false;
            }
        }

        //check for blocking tile types here
        if (isVertical) verEdges[location.x, location.y] += (edgesPresent.Count > 0 ? "," : "") + edgeType;
        else horEdges[location.x, location.y] += (edgesPresent.Count > 0 ? "," : "") + edgeType;
        EdgePoint newEdge = new EdgePoint(location, isVertical);
        List<ResourceGrid> gridsConnecting = new List<ResourceGrid>();
        foreach(ResourceGrid rg in rGrids)  //try to add the edge object to every existing resourceGrid
        {
            if (rg.TryAddEdge(newEdge, edgeType))
            {
                gridsConnecting.Add(rg);
                foreach(GridObject go in objects)
                {
                    rg.TryAddObject(go);
                }
            }
        }
        if(gridsConnecting.Count == 0) //if no grids connected, make a new grid for this new edge
        {
            //Make new grid
            ResourceGrid tempGrid = new ResourceGrid(newEdge, edgeType);
            foreach (GridObject obj in objects) tempGrid.TryAddObject(obj);
            rGrids.Add(tempGrid);
        }
        else if(gridsConnecting.Count > 1) //if connected to more than one grid, merge all grids connected to
        {
            ResourceGrid tempGrid = ResourceGrid.CombineResourceGrids(gridsConnecting);
            foreach (ResourceGrid rg in gridsConnecting) rGrids.Remove(rg);
            rGrids.Add(tempGrid);
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
                edge = verEdges[location.x, location.y];
            }
            else
            {
                edge = horEdges[location.x, location.y];
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestPlaceEdge();
        }

        //Debug.Log(objects[1].RequirementMet() + " building 1");
        //Debug.Log(objects[2].RequirementMet() + " building 2");
    }
}

//Combined edge object position and orientation, used for calculating resource grids
public class EdgePoint
{
    public Vector2Int position;
    public bool isVertical;
    public EdgePoint(Vector2Int _position, bool _isVertical)
    {
        position = _position;
        isVertical = _isVertical;
    }
}

//used for connecting buildings via edge objects
public class ResourceGrid
{
    public string edgeType;
    public List<EdgePoint> edges;
    public List<GridObject> connectedObjects;
    public List<ResourceChange> producingResources;
    public List<ResourceChange> requiredResources;

    public ResourceGrid(EdgePoint startingEdge, string _edgeType)
    {
        edgeType = _edgeType;
        edges = new List<EdgePoint>();
        edges.Add(startingEdge);
        connectedObjects = new List<GridObject>();
    }
    public ResourceGrid(string _edgeType)
    {
        edgeType = _edgeType;
        edges = new List<EdgePoint>();
        connectedObjects = new List<GridObject>();
    }

    //Calculates all dynamic resource changes for every connected object
    public void CalculateGridResources()
    {
        int calcTurns = 2;
        List<string> resourceTypes = GetResourcesCanCarry();
        do
        {
            Dictionary<string, int> totalResourceProduction = new Dictionary<string, int>();
            Dictionary<string, int> totalResourceRequirement = new Dictionary<string, int>();

            //get all resource requirements and productions that effect this grid
            foreach (GridObject go in connectedObjects)
            {
                foreach (DynamicBuildingResource rbt in go.resources.OfType<DynamicBuildingResource>())
                {
                    ResourceChange[] change = rbt.GetDynamicResourceValue();
                    if (resourceTypes.Contains(change[0].name))
                    {
                        if (change[0].valueChange > 0 && go.RequirementMet())
                        {
                            if (totalResourceProduction.ContainsKey(change[0].name)) totalResourceProduction[change[0].name] += change[0].valueChange;
                            else totalResourceProduction.Add(change[0].name, change[0].valueChange);
                        }
                        if (change[1].valueChange > 0)
                        {
                            if (totalResourceRequirement.ContainsKey(change[1].name)) totalResourceRequirement[change[1].name] += change[1].valueChange;
                            else totalResourceRequirement.Add(change[1].name, change[1].valueChange);
                        }
                        if (rbt.requiring > 0)
                        {
                            go.resourceRequirementsMet[rbt.GetResourceName()] = true;
                            Debug.Log("Building req set to true");
                        }
                    }
                }
            }

            //checks which resources are requiring more than is produced
            List<string> resourcesNotMet = new List<string>();
            foreach(KeyValuePair<string, int> kvp in totalResourceRequirement)
            {
                if (totalResourceProduction.ContainsKey(kvp.Key))
                {
                    if (totalResourceProduction[kvp.Key] < kvp.Value) resourcesNotMet.Add(kvp.Key);
                }
                else
                {
                    resourcesNotMet.Add(kvp.Key);
                }
            }

            //disables the newest buildings until resources requirement is less than production.
            foreach(string s in resourcesNotMet)
            {
                int resourceCount = totalResourceRequirement[s];
                for(int i = connectedObjects.Count-1; i >= 0; i--)
                {
                    foreach(DynamicBuildingResource rbt in connectedObjects[i].resources.OfType<DynamicBuildingResource>())
                    {
                        if(rbt.GetResourceName() == s && rbt.requiring > 0)
                        {
                            connectedObjects[i].resourceRequirementsMet[s] = false;
                            resourceCount -= rbt.requiring;
                            Debug.Log("building req set to false :" + resourceCount);
                            break;
                        }
                    }
                    int resourceProduction;
                    totalResourceProduction.TryGetValue(s, out resourceProduction);
                    if (resourceCount <= resourceProduction) break; 
                }
            }
            calcTurns--;
            //runs twice just as a reduncany measure
        } while (calcTurns <= 0);
    }

    //Attempts to add an edge object to the grid
    public bool TryAddEdge(EdgePoint point, string pointType)
    {
        if (pointType != edgeType) return false;
        foreach(EdgePoint ep in edges)
        {
            if (Vector2Int.Distance(point.position, ep.position) < 2)
            {
                if (DoesEdgeConnect(ep, point))
                {
                    edges.Add(point);
                    return true;
                }
            }
        }
        return false;
    }

    //Attempts to add an object to the grid, if added the grid will recheck all resource production
    public bool TryAddObject(GridObject obj)
    {
        //if the grid already has the object in question return false.
        if (connectedObjects.Contains(obj)) return false;

        List<EdgePoint> objectPerimeter = new List<EdgePoint>();
        for(int x = obj.position.x; x < obj.position.x + obj.size.x; x++)
        {
            objectPerimeter.Add(new EdgePoint(new Vector2Int(x, obj.position.y - 1), false));
            objectPerimeter.Add(new EdgePoint(new Vector2Int(x, obj.position.y + (obj.size.y-1)), false));
        }
        for (int y = obj.position.y; y < obj.position.y + obj.size.y; y++)
        {
            objectPerimeter.Add(new EdgePoint(new Vector2Int(obj.position.x - 1, y), true));
            objectPerimeter.Add(new EdgePoint(new Vector2Int(obj.position.x + (obj.size.x - 1), y), true));
        }
        foreach(EdgePoint ep in edges)
        {
            foreach(EdgePoint op in objectPerimeter)
            {
                if(ep.isVertical == op.isVertical && ep.position == op.position)
                {
                    connectedObjects.Add(obj);
                    CalculateGridResources();
                    return true;
                }
            }
        }
        return false;
    }

    //Used for combining two or more grids when they connect
    public static ResourceGrid CombineResourceGrids(List<ResourceGrid> rgList)
    {
        ResourceGrid tempGrid = new ResourceGrid(rgList[0].edgeType);
        foreach(ResourceGrid rg in rgList)
        {
            tempGrid.edges.AddRange(rg.edges);
            tempGrid.connectedObjects.AddRange(rg.connectedObjects);
        }
        List<int> indexesToClear = new List<int>();
        for(int i = 0; i < tempGrid.edges.Count-1; i++)
        {
            if (tempGrid.edges[i].position == tempGrid.edges[i + 1].position && tempGrid.edges[i].isVertical == tempGrid.edges[i + 1].isVertical) indexesToClear.Insert(0, i + 1);
        }
        foreach(int i in indexesToClear)
        {
            tempGrid.edges.RemoveAt(i);
        }
        indexesToClear = new List<int>();
        for (int i = 0; i < tempGrid.connectedObjects.Count - 1; i++)
        {
            if (tempGrid.connectedObjects[i].position == tempGrid.connectedObjects[i + 1].position) indexesToClear.Insert(0, i + 1);
        }
        foreach (int i in indexesToClear)
        {
            tempGrid.connectedObjects.RemoveAt(i);
        }

        tempGrid.CalculateGridResources();
        return tempGrid;
    }

    //calculates if two edge points are connected to eachother
    public static bool DoesEdgeConnect(EdgePoint point1, EdgePoint point2)
    {
        if (point1.isVertical == point2.isVertical)
        {
            if (point1.isVertical)
            {
                return Mathf.Abs(point1.position.y - point2.position.y) <= 1 && point1.position.x == point2.position.x;
            }
            else
            {
                return Mathf.Abs(point1.position.x - point2.position.x) <= 1 && point1.position.y == point2.position.y;
            }
        }
        else
        {
            if (point1.isVertical)
            {
                return (point2.position.x == point1.position.x || point2.position.x == point1.position.x + 1) && (point2.position.y == point1.position.y || point2.position.y == point1.position.y - 1);
            }
            else
            {
                return (point2.position.x == point1.position.x || point2.position.x == point1.position.x - 1) && (point2.position.y == point1.position.y || point2.position.y == point1.position.y + 1);
            }
        }
    }

    //returns a list of the resource type strings that this grid type can carry
    //Example: powerlines can only carry power resources, not transport
    List<string> GetResourcesCanCarry()
    {
        switch (edgeType)
        {
            default:
                return new List<string> { "Power"};
        }
    }
}

//The baseline object for buildings and other constructions, has a size and resource requirement, use type to determine the visuals of the object
public class GridObject
{
    public string type;
    public Vector2Int size;
    public Vector2Int position;
    public List<ResourceBuildingType> resources;
    public Dictionary<string, bool> resourceRequirementsMet = new Dictionary<string, bool>();

    //Buildings can only produce resources if they have all their resource requirments met
    public bool RequirementMet()
    {
        bool outcome = true;
        foreach(bool b in resourceRequirementsMet.Values)
        {
            if (!b) outcome = false;
        }
        return outcome;
    }

    public GridObject(string _type, Vector2Int _size, Vector2Int _position, List<ResourceBuildingType> _resources)
    {
        type = _type;
        size = _size;
        position = _position;
        resources = _resources;
        foreach(ResourceBuildingType rbt in resources)
        {
            if (rbt.requiring > 0) resourceRequirementsMet.Add(rbt.GetResourceName(), false);
        }
    }

    //is the grid location over a part of the object
    public bool IsLocOnObject(Vector2Int location)
    {
        for(int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(location == new Vector2Int(position.x + x, position.y + y))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //checks if another object overlaps with this object
    public bool DoesObjectOverlap(GridObject otherObject)
    {
        Vector2Int[] objectCorners = new Vector2Int[]
        {
            otherObject.position,
            new Vector2Int(otherObject.position.x + otherObject.size.x-1, otherObject.position.y),
            new Vector2Int(otherObject.position.x, otherObject.position.y + otherObject.size.y-1),
            new Vector2Int(otherObject.position.x + otherObject.size.x-1, otherObject.position.y + otherObject.size.y-1)
        };
        foreach(Vector2Int corner in objectCorners) if (corner.x - position.x < size.x && corner.x >= position.x && corner.y - position.y < size.y && corner.y >= position.y) return true;
        return false;
    }

    //checks if an edge is over an object, only applies to objects that are larger than 1 in one dimension
    public bool IsEdgeOverObject(Vector2Int location, bool isVertical)
    {
        if (isVertical)
        {
            if (location.x >= position.x && location.x < position.x + (size.x - 1) && location.y >= position.y && location.y <= position.y + (size.y - 1)) return true;
        }
        else
        {
            if (location.x >= position.x && location.x <= position.x + (size.x - 1) && location.y >= position.y && location.y < position.y + (size.y - 1)) return true;
        }
        return false;
    }
}

//the resources that buildings can require
public abstract class ResourceBuildingType 
{
    public int producing;
    public int requiring;

    public abstract string GetResourceName();
    public ResourceBuildingType(int producing, int requiring)
    {
        this.producing = producing;
        this.requiring = requiring;
    }
}

//static resources are calculated at the end of each turn.
//Static resources are additive, meaning that excess production/requirement will carry over between turns.
public abstract class StaticBuildingResource : ResourceBuildingType
{
    //The total resources changed for the player
    public virtual ResourceChange GetStaticResourceChange()
    {
        ResourceChange output = new ResourceChange();
        output.name = GetResourceName();
        output.valueChange = producing - requiring;
        return output;
    }
    public StaticBuildingResource(int producing, int requiring ) : base(producing, requiring) { }
}

//Dynamic resources are those that change freqently/effect the game during a turn.
//Dynamic resources are not additive, meaning that excess production/requirement does not carry over between turns.
public abstract class DynamicBuildingResource : ResourceBuildingType
{
    //The total resources available to the connected grid in two parts, producing, and requiring.
    //This is given seperately so that the grid can keep track of total resource needs better.
    //The first ResourceChange is the producing value, the second is the requiring value.
    public virtual ResourceChange[] GetDynamicResourceValue()
    {
        ResourceChange[] output = new ResourceChange[2];
        output[0] = new ResourceChange();
        output[1] = new ResourceChange();
        output[0].name = GetResourceName();
        output[1].name = GetResourceName();
        output[0].valueChange = producing;
        output[1].valueChange = requiring;
        return output;
    }

    public abstract bool IsGlobal();
    public DynamicBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class MoneyBuildingResource : StaticBuildingResource
{
    public override string GetResourceName()
    {
        return "Money";
    }
    public MoneyBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class PowerBuildingResource : DynamicBuildingResource
{
    public override string GetResourceName()
    {
        return "Power";
    }
    public override bool IsGlobal()
    {
        return false;
    }
    public PowerBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class TransportBuildingResource : DynamicBuildingResource
{
    public override string GetResourceName()
    {
        return "Transport";
    }
    public override bool IsGlobal()
    {
        return false;
    }
    public TransportBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class HappinessBuildingResource : StaticBuildingResource
{
    public override string GetResourceName()
    {
        return "Happiness";
    }
    public HappinessBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class JobsBuildingResource : DynamicBuildingResource
{
    public override string GetResourceName()
    {
        return "Jobs";
    }
    public override bool IsGlobal()
    {
        return true;
    }
    public JobsBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class PopulationCapBuildingResource : DynamicBuildingResource
{
    public override string GetResourceName()
    {
        return "PopulationCap";
    }
    public override bool IsGlobal()
    {
        return true;
    }
    public PopulationCapBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class CitizensBuildingResource : StaticBuildingResource
{
    public override string GetResourceName()
    {
        return "Citizens";
    }
    public CitizensBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}
public class ImpactBuildingResource : StaticBuildingResource
{
    public override string GetResourceName()
    {
        return "Impact";
    }
    public ImpactBuildingResource(int producing, int requiring) : base(producing, requiring) { }
}

//the combined resource name and value change for a resource
public class ResourceChange
{
    public string name;
    public int valueChange;
}