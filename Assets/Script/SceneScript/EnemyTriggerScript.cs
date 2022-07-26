using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerScript : MonoBehaviour
{

    public int[] roleId;

    public int[] countOfRoleId;

    public string[] rolePrefabPath;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Debug.Log("韩立过来了，开始战斗");
        }
    }

}
