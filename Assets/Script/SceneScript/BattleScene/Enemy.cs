using System.Collections.Generic;
using UnityEngine.UI;

public class Enemy : BaseRole
{

    //public void Init()
    //{
    //    Shentong[] shentongs = new Shentong[12];
    //    shentongs[0] = new Shentong("ÆÕÍ¨¹¥»÷", 2, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

    //    InitRoleData(100, 100, 50, 50, 10, 5, shentongs, 11, 2, TeamNum.TEAM_TWO, "²âÊÔÓ×È®", "²âÊÔÓ×È®");

    //    //Slider slide = GetSlide();
    //    //slide.maxValue = 100;
    //    //slide.minValue = 0;
    //    //slide.value = 100;
    //}

    public void Init(int roleId, int index)
    {
        //Shentong[] shentongs = new Shentong[12];
        //shentongs[0] = new Shentong("ÆÕÍ¨¹¥»÷", 2, 5, 10, "Ef/ZhaYanJianFa", "SoundEff/ZhaYanJianFa");

        //InitRoleData(100, 100, 50, 10, 5, shentongs, 11, 2, TeamNum.TEAM_TWO);

        RoleInfo enemyRoleInfo = MyDBManager.GetInstance().GetRoleInfo(roleId);
        List<Shentong> enemyRoleShentongs = MyDBManager.GetInstance().GetRoleShentong(roleId, 1);

        Shentong[] tmp = new Shentong[12];
        for (int i = 0; i < enemyRoleShentongs.Count; i++)
        {
            tmp[i] = enemyRoleShentongs[i];
        }

        InitRoleData(enemyRoleInfo.currentHp, enemyRoleInfo.maxHp, enemyRoleInfo.currentMp, enemyRoleInfo.maxMp, enemyRoleInfo.gongJiLi, enemyRoleInfo.fangYuLi, tmp, enemyRoleInfo.speed, enemyRoleInfo.roleId, TeamNum.TEAM_TWO, enemyRoleInfo.roleName, enemyRoleInfo.roleName + index);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

}
