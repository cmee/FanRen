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
        GameObject catchWupin = this.wuPinGameObjs[this.wuPinGameObjs.Count - 1] as GameObject;
        WuPinScript wuPinScript = catchWupin.GetComponent<WuPinScript>();
        Debug.Log("Ê°È¡µ½ : " + wuPinScript.wuPinName);
        PlayerPrefs.SetInt(wuPinScript.uniquePrefenceKey, 1);
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.GetInstance().AddItemToBag(wuPinScript.itemId, wuPinScript.itemType, 1);
        Destroy(catchWupin);
        this.wuPinGameObjs.RemoveAt(this.wuPinGameObjs.Count - 1);
        UIUtil.ShowTipsUI(wuPinScript.wuPinName + " +" + wuPinScript.wuPinCount);
        if(this.wuPinGameObjs.Count == 0)
        {
            catchButtonGameObj.SetActive(false);
        }
    }

    ArrayList wuPinGameObjs = new ArrayList();

    public void ShowCatchButton(GameObject wuPinGameObj)
    {
        catchButtonGameObj.SetActive(true);
        wuPinGameObjs.Add(wuPinGameObj);
    }

    public void HideCatchButton(GameObject wuPinGameObj)
    {
        catchButtonGameObj.SetActive(false);
        wuPinGameObjs.Remove(wuPinGameObj);
    }

}
