using UnityEngine.EventSystems;
using UnityEngine;

public class BaseMono : MonoBehaviour
{

    public GameObjectType gameObjectType;

    // Start is called before the first frame update
    

    // Update is called once per frame
    //protected virtual void Update()
    //{       
    //    //Debug.Log("Update1");
    //    //if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
    //    //{
    //    //    Debug.Log("一直按着UI");
    //    //    return;
    //    //}        
    //    if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject())
    //    {
    //        Debug.Log("点击了UI");
    //        return;
    //    }
    //    //Debug.Log("Update2");
    //}    

    protected bool IsClickUpOnUI()
    {
        return Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject();
    }

    protected bool IsPointerOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}

public enum GameObjectType
{
    Role = 1,
    Building = 2,
    Other = 3
}
