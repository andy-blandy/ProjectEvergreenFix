using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
