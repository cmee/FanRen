using UnityEngine;
using UnityEngine.UI;

public class HanLi : BaseRole
{

    //todo 要从数据库查询出装备了哪些神通
    public void Init()
    {
        MyDBManager.GetInstance().ConnDB();
        Shentong[] shentongs = MyDBManager.GetInstance().GetRoleShentong(1, 1).ToArray();

        MyDBManager.RoleInfo roleInfo = MyDBManager.GetInstance().GetRoleInfo(1);

        //InitRoleData(100, 100, 50, 50, 10, 4, shentongs, 13, 1, TeamNum.TEAM_ONE);

        InitRoleData(roleInfo.currentHp, roleInfo.maxHp, roleInfo.currentMp, roleInfo.maxMp, roleInfo.gongJiLi, roleInfo.fangYuLi, shentongs, roleInfo.speed, roleInfo.roleId, TeamNum.TEAM_ONE);

        //Slider slide = GetSlide();
        //slide.maxValue = 100;
        //slide.minValue = 0;
        //slide.value = 100;
    }

}
