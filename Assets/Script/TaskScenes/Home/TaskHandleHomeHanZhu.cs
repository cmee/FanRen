using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskHandleHomeHanZhu : ITaskHandle
{

    public const int ROLE_ID = 5;
    public override Queue<TalkContentItemModel> GeneralTalkData()
    {
        Queue<TalkContentItemModel> allTalkContent = new Queue<TalkContentItemModel>();
        TalkContentItemModel talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanZhu",
            dfName = "º«Öý",
            dfTalkContent = "ZzzzZzzzZzzz"
        };
        allTalkContent.Enqueue(talkContentItemModel);
        return allTalkContent;
    }

    public override Queue<TalkContentItemModel> InProgressTaskTalkData(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsSubmitable(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsTriggerable(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override void OnSubmitTaskComplete(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerTask(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override Queue<TalkContentItemModel> SubmitTaskTalkData(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public override Queue<TalkContentItemModel> TriggerTaskTalkData(int taskId)
    {
        throw new System.NotImplementedException();
    }
}
