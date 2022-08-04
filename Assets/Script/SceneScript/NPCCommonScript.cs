using cakeslice;
using UnityEngine;

public class NPCCommonScript : BaseMono, IColliderWithCC
{

    Outline outline;
    TalkButtonController talkButtonController;

    public int roleId;

    void Start()
    {
        outline = GetComponentInChildren<Outline>();
        if (outline == null) Debug.LogError("GetComponentInChildren<Outline>() is null");
        if(outline != null) outline.enabled = false;
        talkButtonController = GameObject.FindGameObjectWithTag("DoTalkButton").GetComponent<TalkButtonController>();
    }

    public void OnPlayerCollisionEnter(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = true;
            Debug.Log(this.gameObject.name + ": 韩立过来了");
            talkButtonController.ShowTalkButton(this.roleId);
        }
    }

    public void OnPlayerCollisionExit(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            if (outline != null && outline.enabled) outline.enabled = false;
            Debug.Log(this.gameObject.name + ": 韩立离开了");
            talkButtonController.HideTalkButton();
        }
    }

}


