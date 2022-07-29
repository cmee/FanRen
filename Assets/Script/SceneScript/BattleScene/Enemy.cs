using UnityEngine.UI;

public class Enemy : BaseRole
{

    public void Init()
    {
        Shentong[] shentongs = new Shentong[12];
        shentongs[0] = new Shentong("ÆÕÍ¨¹¥»÷", 2, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

        InitRoleData(100, 100, 50, 10, 5, shentongs, 11, 2, TeamNum.TEAM_TWO);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

    public void Init(MyDBManager.RoleInfo enemyRoleInfo, Shentong[] shentongs)
    {
        //Shentong[] shentongs = new Shentong[12];
        shentongs[0] = new Shentong("ÆÕÍ¨¹¥»÷", 2, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

        InitRoleData(100, 100, 50, 10, 5, shentongs, 11, 2, TeamNum.TEAM_TWO);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

}
