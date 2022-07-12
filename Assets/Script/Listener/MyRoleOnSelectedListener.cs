using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class MyRoleOnSelectedListener : BaseRole.RoleOnSelectedListener
//{
//    private WeakReference<BattleUIControl> battleUIControl;    

//    public MyRoleOnSelectedListener(BattleUIControl battleUIControl)
//    {
//        this.battleUIControl = new WeakReference<BattleUIControl>(battleUIControl);
//    }    

//    public void OnSelected()
//    {
//        BattleUIControl target = null;
//        battleUIControl.TryGetTarget(out target);
//        if (target != null)
//        {
//            target.ShowAndHideShentongButton();
//        }
//        else
//        {
//            Debug.LogError("MyRoleOnSelectedListener OnSelected(): target is null");
//        }
//    }
//}
