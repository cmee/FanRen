
using System.Collections;
using UnityEngine;

public abstract class ITaskHandle
{

    public abstract Queue TriggerTaskTalkData(int taskId);
    public abstract Queue InProgressTaskTalkData(int taskId);//一般和TriggerTaskTalkData相同
    public abstract Queue SubmitTaskTalkData(int taskId);

    //一般性对话，一般是无任务NPC使用
    public abstract Queue GeneralTalkData();

    public abstract bool IsTriggerable(int taskId);//任务是否可触发

    public abstract bool IsSubmitable(int taskId);//任务是否可提交

    public abstract void OnSubmitTaskComplete(int taskId); //提交任务完成回调

    public abstract void OnTriggerTask(int taskId); //触发任务时回调

    public class TaskHandleBuilder
    {
        public static ITaskHandle Build(int roleId)
        {
            switch (roleId)
            {
                case TaskHandleHomeXiaoMei.ROLE_ID: return new TaskHandleHomeXiaoMei();
                case TaskHandleHomeHanFu.ROLE_ID: return new TaskHandleHomeHanFu();
                case TaskHandleHomeHanMu.ROLE_ID: return new TaskHandleHomeHanMu();
                case TaskHandleHomeSanShu.ROLE_ID: return new TaskHandleHomeSanShu();
                case TaskHandleHomeHanZhu.ROLE_ID: return new TaskHandleHomeHanZhu();
            }

            Debug.LogError("unknow roleId : " + roleId);

            return null;
        }
    }

}

public class TalkContentItemModel
{
    public string dfAvatar;
    public string dfName;
    public string dfTalkContent;
}

