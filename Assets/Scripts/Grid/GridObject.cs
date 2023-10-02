using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The baseline object for buildings and other constructions, has a size and resource requirement, use type to determine the visuals of the object
public class GridObject
{
    public string type;
    public Vector2Int size;
    public Vector2Int position;
    public List<ResourceBuildingType> resources;
    public Dictionary<string, bool> resourceRequirementsMet = new Dictionary<string, bool>();

    // Buildings can only produce resources if they have all their resource requirments met
    public bool RequirementMet()
    {
        bool outcome = true;
        foreach (bool b in resourceRequirementsMet.Values)
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
        foreach (ResourceBuildingType rbt in resources)
        {
            if (rbt.requiring > 0) resourceRequirementsMet.Add(rbt.GetResourceName(), false);
        }
    }

    // is the grid location over a part of the object
    public bool IsLocOnObject(Vector2Int location)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (location == new Vector2Int(position.x + x, position.y + y))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // checks if another object overlaps with this object
    public bool DoesObjectOverlap(GridObject otherObject)
    {
        Vector2Int[] objectCorners = new Vector2Int[]
        {
            otherObject.position,
            new Vector2Int(otherObject.position.x + otherObject.size.x-1, otherObject.position.y),
            new Vector2Int(otherObject.position.x, otherObject.position.y + otherObject.size.y-1),
            new Vector2Int(otherObject.position.x + otherObject.size.x-1, otherObject.position.y + otherObject.size.y-1)
        };
        foreach (Vector2Int corner in objectCorners) if (corner.x - position.x < size.x && corner.x >= position.x && corner.y - position.y < size.y && corner.y >= position.y) return true;
        return false;
    }

    // checks if an edge is over an object, only applies to objects that are larger than 1 in one dimension
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
