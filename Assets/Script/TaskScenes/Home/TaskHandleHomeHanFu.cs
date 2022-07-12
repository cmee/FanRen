using System.Collections;
using UnityEngine;

public class TaskHandleHomeHanFu : ITaskHandle
{

    public const int ROLE_ID = 3;

    public override Queue TriggerTaskTalkData(int taskId)
    {
        Queue allTalkContent = new Queue();
        TalkContentItemModel talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanFu",
            dfName = "韩父",
            dfTalkContent = "韩立，去后山捡些干柴回来"
        };
        allTalkContent.Enqueue(talkContentItemModel);

        talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanLi",
            dfName = "韩立",
            dfTalkContent = "好，我这就去"
        };
        allTalkContent.Enqueue(talkContentItemModel);

        talkContentItemModel = new TalkContentItemModel
        {
            dfAvatar = "hanFu",
            dfName = "韩父",
            dfTalkContent = "早点回来，别乱跑。"
        };
        allTalkContent.Enqueue(talkContentItemModel);
        return allTalkContent;
    }

    public override Queue InProgressTaskTalkData(int taskId)  //A NPC触发，B NPC提交的情况
    {
        if(taskId == 1) //干柴任务，触发、提交同人
        {
            return TriggerTaskTalkData(taskId);
        }
        else if (taskId == 5) //告别任务，触发、提交不同人
        {
            Queue allTalkContent = new Queue();
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanFu",
                dfName = "韩父",
                dfTalkContent = "韩立，你三叔来看你了，快叫人"
            };
            allTalkContent.Enqueue(talkContentItemModel);
            return allTalkContent;
        }
        Debug.LogError("逻辑错误 TaskHandleHomeHanFu InProgressTaskTalkData taskId " + taskId);
        return null;
        //return TriggerTaskTalkData(taskId);
    }

    public override Queue SubmitTaskTalkData(int taskId)
    {
        Queue allTalkContent = new Queue();
        if (taskId == 1) //提交收集干柴任务
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanFu",
                dfName = "韩父",
                dfTalkContent = "韩立，你三叔来看你了，快叫人"
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }
        else if(taskId == 5) //提交告别任务
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanFu",
                dfName = "韩父",
                dfTalkContent = "韩立，在外面要老实，遇事多忍让，别和其他人起争执"
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }
        return allTalkContent;
    }

    public override Queue GeneralTalkData()
    {
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.RoleTask roleTask = MyDBManager.GetInstance().GetRoleTask(1);
        MyDBManager.RoleTask roleTask2 = MyDBManager.GetInstance().GetRoleTask(5);

        Queue allTalkContent = new Queue();
        if (roleTask.taskState == (int)FRTaskState.Untrigger) //未接任务
        {
            Debug.LogError("逻辑错误 TaskHandleHomeHanFu GeneralTalkData");
        }
        else if (roleTask.taskState == (int)FRTaskState.Finished && roleTask2.taskState == (int)FRTaskState.Untrigger)
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanFu",
                dfName = "韩父",
                dfTalkContent = "韩立，你三叔来看你了，快叫人"
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }
        else if (roleTask2.taskState == (int)FRTaskState.Finished)
        {
            TalkContentItemModel talkContentItemModel = new TalkContentItemModel
            {
                dfAvatar = "hanFu",
                dfName = "韩父",
                dfTalkContent = "韩立，在外面要老实，遇事多忍让，别和其他人起争执"
            };
            allTalkContent.Enqueue(talkContentItemModel);
        }

        return allTalkContent;
    }

    

    public bool IsCanSubmitTask()
    {
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.RoleItem roleItem = MyDBManager.GetInstance().GetRoleItem(1); //1是干柴
        return roleItem.itemCount >= 5;
    }

    public override bool IsTriggerable(int taskId)
    {
        MyDBManager.GetInstance().ConnDB();
        return MyDBManager.GetInstance().GetRoleTask(taskId).taskState == (int)FRTaskState.Untrigger;
    }

    public override bool IsSubmitable(int taskId)
    {
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.RoleItem roleItem = MyDBManager.GetInstance().GetRoleItem(1); //1是干柴
        return roleItem.itemCount >= 5;
    }

    public override void OnSubmitTaskComplete(int taskId)
    {
        Debug.Log("干柴>=5，心境+1");
        MyDBManager.GetInstance().ConnDB();
        MyDBManager.RoleItem roleItem = MyDBManager.GetInstance().GetRoleItem(1);
        MyDBManager.GetInstance().DeleteItemInBag(1, 5, roleItem.itemCount);
    }

    public override void OnTriggerTask(int taskId)
    {
        Debug.Log("OnTriggerTask taskId " + taskId);
    }

}
