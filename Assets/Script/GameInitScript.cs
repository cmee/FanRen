using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitScript : MonoBehaviour
{

    private void Awake()
    {
        //if (PlayerPrefs.GetInt("isInited", 0) == 0)
        //{
        //    PlayerPrefs.SetInt("isInited", 1);

        //}
    }

}

//任务状态
public enum FRTaskState
{    
    InProgress = 1, //进行种
    Finished = 2, //完成
    Fail = 3, //失败
    Untrigger = 0 //还没有触发
}

//物品类型
public enum FRItemType
{
    Fabao = 1,//法宝
    CaiLiao = 2,//材料
    LingCao = 3,//灵草
    DanYao = 4,//丹药
    LingShou = 5,//灵兽
    LingChong = 6,//灵虫
    GongFa = 7,//功法
    DanFang = 8,//丹方
    Other = 9,//其他
    KuiLei = 10,//傀儡
    TianDiLingWu = 11,//天地灵物
    ShenTong = 12,//神通
    Story = 13//剧情
}

