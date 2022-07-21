using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookScript : MonoBehaviour
{

    CinemachineVirtualCamera cinemachine;
    CinemachineFramingTransposer cft;

    // Start is called before the first frame update
    void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        cft = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    float mouseScrollWheel;

    // Update is called once per frame
    void Update()
    {
        mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel < 0)
        {
            if (cft.m_CameraDistance + 2 > 16) return;
            cft.m_CameraDistance += 2;
        }
        else if (mouseScrollWheel > 0)
        {
            if (cft.m_CameraDistance - 2 < 0) return;
            cft.m_CameraDistance -= 2;
        }
    }

}
