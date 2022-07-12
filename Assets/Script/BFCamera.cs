using UnityEngine;

public class BFCamera : BaseMono
{

    private GameObject terrainGO;

    // Start is called before the first frame update
    void Start()
    {
        terrainGO = GameObject.FindWithTag("Terrain");
        Terrain terrain = terrainGO.GetComponent<Terrain>();
        terrainWidthUnit = terrain.terrainData.bounds.size.x;
        terrainHeightUnit = terrain.terrainData.bounds.size.z;
        //Debug.Log(terrain.terrainData.bounds.size.x);
    }

    float rSpeed = 200f;
    float mSpeed = 10f;
    float terrainWidthUnit;
    float terrainHeightUnit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.fieldOfView -= 4f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.fieldOfView += 4f;
        }
        else if (Input.GetMouseButton(0) && !base.IsPointerOnUI())
        {
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * mSpeed;//左右移动
            float z = Input.GetAxis("Mouse Y") * Time.deltaTime * mSpeed;//前后移动

            //Debug.Log("mouse x " + x);

            //战场中点(15,0,15)指向(相机对地面的垂直投影)
            Vector3 terrainToCam = new Vector3(transform.position.x - terrainWidthUnit / 2, 0, transform.position.z - terrainHeightUnit / 2);

            //前后移动相机用世界坐标，因为旋转后直接移动会不正确，所以x,z方向要分别用不同坐标系
            transform.Translate(terrainToCam * z, Space.World);

            //横向移动相机，用自己的坐标系，怎么旋转都不影响
            transform.Translate(-x * terrainToCam.magnitude, 0, 0, Space.Self);

        }
        else if (Input.GetMouseButton(1) && !base.IsPointerOnUI())
        {
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * rSpeed;//鼠标左右移动
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * rSpeed;//鼠标前后移动
            transform.RotateAround(Vector3.one * 15f, Vector3.up, x * 5);
            transform.RotateAround(Vector3.one * 15f, transform.right, -y * 5);
        }



    }

    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
        //（_dir.x,_dir.z）与（？，1）垂直，则_dir.x * ？ + _dir.z * 1 = 0
        if (_dir.z == 0)
        {
            return new Vector3(0, 0, -1);
        }
        else if (_dir.x == 0)
        {
            return new Vector3(-1, 0, 0);
        }
        else if (_dir.y == 0)
        {
            return new Vector3(0, -1, 0);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }

}
