using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Transform player;

    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 cp = player.transform.position - player.forward*6;
        cp.y += 5; 
        this.gameObject.transform.position = cp;

        //this.gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z);
        //this.gameObject.transform.Translate(player.forward * 7);
        this.gameObject.transform.LookAt(player);

        dir = player.transform.position - transform.position;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    //摄像头放大缩小速度
    float speed = 6f;

    // Update is called once per frame
    //void Update()
    //{

    //    if (Input.GetKeyUp(KeyCode.LeftAlt))
    //    {
    //        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    //    }

    //    transform.position = player.transform.position - dir;

    //    if(Input.GetAxis("Mouse ScrollWheel") > 0)
    //    {         
    //        transform.Translate(dir/ speed, Space.World);
    //        dir = player.transform.position - transform.position;
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0)
    //    {
    //        transform.Translate(-dir/ speed, Space.World);
    //        dir = player.transform.position - transform.position;
    //    }

    //    if (Input.GetAxis("Mouse X") != 0f)
    //    {            
    //        float mouseX = Input.GetAxis("Mouse X");
    //        transform.RotateAround(player.position, player.up, mouseX * 400 * Time.deltaTime);
    //        dir = player.transform.position - transform.position;
    //    }

    //    if (Input.GetAxis("Mouse Y") != 0f)
    //    {            
    //        float mouseY = Input.GetAxis("Mouse Y");
    //        transform.Rotate(transform.right, -mouseY * 400 * Time.deltaTime, Space.World);
    //    }
    //}

    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }

        transform.position = player.transform.position - dir;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(dir / speed, Space.World);
            dir = player.transform.position - transform.position;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(-dir / speed, Space.World);
            dir = player.transform.position - transform.position;
        }

        if (Input.GetAxis("Mouse X") != 0f)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(player.position, player.up, mouseX * 400 * Time.deltaTime);
            dir = player.transform.position - transform.position;
        }

        if (Input.GetAxis("Mouse Y") != 0f)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(transform.right, -mouseY * 400 * Time.deltaTime, Space.World);
        }
    }

}
