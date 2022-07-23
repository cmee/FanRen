using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//剧本脚本
public class QingNiuTownInitScript : MonoBehaviour
{

    public GameObject horseCar;
    public GameObject[] wayPaths;
    public GameObject carBGM;
    //public GameObject carStopSound;
    private int nextIndex = 0;
    private bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (nextIndex > wayPaths.Length - 1)
        {
            if (flag)
            {
                flag = false;
                MyAudioManager.GetInstance().PlaySE("SoundEff/horse_stop2");
                carBGM.SetActive(false);
                Invoke("OnHorseCarStop", 1); //延迟1秒是为了等马叫声停止才下车，显得真实
            }
            return;
        }

        Vector3 horseCarForward = horseCar.transform.forward;
        horseCarForward.y = 0f;

        Vector3 horseCarTowardTargetPoint = (wayPaths[nextIndex].transform.position - horseCar.transform.position).normalized;
        horseCarTowardTargetPoint.y = 0f;

        Vector3 horseRotateToward = Vector3.RotateTowards(horseCarForward, horseCarTowardTargetPoint, 0.7f * Time.deltaTime, 0f);
        horseCar.transform.rotation = Quaternion.LookRotation(horseRotateToward);

        horseCar.transform.Translate(horseCarTowardTargetPoint * Time.deltaTime * 5f, Space.World);
        if (Vector3.Distance(horseCar.transform.position, wayPaths[nextIndex].transform.position) <= 0.2f)
        {
            nextIndex++;
        }
    }

    private void OnHorseCarStop()
    {
        //马车停止后，韩立和三叔下车和一些老朋友调侃对话
    }

}
