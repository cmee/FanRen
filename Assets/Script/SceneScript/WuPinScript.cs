using cakeslice;
using UnityEngine;

public class WuPinScript : BaseMono, IColliderWithCC
{

    Outline outline;
    CatchWupinButtonScript mCatchWupinButtonScript;

    public string wuPinName;
    public string wuPinCount = "1";

    public int itemId;
    public FRItemType itemType;

    // Start is called before the first frame update
    void Start()
    {

        if (!ShowOrHideGameObjByUniquePrefenceKey()) return;

        outline = GetComponentInChildren<Outline>();
        outline.enabled = false;

        mCatchWupinButtonScript = GameObject.Find("CatchWupingCanvas").GetComponent<CatchWupinButtonScript>();
    }

    public void OnPlayerCollisionEnter(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = true;
            //Debug.Log(this.gameObject.name + ": 韩立过来了");
            //talkButtonController.ShowTalkButton(allTalkContent.Clone() as Queue);

            //显示拾取按钮
            mCatchWupinButtonScript.ShowCatchButton(gameObject);

        }
    }

    public void OnPlayerCollisionExit(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = false;
            //Debug.Log(this.gameObject.name + ": 韩立离开了");
            //talkButtonController.HideTalkButton();

            //隐藏拾取按钮
            mCatchWupinButtonScript.HideCatchButton(gameObject);
        }
    }

}
