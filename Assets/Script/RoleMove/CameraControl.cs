using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private GameObject player;

    //镜头指向人物位置（含垂直偏移量）
    private Vector3 dir;

    public bool isVerticalRotateAroundSelf = false;

    public float initCameraHeight = 6;
    public float lookAtTargetHeightOffset = 2;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");

        //人物初始位置的背后位置
        Vector3 cp = player.transform.position - player.transform.forward * initCameraHeight;
        cp.y += initCameraHeight; 

        //设置镜头初始位置
        this.gameObject.transform.position = cp;

        Vector3 playerPosition = player.transform.position;
        playerPosition += Vector3.up * lookAtTargetHeightOffset;
        this.gameObject.transform.LookAt(playerPosition);

        
        CalDir();
        //transform.Translate(dir.normalized * speed, Space.World);
        //CalDir();
    }

    private void CalDir()
    {
        dir = player.transform.position + Vector3.up * lookAtTargetHeightOffset - transform.position;
    }

    //摄像头放大缩小速度
    float speed = 1f;

    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }

        transform.position = player.transform.position + Vector3.up * lookAtTargetHeightOffset - dir;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Vector3.Distance(player.transform.position + Vector3.up * lookAtTargetHeightOffset, transform.position) > 3)
            {
                transform.Translate(dir.normalized * speed, Space.World);
                CalDir();
            }
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Vector3.Distance(player.transform.position + Vector3.up * lookAtTargetHeightOffset, transform.position) < 10)
            {
                transform.Translate(-dir.normalized * speed, Space.World);
                CalDir();
            }
        }

        if (Input.GetAxis("Mouse X") != 0f)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(player.transform.position + Vector3.up * lookAtTargetHeightOffset, player.transform.up, mouseX * 400 * Time.deltaTime);
            CalDir();
        }

        if (Input.GetAxis("Mouse Y") != 0f)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            if (isVerticalRotateAroundSelf)
            {
                transform.Rotate(transform.right, -mouseY * 400 * Time.deltaTime, Space.World);
            }
            else
            {
                if(mouseY > 0)
                {
                    if (transform.rotation.eulerAngles.x > 20)
                    {
                        transform.RotateAround(player.transform.position + Vector3.up * lookAtTargetHeightOffset, transform.right, -mouseY * 400 * Time.deltaTime);
                    }
                }
                else if(mouseY < 0)
                {
                    if (transform.rotation.eulerAngles.x < 70)
                    {
                        transform.RotateAround(player.transform.position + Vector3.up * lookAtTargetHeightOffset, transform.right, -mouseY * 400 * Time.deltaTime);
                    }
                }
                CalDir();
            }
        }
    }

}
