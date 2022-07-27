using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideAvatarController : BaseMono
{
    // Start is called before the first frame update

    private GameObject sliderForActionGO;
    private float w;
    private Vector2 originPos;
    private BattleController battleController;
    private BattleUIControl battleUIControlCS;

    public GameObject roleGO;
    public float speed = 0f; //¶ÝËÙ

    private bool runningFlag = false;
    private bool reachedFlag = false;

    void Start()
    {
        sliderForActionGO = GameObject.FindGameObjectWithTag("SliderForAction");
        w = sliderForActionGO.GetComponent<RectTransform>().rect.width;
        originPos = transform.position;
        battleController = GameObject.FindGameObjectWithTag("Terrain").GetComponent<BattleController>();
        battleUIControlCS = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>();
    }

    public void PauseRun()
    {
        this.runningFlag = true;
    }

    public void RePlayRun()
    {
        this.runningFlag = false;
        if (reachedFlag)
        {            
            transform.position = originPos;
            reachedFlag = false;
        }
    }

    void Update()
    {
        if (runningFlag) return;
        if (transform.position.x <= (sliderForActionGO.transform.position.x + w / 2))
        {            
            transform.Translate(Vector2.right * Time.deltaTime * 500 * speed, Space.Self);
        }
        else
        {
            reachedFlag = true;
            runningFlag = true;
            battleController.OnChangeRoleAction(roleGO);
            battleUIControlCS.OnChangeRoleAction(roleGO);
        }
    }

}
