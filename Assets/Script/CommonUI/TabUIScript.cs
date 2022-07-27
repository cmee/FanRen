using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUIScript : MonoBehaviour
{

    public GameObject[] tabContainers;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tabRenwuGO = GameObject.Find("tabRenwu");
        Button tabRenwu = tabRenwuGO.GetComponent<Button>();
        tabRenwu.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTabButtonClick(GameObject gameObject)
    {
        Debug.Log("OnTabButtonClick " + gameObject);
        foreach (GameObject item in tabContainers)
        {
            if(item == gameObject)
            {
                Debug.Log("click button " + item);
                item.SetActive(true);
            }
            else
            {
                Debug.Log("no click button " + item);
                item.SetActive(false);
            }
        }
        switch (gameObject.name)
        {
            case "renwu": break;
            case "chuwudai": break;
            case "shentong": break;
            case "gongfa": break;
            case "fabao": break;
            case "lingshou": break;
            case "lingchong": break;
            case "kuilei": break;
            case "shenwaihuashen": break;
        }
    }

}
