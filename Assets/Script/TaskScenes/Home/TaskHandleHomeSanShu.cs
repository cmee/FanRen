using System.Collections;
using UnityEngine;

public class TaskHandleHomeSanShu : ITaskHandle
{

    public const int ROLE_ID = 6;

    public override Queue TriggerTaskTalkData(int taskId)
    {
        //接任务 【和韩父告别】【和韩母告别】
        //MyDBManager.GetInstance().ConnDB();
        //MyDBManager.GetInstance().AddRoleTask(5);
        //MyDBManager.GetInstance().AddRoleTask(4);
        //UIUtil.NotifyTaskUIDatasetChanged();


        Queue allTalkContent = new Queue();
        TalkContentItemModel talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanLi",
            dfName = "韩立",
            dfTalkContent = "三叔好"
        };
        allTalkContent.Enqueue(talkContentItemModel);
        talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "sanShu",
            dfName = "三叔",
            dfTalkContent = "韩立，长大了啊"
        };
        allTalkContent.Enqueue(talkContentItemModel);

        //todo ......

        return allTalkContent;
    }

    public override Queue InProgressTaskTalkData(int taskId)
    {
        return TriggerTaskTalkData(taskId);
    }

    public override Queue SubmitTaskTalkData(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override Queue GeneralTalkData()
    {
        Queue allTalkContent = new Queue();
        TalkContentItemModel talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "sanShu",
            dfName = "三叔",
            dfTalkContent = "韩立，长大了啊"
        };
        allTalkContent.Enqueue(talkContentItemModel);
        return allTalkContent;
    }

    public override bool IsTriggerable(int taskId)
    {
        MyDBManager.GetInstance().ConnDB();
        if(taskId == 3) //和三叔到青牛镇任务
        {
            return MyDBManager.GetInstance().GetRoleTask(1).taskState == (int)FRTaskState.Finished; //收集干柴任务完成可触发
        }
        else
        {
            Debug.LogError("逻辑错误 TaskHandleHomeSanShu IsTriggerable taskId " + taskId);
            return false;
        }
    }

    public override bool IsSubmitable(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override void OnSubmitTaskComplete(int taskId)
    {

    }

    public override void OnTriggerTask(int taskId)
    {
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.GetInstance().AddRoleTask(4);
        MyDBManager.GetInstance().AddRoleTask(5);
        UIUtil.NotifyTaskUIDatasetChanged();
    }

}
