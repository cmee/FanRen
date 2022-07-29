using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTriggerToBattleScript : MonoBehaviour, IColliderWithCC
{

    public int[] roleId;

    public int[] countOfRoleId;

    public string[] rolePrefabPath;

    public void OnPlayerCollisionEnter(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            Debug.Log("韩立过来了，开始战斗");

            RootBattleInit.enemyRoleIds = roleId;
            RootBattleInit.countOfEnemyRole = countOfRoleId;
            RootBattleInit.enemyRolePrefabPath = rolePrefabPath;

            SceneManager.LoadScene(2);
        }
    }

    public void OnPlayerCollisionExit(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            Debug.Log("韩立离开了");
        }
    }

}
