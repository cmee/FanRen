using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUtil
{

    public static void SaveGameObjLastState(GameObject hanLi)
    {
        Debug.LogWarning("保存角色数据 scene " + hanLi.scene.name);
        PlayerPrefs.SetInt("lastSceneBuildIndex", hanLi.scene.buildIndex);
        PlayerPrefs.SetFloat("lastPositionX", hanLi.transform.position.x);
        PlayerPrefs.SetFloat("lastPositionY", hanLi.transform.position.y);
        PlayerPrefs.SetFloat("lastPositionZ", hanLi.transform.position.z);
    }


    public static int GetLastSceneBuildIndex()
    {
        return PlayerPrefs.GetInt("lastSceneBuildIndex", -1);
    }

    public static Vector3 GetLastPosition()
    {
        float x =  PlayerPrefs.GetFloat("lastPositionX", -1f);
        if (x == -1f) return Vector3.zero;

        float y = PlayerPrefs.GetFloat("lastPositionY", -1f);
        float z = PlayerPrefs.GetFloat("lastPositionZ", -1f);
        return new Vector3(x, y, z);
    }

}
