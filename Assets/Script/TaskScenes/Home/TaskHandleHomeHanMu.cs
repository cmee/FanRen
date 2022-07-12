using System.Collections;
using UnityEngine;

public class TaskHandleHomeHanMu : ITaskHandle
{

    public const int ROLE_ID = 4;

    public override Queue TriggerTaskTalkData(int taskId)
    {
        Debug.LogError("逻辑错误，不应该有此打印 TaskHandleHomeHanMu TriggerTaskTalkData " + taskId);
        return null;
    }

    public override Queue InProgressTaskTalkData(int taskId)
    {
        return SubmitTaskTalkData(taskId);
    }

    public override Queue SubmitTaskTalkData(int taskId)
    {
        Queue allTalkContent = new Queue();
        TalkContentItemModel talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanMu",
            dfName = "韩母",
            dfTalkContent = "韩立，在外面要多注意身体，要吃好睡好"
        };
        allTalkContent.Enqueue(talkContentItemModel);
        return allTalkContent;
    }

    public override Queue GeneralTalkData()
    {
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.RoleTask roleTask = MyDBManager.GetInstance().GetRoleTask(4); //告别韩母任务
        Queue allTalkContent = new Queue();
        if (roleTask.taskState == (int)FRTaskState.Untrigger)
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanMu",
                dfName = "韩母",
                dfTalkContent = "快没米了..."
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }
        else if(roleTask.taskState == (int)FRTaskState.Finished)
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanMu",
                dfName = "韩母",
                dfTalkContent = "韩立，在外面要多注意身体，要吃好睡好"
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }
        return allTalkContent;
    }

    public override bool IsTriggerable(int taskId)
    {
        Debug.LogError("逻辑错误，不应该有此打印 TaskHandleHomeHanMu IsTriggerable " + taskId);
        return false;
    }

    public override bool IsSubmitable(int taskId)
    {
        MyDBManager.GetInstance().ConnDB();
        return MyDBManager.GetInstance().GetRoleTask(4).taskState == (int)FRTaskState.InProgress;
    }

    public override void OnSubmitTaskComplete(int taskId)
    {
        Debug.Log("心境+1");
    }

    public override void OnTriggerTask(int taskId)
    {
    }

}
