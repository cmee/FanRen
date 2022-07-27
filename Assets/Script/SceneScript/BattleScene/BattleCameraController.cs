using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraController : BaseMono
{
    private Transform player;
    private Vector3 dir;
    private bool stopUpdateFlag = false;

    // Start is called before the first frame update

    private GameObject selectedRole;

    //Vector3 roleHeadPointToCamera = Vector3.zero;

    //GameObject terrainGO;
    private TerrainCollider terrainCollider;

    void Start()
    {
        terrainCollider = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainCollider>();
    }

    private Vector3 CalcScreenCenterPosOnTerrain()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0));
        //public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance);
        RaycastHit hitInfo;
        if (terrainCollider.Raycast(ray, out hitInfo, 1000))
        {
            return ray.GetPoint(hitInfo.distance);
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void CameraFocusAt(Transform target)
    {
        Vector3 cp = CalcScreenCenterPosOnTerrain();
        Vector3 tp = target.position;

        Hashtable args = new Hashtable();
        //lookahead
        //args.Add("lookahead", 0.9f);
        //args.Add("path", path.ToArray());
        //args.Add("looktarget", selectedRole.transform);
        args.Add("looptype", iTween.LoopType.none);
        args.Add("easeType", iTween.EaseType.easeOutQuint);
        args.Add("time", 1f);
        //args.Add("speed", 7);
        //args.Add("orienttopath", true);
        //Debug.Log("roleHeadPointToCamera y " + roleHeadPointToCamera);
        args.Add("position", Camera.main.transform.position + (tp - cp));
        args.Add("oncomplete", "OnComplete");
        args.Add("onCompleteTarget", this.gameObject);
        //looktarget
        iTween.MoveTo(this.gameObject, args);
    }

    public void SetSelectedRole(GameObject selectedRole)
    {
        this.selectedRole = selectedRole;
        stopUpdateFlag = true;
        CameraFocusAt(selectedRole.transform);
    }

    private void OnComplete()
    {
        player = this.selectedRole.transform;
        dir = player.transform.position - transform.position;
        stopUpdateFlag = false;
    }

    void LateUpdate()
    {

        //if (Input.GetKeyUp(KeyCode.LeftAlt))
        //{
        //    Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        //}

        if (player == null || dir == null || stopUpdateFlag) return;

        transform.position = player.transform.position - dir;

        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") != 0f)
            {
                float mouseX = Input.GetAxis("Mouse X");
                transform.RotateAround(player.position, player.up, mouseX * 600 * Time.deltaTime);
                dir = player.transform.position - transform.position;
            }

            if (Input.GetAxis("Mouse Y") != 0f)
            {
                float mouseY = Input.GetAxis("Mouse Y");
                transform.RotateAround(player.position, transform.right, -mouseY * 600 * Time.deltaTime);
                dir = player.transform.position - transform.position;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(dir / 6f, Space.World);
            dir = player.transform.position - transform.position;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(-dir / 6f, Space.World);
            dir = player.transform.position - transform.position;
        }

    }

}
