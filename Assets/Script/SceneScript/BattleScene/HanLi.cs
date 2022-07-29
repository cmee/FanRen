using UnityEngine;
using UnityEngine.UI;

public class HanLi : BaseRole
{

    //todo 要从数据库查询出装备了哪些神通
    public void Init()
    {
        Shentong[] shentongs = new Shentong[12];
        shentongs[0] = new Shentong("眨眼剑法", 2, 0, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

        shentongs[1] = new Shentong("乾蓝冰焰", 8, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");
        shentongs[1].ackType = ShentongAckType.Line;

        shentongs[2] = new Shentong("大庚剑阵", 6, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa", 4);
        shentongs[2].ackType = ShentongAckType.Plane;

        InitRoleData(100, 100, 50, 10, 4, shentongs, 13, 1, TeamNum.TEAM_ONE);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

}
