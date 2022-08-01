using UnityEngine;

public class Shentong
{

    public string shenTongName;
    public int unitDistance;
    public int needMp;
    public int damage;

    public string effPath; //特效文件路径
    public string soundEffPath; //音效文件路径

    //plane类型攻击神通的“半径”
    public int planeRadius;

    //public ShentongEffType type = ShentongEffType.Gong_Ji;
    //public ShentongRangeType ackType = ShentongRangeType.Point;


    public int shenTongId;
    public int isActive;
    public int roleId;
    //string shenTongName = (string)sdr["name"];
    //int damage = (int)((Int64)sdr["damage"]);
    public int defence;
    public string desc;
    public int studyRequireLevel;
    public ShentongEffType effType; //神通类型，攻击、防御、变身 等等
    public ShentongRangeType rangeType; //攻击范围类型，一条、一个面、一个点 等等

    public Shentong()
    {
    }

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

public enum ShentongEffType
{
    None = 0,
    Gong_Ji = 1,
    Fang_Yu = 2,
    Bian_Shen = 3 //变身、秘法 等
}

public enum ShentongRangeType
{
    None = 0,
    Line = 1,
    //Ten = 2,
    Point = 3,
    Plane = 4
}

public enum ShenTongStudyLevel
{
    FanRen = 1,
    LianQi = 2,
    ZhuJi = 3,
    JieDan = 4,
    YuanYing = 5,
    HuaShen = 6
}