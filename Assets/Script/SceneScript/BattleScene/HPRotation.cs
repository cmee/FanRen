using UnityEngine;

public class HPRotation : BaseMono
{

    public GameObject target;
    RectTransform rt;
    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 cv = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        //transform.LookAt(cv);
        if(target != null && target.activeSelf && target.activeInHierarchy)
        {
            //if (targetPosition != target.transform.position)
            //{
                //Debug.Log("更新血条位置");
                targetPosition = target.transform.position;
                //Vector2 tp2 = Camera.main.WorldToScreenPoint(targetPosition);
                Vector2 tp2 = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
                rt.position = tp2;
            //}
            
        }
        
    }
}
