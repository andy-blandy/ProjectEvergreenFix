using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Combined edge object position and orientation, used for calculating resource grids
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

// Used for connecting buildings via edge objects
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
            foreach (KeyValuePair<string, int> kvp in totalResourceRequirement)
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
            foreach (string s in resourcesNotMet)
            {
                int resourceCount = totalResourceRequirement[s];
                for (int i = connectedObjects.Count - 1; i >= 0; i--)
                {
                    foreach (DynamicBuildingResource rbt in connectedObjects[i].resources.OfType<DynamicBuildingResource>())
                    {
                        if (rbt.GetResourceName() == s && rbt.requiring > 0)
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
        foreach (EdgePoint ep in edges)
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
        for (int x = obj.position.x; x < obj.position.x + obj.size.x; x++)
        {
            objectPerimeter.Add(new EdgePoint(new Vector2Int(x, obj.position.y - 1), false));
            objectPerimeter.Add(new EdgePoint(new Vector2Int(x, obj.position.y + (obj.size.y - 1)), false));
        }
        for (int y = obj.position.y; y < obj.position.y + obj.size.y; y++)
        {
            objectPerimeter.Add(new EdgePoint(new Vector2Int(obj.position.x - 1, y), true));
            objectPerimeter.Add(new EdgePoint(new Vector2Int(obj.position.x + (obj.size.x - 1), y), true));
        }
        foreach (EdgePoint ep in edges)
        {
            foreach (EdgePoint op in objectPerimeter)
            {
                if (ep.isVertical == op.isVertical && ep.position == op.position)
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
        foreach (ResourceGrid rg in rgList)
        {
            tempGrid.edges.AddRange(rg.edges);
            tempGrid.connectedObjects.AddRange(rg.connectedObjects);
        }
        List<int> indexesToClear = new List<int>();
        for (int i = 0; i < tempGrid.edges.Count - 1; i++)
        {
            if (tempGrid.edges[i].position == tempGrid.edges[i + 1].position && tempGrid.edges[i].isVertical == tempGrid.edges[i + 1].isVertical) indexesToClear.Insert(0, i + 1);
        }
        foreach (int i in indexesToClear)
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
                return new List<string> { "Power" };
        }
    }
}
