using cakeslice;
using UnityEngine;

public class NPCCommonScript : BaseMono, IColliderWithCC
{

    //Queue allTalkContent = new Queue();

    Outline outline;
    TalkButtonController talkButtonController;

    public int roleId;

    //public string defaultName = "";
    //public string defaultTalkContent = "";
    //public string defaultAvatar = "";

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponentInChildren<Outline>();
        if (outline == null) Debug.LogError("GetComponentInChildren<Outline>() is null");
        if(outline != null) outline.enabled = false;
        talkButtonController = GameObject.FindGameObjectWithTag("DoTalkButton").GetComponent<TalkButtonController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlayerCollisionEnter(GameObject player)
    {
        if (player.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = true;
            Debug.Log(this.gameObject.name + ": 韩立过来了");
            talkButtonController.ShowTalkButton(this);
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


