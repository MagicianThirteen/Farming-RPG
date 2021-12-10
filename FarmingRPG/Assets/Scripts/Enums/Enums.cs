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

//动画名字
public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkUp,
    walkDown,
    walkRight,
    walkLeft,
    runUp,
    runDown,
    runRight,
    runLeft,
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,
    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,
    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}

//带动画的部位
public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}

//动画颜色？
public enum PartVariantColour
{
    none,
    count
}

//要替换动画的动作或者说行为
public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}


