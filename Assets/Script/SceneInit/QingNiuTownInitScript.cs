using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QingNiuTownInitScript : MonoBehaviour
{

    public GameObject horseCar;
    public GameObject[] wayPaths;
    public GameObject carBGM;
    //public GameObject carStopSound;

    // Start is called before the first frame update
    void Start()
    {
    }


    int nextIndex = 0;
    bool flag = true;

    // Update is called once per frame
    void Update()
    {
        //horseCar.transform.LookAt(wayPaths[0].transform);
        //horseCar.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);

        if (nextIndex > wayPaths.Length - 1)
        {
            //Destroy(this.gameObject);
            
            if (flag)
            {
                flag = false;
                MyAudioManager.GetInstance().PlaySE("SoundEff/horse_stop2");
                carBGM.SetActive(false);
            }
            return;
        }
        //horseCar.transform.LookAt(wayPaths[nextIndex].transform);

        Vector3 aa = horseCar.transform.forward;
        aa.y = 0f;

        Vector3 bb = (wayPaths[nextIndex].transform.position - horseCar.transform.position).normalized;
        bb.y = 0f;

        Vector3 a = Vector3.RotateTowards(aa, bb, 0.7f * Time.deltaTime, 0f);
        horseCar.transform.rotation = Quaternion.LookRotation(a);

        

        horseCar.transform.Translate(bb * Time.deltaTime * 5f, Space.World);
        if (Vector3.Distance(horseCar.transform.position, wayPaths[nextIndex].transform.position) <= 0.2f)
        {
            nextIndex++;
        }
    }

}
