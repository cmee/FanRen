using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBattleInit : BaseMono
{

    public GameObject[] roles = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        GameObject hanLiGO = GameObject.FindGameObjectWithTag("HanLi");
        HanLi hanLiCS = hanLiGO.GetComponent<HanLi>();
        hanLiCS.Init();
        hanLiCS.InitRoleBattelePos(28, 28);

        GameObject enemyGO = GameObject.FindGameObjectWithTag("Enemy");
        Enemy enemyCS = enemyGO.GetComponent<Enemy>();
        enemyCS.Init();
        enemyCS.InitRoleBattelePos(16, 16);

        
        roles[0] = hanLiGO;
        roles[1] = enemyGO;

        GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().Init(roles);

        GameObject.FindGameObjectWithTag("Terrain").GetComponent<BattleController>().Init(roles);
    }    

}
