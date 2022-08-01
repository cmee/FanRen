using System.Collections.Generic;
using UnityEngine;

public class RootBattleInit : BaseMono
{

    public GameObject[] roles;

    public static int[] enemyRoleIds; //从数据库查询角色属性

    public static int[] countOfEnemyRole; //对应数量

    public static string[] enemyRolePrefabPath; //人物预制体路径

    private void OnDestroy()
    {
        enemyRoleIds = null;
        countOfEnemyRole = null;
        enemyRolePrefabPath = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        //roles = new GameObject[enemyRoleIds.Length + 1 + (队友?傀儡？灵兽？灵虫？)];

        

        if (enemyRoleIds == null) //todo for test
        {
            roles[0].SetActive(true);
            roles[1].SetActive(true);

            GameObject hanLiGameObj = roles[0];
            HanLi hanLiCS = hanLiGameObj.GetComponent<HanLi>();
            hanLiCS.Init();
            hanLiCS.InitRoleBattelePos(15, 15); //todo
            
            Enemy enemyCS = roles[1].GetComponent<Enemy>();
            enemyCS.Init();
            enemyCS.InitRoleBattelePos(25, 25);

            GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().Init(roles);
            GameObject.FindGameObjectWithTag("Terrain").GetComponent<BattleController>().Init(roles);
        }
        else
        {
            List<GameObject> roleList = new List<GameObject>();
            //roles = new GameObject[enemyRoleIds.Length + 1]; //todo
            GameObject hanLiPrefab = Resources.Load<GameObject>("Prefab/RolePrefab/HanLi");
            GameObject hanLiGameObj = Instantiate(hanLiPrefab);
            HanLi hanLiCS = hanLiGameObj.AddComponent<HanLi>();
            hanLiCS.Init();
            hanLiCS.InitRoleBattelePos(5, 5); //todo
            //roles[0] = hanLiGameObj;
            roleList.Add(hanLiGameObj);

            MyDBManager.GetInstance().ConnDB();
            for (int i = 0; i < enemyRoleIds.Length; i++)
            {
                MyDBManager.RoleInfo enemyRoleInfo = MyDBManager.GetInstance().GetRoleInfo(enemyRoleIds[i]);
                List<Shentong> enemyRoleShentongs = MyDBManager.GetInstance().GetRoleShentong(enemyRoleIds[i], 1);
                for (int j = 0; j < countOfEnemyRole[i]; j++)
                {
                    GameObject enemyRolePrefab = Resources.Load<GameObject>(enemyRolePrefabPath[i]);
                    GameObject enemyRoleGameObj = Instantiate(enemyRolePrefab);
                    Enemy enemyCS = enemyRoleGameObj.AddComponent<Enemy>();
                    enemyCS.Init(enemyRoleInfo, enemyRoleShentongs.ToArray());
                    enemyCS.InitRoleBattelePos(6 + i, 6 + i); //todo
                    roleList.Add(enemyRoleGameObj);
                }
            }

            roles = roleList.ToArray();
            GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().Init(roles);
            GameObject.FindGameObjectWithTag("Terrain").GetComponent<BattleController>().Init(roles);
        }

    }    

}
