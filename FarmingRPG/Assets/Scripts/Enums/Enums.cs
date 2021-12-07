public enum ToolEffect
{
    none,
    watering
}

//不同容器的定义
public enum InventoryLocation
{
    player=0,
    chest=1,
    count=2,//这里加的count是为了好方便统计总共有多个个比如用InventoryLocation.count
}

public enum Direction
{
    right,
    left,
    up,
    down
}

public enum ItemType
{
    Seed,
    Commodity,
    Watering_tool,
    Hoeing_tool,//锄头
    Chopping_tool,//砍
    Breaking_tool,
    Reaping_tool,//收割的工具
    Collecting_tool,
    Reapable_scenary,
    Furniture,
    none,
    count
}


