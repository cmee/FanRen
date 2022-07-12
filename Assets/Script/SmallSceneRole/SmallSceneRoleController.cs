using cakeslice;
using System.Collections;
using UnityEngine;

public class SmallSceneRoleController : MonoBehaviour
{

    //对话逻辑设计：首先数据源都在数据库，
    //每个NPC都有属于自己的故事线，每条故事线都有N个step，
    //在每个step,NPC会做出不同动作（说话或者给物品等等）
    //有个index标记当前处于哪个step

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

        //Debug.Log("here is " + gameObject.name);

        //TalkContentItemModel talkContentItemModel;

        //defaultName = gameObject.name;
        //switch (gameObject.name)
        //{
        //    case "小妹":
        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "xiaoMei",
        //            dfName = gameObject.name,
        //            dfTalkContent = "立哥哥，我想吃红浆果！"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);
        //        break;

        //    case "三叔":
        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "sanShu",
        //            dfName = gameObject.name,
        //            dfTalkContent = "小韩立现在懂事了"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);
        //        break;

        //    case "韩父":
        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "hanFu",
        //            dfName = "韩父",
        //            dfTalkContent = "韩立，去后山捡些干柴回来"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);

        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "hanLi",
        //            dfName = "韩立",
        //            dfTalkContent = "好，我这就去"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);

        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "hanFu",
        //            dfName = "韩父",
        //            dfTalkContent = "早点回来，别乱跑。"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);
        //        break;

        //    case "韩母":
        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "hanMu",
        //            dfName = gameObject.name,
        //            dfTalkContent = "快没米了..."
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);
        //        break;

        //    case "韩铸":
        //        talkContentItemModel = new TalkContentItemModel
        //        {
        //            dfAvatar = "hanZhu",
        //            dfName = gameObject.name,
        //            dfTalkContent = "呼呼..呼呼..【熟睡中...】"
        //        };
        //        allTalkContent.Enqueue(talkContentItemModel);
        //        break;
        //}

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(this.gameObject.name + " SmallSceneRoleController OnCollisionEnter() " + collision.gameObject.name);
        if (collision != null && collision.gameObject.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = true;
            //Debug.Log(this.gameObject.name + ": 韩立过来了, " + allTalkContent.Count);
            talkButtonController.ShowTalkButton(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log(this.gameObject.name + " SmallSceneRoleController OnCollisionExit() " + collision.gameObject.name);
        if (collision != null && collision.gameObject.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = false;
            //Debug.Log(this.gameObject.name + ": 韩立离开了");
            talkButtonController.HideTalkButton();
        }
    }

}


