using UnityEngine;

public class MapContent : BaseMono
{

    public void OnTianDaoMengClick()
    {
        Debug.Log("OnTianDaoMengClick");
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Vector3 newS = transform.localScale + Vector3.one * 0.1f;
            if (newS.x > 2.2f)
            {
                return;
            }
            else
            {
                ZoomRect(newS);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Vector3 newS = transform.localScale - Vector3.one * 0.1f;
            if (newS.x < 0.5f)
            {
                return;
            }
            else
            {
                ZoomRect(newS);
            }
        }
    }

    private void ZoomRect(Vector3 newScale)
    {
        //屏幕左下角指向屏幕中心
        Vector2 worldToScreenPivotV2 = new Vector2(Screen.width / 2, Screen.height / 2);
        //屏幕左下角指向gameObject的圆心
        Vector2 worldToGameObjectPivotV2 = new Vector2(transform.position.x, transform.position.y);
        //拖动屏幕的偏移量(离开屏幕中心的距离)
        Vector2 dragOffset = worldToGameObjectPivotV2 - worldToScreenPivotV2;

        Debug.Log("transform.position.x : " + transform.position.x + " transform.position.y : " + transform.position.y);

        float width = GetComponent<RectTransform>().rect.width * transform.localScale.x;
        float height = GetComponent<RectTransform>().rect.height * transform.localScale.y;

        //Debug.Log("dragOffset pivot: " + dragOffset.x / width + "," + dragOffset.y / height);

        //Debug.Log("width " + width + ", height " + height);

        float dopx = dragOffset.x / width;
        float dopy = dragOffset.y / height;

        //Debug.Log("dopx " + dopx + ", dopy " + dopy);

        //圆心反向移动
        GetComponent<RectTransform>().pivot -= new Vector2(dopx, dopy);

        transform.position -= new Vector3(dragOffset.x, dragOffset.y, 0);

        transform.localScale = newScale;
    }

}
