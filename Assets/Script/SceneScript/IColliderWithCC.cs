using UnityEngine;

//想和主角发生碰撞，必须实现这个接口
public interface IColliderWithCC
{

    public void OnPlayerCollisionEnter(GameObject player);
    public void OnPlayerCollisionExit(GameObject player);

}
