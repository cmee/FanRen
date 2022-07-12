using UnityEngine;
using UnityEngine.UI;

public class HanLi : BaseRole
{
    public void Init()
    {
        Shentong[] shentongs = new Shentong[12];
        shentongs[0] = new Shentong("Õ£ÑÛ½£·¨", 2, 0, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

        shentongs[1] = new Shentong("Ç¬À¶±ùÑæ", 8, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");
        shentongs[1].ackType = ShentongAckType.Line;

        shentongs[2] = new Shentong("´ó¸ý½£Õó", 6, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa", 4);
        shentongs[2].ackType = ShentongAckType.Plane;

        InitRoleData(100, 100, 50, 10, 4, shentongs, 13, 1, TeamNum.Us);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

}
