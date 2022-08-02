using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIControl : BaseMono
{

    private BaseRole selectedRoleCS;

    private GameObject passButton;
    private GameObject resetButton;
    private GameObject[] buttons = new GameObject[12];

    private List<SlideAvatarController> allSlideAvatarCS = new List<SlideAvatarController>();

    public void Init(GameObject[] allRole)
    {
        passButton = GameObject.FindGameObjectWithTag("PassButton");
        resetButton = GameObject.FindGameObjectWithTag("ResetButton");
        passButton.SetActive(false);
        resetButton.SetActive(false);

        for (int i = 0; i < 12; i++)
        {                        
            buttons[i] = GameObject.Find("st" + i);
            buttons[i].SetActive(false);
            //ColorBlock cb = buttons[i].GetComponent<Button>().colors;
            //cb.selectedColor = Color.red; 修改无效，很奇怪
        }

        
        GameObject sliderAvatarPrefab = Resources.Load<GameObject>("Prefab/SliderAvatar");
        GameObject avatarParent = GameObject.FindGameObjectWithTag("SliderForAction");
        float parentWidth = avatarParent.GetComponent<RectTransform>().rect.width;
        float parentHeight = avatarParent.GetComponent<RectTransform>().rect.height;

        foreach (GameObject roleGO in allRole)
        {
            //头像移动代码
            BaseRole roleCS = roleGO.GetComponent<BaseRole>();
            GameObject sliderAvatarGO = Instantiate(sliderAvatarPrefab);
            SlideAvatarController sac = sliderAvatarGO.GetComponent<SlideAvatarController>();
            sliderAvatarGO.transform.SetParent(avatarParent.transform);
            //todo 头像滑动速度公式待定
            sac.speed = roleCS.speed / 10f;
            if (roleCS.teamNum == TeamNum.TEAM_ONE)
            {
                sliderAvatarGO.GetComponent<Image>().color = Color.blue;
                sliderAvatarGO.transform.position = new Vector2(avatarParent.transform.position.x - parentWidth / 2, avatarParent.transform.position.y - parentHeight / 2);
            }
            else
            {
                sliderAvatarGO.GetComponent<Image>().color = Color.red;
                sliderAvatarGO.transform.position = new Vector2(avatarParent.transform.position.x - parentWidth / 2, avatarParent.transform.position.y + parentHeight / 2);
            }
            sac.roleGO = roleGO;
            allSlideAvatarCS.Add(sac);


            //初始化血条代码
            GameObject hpSliderPrefab = Resources.Load<GameObject>("Prefab/HP_Slider");

            GameObject hpSlideGameObject = Instantiate(hpSliderPrefab, this.transform);
            hpSlideGameObject.name = roleCS.GetHpUIGameObjectName();

            Text roleName = hpSlideGameObject.GetComponentsInChildren<Text>()[0];
            roleName.text = roleCS.roleName;

            HPRotation hpRotation = hpSlideGameObject.GetComponent<HPRotation>();
            hpRotation.target = roleGO;

            Slider slide = hpSlideGameObject.GetComponent<Slider>();
            slide.maxValue = roleCS.maxHp;
            slide.minValue = 0;
            slide.value = roleCS.hp;

            roleCS.hpGO = hpSlideGameObject;
            roleCS.sliderAvatarGO = sliderAvatarGO;

        }




    }     

    public void OnRoleSelected(BaseRole selectedRoleCS)
    {
        this.selectedRoleCS = selectedRoleCS;
        ShowAndHideShentongButton();
        passButton.SetActive(true);
        resetButton.SetActive(true);
    }

    public void OnChangeRoleAction(GameObject roleGO)
    {
        foreach (SlideAvatarController sac in allSlideAvatarCS)
        {
            sac.PauseRun();
        }
    }

    public void OnClickPassButton()
    {
        HideAllShentongButton();
        passButton.SetActive(false);
        resetButton.SetActive(false);
        foreach (SlideAvatarController sac in allSlideAvatarCS)
        {
            sac.RePlayRun();
        }
    }

    private void HideAllShentongButton()
    {
        if(buttons != null && buttons.Length > 0)
        {
            foreach (GameObject bt in buttons)
            {
                bt.SetActive(false);
            }
        }
    }

    public void ShowAndHideShentongButton()
    {
        if (selectedRoleCS == null || selectedRoleCS.shentongInBattle == null)
        {
            Debug.LogError(selectedRoleCS.name + "shentongInBattle not inited or selectedRoleCS is null");
            return;
        }        
        for (int i=0; i<12; i++)
        {            
            GameObject shentongButtonGO = buttons[i];
            if (selectedRoleCS.shentongInBattle[i] != null)
            {
                buttons[i] = shentongButtonGO;
                shentongButtonGO.SetActive(true);
                Text buttonText = shentongButtonGO.GetComponentInChildren<Text>();
                buttonText.text = selectedRoleCS.shentongInBattle[i].shenTongName;
            }
            else
            {
                buttons[i] = shentongButtonGO;
                shentongButtonGO.SetActive(false);
            }
        }
    }

    private void changeButtonColor(int clickButtonIndex)
    {
        //for(int i=0; i<12; i++)
        //{
        //    if(i== clickButtonIndex)
        //    {
        //        ColorBlock cb = buttons[i].GetComponent<Button>().colors;
        //        cb.selectedColor = Color.red;
        //    }
        //    else
        //    {
        //        ColorBlock cb = buttons[i].GetComponent<Button>().colors;
        //        cb.selectedColor = Color.clear;
        //    }
        //}
    }

    public void OnClickShentong0()
    {
        selectedRoleCS.OnSelectShentong(0);
        changeButtonColor(0);
    }

    public void OnClickShentong1()
    {
        selectedRoleCS.OnSelectShentong(1);
        changeButtonColor(1);
    }

    public void OnClickShentong2()
    {
        selectedRoleCS.OnSelectShentong(2);
        changeButtonColor(2);
    }

    public void OnClickShentong3()
    {
        selectedRoleCS.OnSelectShentong(3);
        changeButtonColor(3);
    }

    public void OnClickShentong4()
    {
        selectedRoleCS.OnSelectShentong(4);
        changeButtonColor(4);
    }

    public void OnClickShentong5()
    {
        selectedRoleCS.OnSelectShentong(5);
        changeButtonColor(5);
    }

    public void OnClickShentong6()
    {
        selectedRoleCS.OnSelectShentong(6);
        changeButtonColor(6);
    }

    public void OnClickShentong7()
    {
        selectedRoleCS.OnSelectShentong(7);
        changeButtonColor(7);
    }

    public void OnClickShentong8()
    {
        selectedRoleCS.OnSelectShentong(8);
        changeButtonColor(8);
    }

    public void OnClickShentong9()
    {
        selectedRoleCS.OnSelectShentong(9);
        changeButtonColor(9);
    }

    public void OnClickShentong10()
    {
        selectedRoleCS.OnSelectShentong(10);
        changeButtonColor(10);
    }

    public void OnClickShentong11()
    {
        selectedRoleCS.OnSelectShentong(11);
        changeButtonColor(11);
    }

    

}


