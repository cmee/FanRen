using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MyDBManager.GetInstance().ConnDB();
        //MyDBManager.GetInstance().AddItemToBag(1, 2);
        //MyAudioManager.GetInstance().PlayBGM("BGM/cm2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
