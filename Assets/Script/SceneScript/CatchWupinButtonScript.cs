using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchWupinButtonScript : MonoBehaviour
{

    public GameObject catchButtonGameObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (catchButtonGameObj.activeInHierarchy)
            {
                OnCatchButtonClick();
            }
        }
    }

    public void OnCatchButtonClick()
    {
        GameObject catchWupin = this.wuPinGameObjs[this.wuPinGameObjs.Count - 1] as GameObject; //同时多个道具都在可以拾取的范围内，先拾取最后一个碰到的
        WuPinScript wuPinScript = catchWupin.GetComponent<WuPinScript>();

        Debug.Log("拾取到 : " + wuPinScript.wuPinName);

        PlayerPrefs.SetInt(wuPinScript.uniquePrefenceKey, 1);  //该道具终身只能在场景中显示1次

        MyDBManager.GetInstance().ConnDB();
        MyDBManager.GetInstance().AddItemToBag(wuPinScript.itemId, wuPinScript.itemType, 1); //背包添加道具

        //Destroy(catchWupin); 用这个会报错
        catchWupin.SetActive(false);
        this.wuPinGameObjs.RemoveAt(this.wuPinGameObjs.Count - 1);

        UIUtil.ShowTipsUI(wuPinScript.wuPinName + " +" + wuPinScript.wuPinCount);

        if(this.wuPinGameObjs.Count == 0) //同时1个或者多个道具都在可以拾取的范围内，并且全部拾取完了
        {
            catchButtonGameObj.SetActive(false);
        }
    }

    ArrayList wuPinGameObjs = new ArrayList(); //同时1个或者多个道具都在可以拾取的范围内

    public void ShowCatchButton(GameObject wuPinGameObj)
    {
        catchButtonGameObj.SetActive(true);
        if (wuPinGameObjs.Contains(wuPinGameObj))
        {
            wuPinGameObjs.Remove(wuPinGameObj);
        }
        wuPinGameObjs.Add(wuPinGameObj);
    }

    public void HideCatchButton(GameObject wuPinGameObj)
    {
        catchButtonGameObj.SetActive(false);
        wuPinGameObjs.Remove(wuPinGameObj);
    }

}
