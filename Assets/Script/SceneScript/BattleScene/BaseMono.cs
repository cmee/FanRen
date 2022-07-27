using UnityEngine.EventSystems;
using UnityEngine;

public class BaseMono : MonoBehaviour
{

    public GameObjectType gameObjectType;

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
