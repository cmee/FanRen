using cakeslice;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WuPinScript : MonoBehaviour
{

    Outline outline;
    CatchWupinButtonScript mCatchWupinButtonScript;

    public string wuPinName;
    public string wuPinCount = "1";

    //0表示显示还没有拾取过，1表示已经拾取了
    //取名规则：场景path + gameObject.name
    [HideInInspector]
    public string uniquePrefenceKey;

    public int itemId;
    public FRItemType itemType;

    private void Awake()
    {
        uniquePrefenceKey = SceneUtility.GetScenePathByBuildIndex(this.gameObject.scene.buildIndex) + "_" + gameObject.name;
    }

    // Start is called before the first frame update
    void Start()
    {

        if(PlayerPrefs.GetInt(uniquePrefenceKey, 0) == 0)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            return;
        }

        outline = GetComponentInChildren<Outline>();
        outline.enabled = false;

        mCatchWupinButtonScript = GameObject.Find("CatchWupingCanvas").GetComponent<CatchWupinButtonScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(this.gameObject.name + " SmallSceneRoleController OnCollisionEnter() " + collision.gameObject.name);
        if (collision != null && collision.gameObject.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = true;
            //Debug.Log(this.gameObject.name + ": 韩立过来了");
            //talkButtonController.ShowTalkButton(allTalkContent.Clone() as Queue);

            //显示拾取按钮
            mCatchWupinButtonScript.ShowCatchButton(gameObject);

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log(this.gameObject.name + " SmallSceneRoleController OnCollisionExit() " + collision.gameObject.name);
        if (collision != null && collision.gameObject.tag.Equals("Player"))
        {
            if (outline != null) outline.enabled = false;
            //Debug.Log(this.gameObject.name + ": 韩立离开了");
            //talkButtonController.HideTalkButton();

            //隐藏拾取按钮
            mCatchWupinButtonScript.HideCatchButton(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
