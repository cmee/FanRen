using UnityEngine;

public class Shentong
{

    public string shenTongName;
    public int unitDistance;
    public int needMp;
    public int damage;

    public string effPath;
    public string soundEffPath;

    //plane类型攻击神通的“半径”
    public int planeRadius;

    public ShentongType type = ShentongType.Gong_Ji;
    public ShentongAckType ackType = ShentongAckType.Point;

    public Shentong(string shenTongName, int unitDistance, int needMp, int damage, string effPath, string soundEffPath)
    {
        this.shenTongName = shenTongName;
        this.unitDistance = unitDistance;
        this.needMp = needMp;
        this.damage = damage;
        this.effPath = effPath;
        this.soundEffPath = soundEffPath;
    }

    public Shentong(string shenTongName, int unitDistance, int needMp, int damage, string effPath, string soundEffPath, int planeRadius)
    {
        this.shenTongName = shenTongName;
        this.unitDistance = unitDistance;
        this.needMp = needMp;
        this.damage = damage;
        this.planeRadius = planeRadius;
        this.effPath = effPath;
        this.soundEffPath = soundEffPath;
    }

}

public enum ShentongType
{
    Gong_Ji = 1,
    Fang_Yu = 2
}

public enum ShentongAckType
{
    Line = 1,
    //Ten = 2,
    Point = 3,
    Plane = 4
}
