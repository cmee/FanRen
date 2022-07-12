using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtil
{
    public static void NotifyTaskUIDatasetChanged()
    {
        GameObject topGO = GameObject.Find("TaskScrollView");
        GameObject parentGO = GameObject.Find("ScrollViewContent");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("TaskUICell");
        if (gameObjects.Length > 0)
        {
            foreach (GameObject item in gameObjects)
            {
                GameObject.Destroy(item);
            }
        }
        MyDBManager.GetInstance().ConnDB();
        List<MyDBManager.RoleTask> roleTasks = MyDBManager.GetInstance().GetAllLeaderActorInProgressTasks();
        if (roleTasks.Count > 0)
        {
            topGO.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            GameObject cellPrefab = Resources.Load<GameObject>("Prefab/TaskUICell");
            foreach (MyDBManager.RoleTask item in roleTasks)
            {
                GameObject cellGameObject = GameObject.Instantiate(cellPrefab);
                cellGameObject.GetComponent<Text>().text = item.remark;
                cellGameObject.transform.SetParent(parentGO.transform);
            }
        }
        else
        {
            topGO.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public static void ShowTipsUI(string content)
    {
        TipsUIScript tmp = GameObject.Find("Panel_Tips").GetComponent<TipsUIScript>();
        tmp.AddTipsQueue(content);
        tmp.ShowTips();
    }

}
