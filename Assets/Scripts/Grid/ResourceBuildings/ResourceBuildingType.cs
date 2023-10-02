using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

//the combined resource name and value change for a resource
public class ResourceChange
{
    public string name;
    public int valueChange;
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
    public StaticBuildingResource(int producing, int requiring) : base(producing, requiring) { }
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
