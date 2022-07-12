using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUIRootScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject tabUIPanelGameObj;
    //public GameObject tabUICamera;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("tab click");
            this.tabUIPanelGameObj.SetActive(!this.tabUIPanelGameObj.activeInHierarchy);
            //this.tabUICamera.SetActive(this.tabUIPanelGameObj.activeInHierarchy);
        }
    }
}
