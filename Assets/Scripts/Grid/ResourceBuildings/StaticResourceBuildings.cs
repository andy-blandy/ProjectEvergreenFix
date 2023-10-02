using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class MoneyBuildingResource : StaticBuildingResource
    {
        public override string GetResourceName()
        {
            return "Money";
        }
        public MoneyBuildingResource(int producing, int requiring) : base(producing, requiring) { }
    }

public class HappinessBuildingResource : StaticBuildingResource
{
    public override string GetResourceName()
    {
        return "Happiness";
    }
    public HappinessBuildingResource(int producing, int requiring) : base(producing, requiring) { }
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
