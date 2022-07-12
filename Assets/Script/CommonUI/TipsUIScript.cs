using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TipsUIScript : MonoBehaviour
{
    // Start is called before the first frame update


    private Queue tipsContentQueue = new Queue();

    public void AddTipsQueue(string content)
    {
        tipsContentQueue.Enqueue(content);
    }

    public void ShowTips()
    {
        if (tipsTextGameObj != null && tipsTextGameObj.activeInHierarchy)
        {
            Debug.Log("tips showing");
            return;
        }
        if (tipsContentQueue.Count > 0)
        {
            ShowTipsUI(tipsContentQueue.Dequeue() as string);
        }
    }

    private GameObject tipsTextGameObj;

    private void ShowTipsUI(string content)
    {

        //GameObject tipsTextPrefab = Resources.Load<GameObject>("Prefab/TipsText");
        //tipsTextGameObj = GameObject.Instantiate(tipsTextPrefab, this.transform);
        //DOTweenCB myCB = new DOTweenCB(tipsTextGameObj, this);

        tipsTextGameObj.SetActive(true);
        tipsTextGameObj.GetComponent<Text>().text = content;
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);

        RectTransform rt = this.GetComponent<RectTransform>();

        Sequence se = DOTween.Sequence();
        se.Append(rt.DOAnchorPosY(-53f, 0.8f));
        se.Append(rt.DOAnchorPosY(-54f, 1.2f));
        se.Append(rt.DOAnchorPosY(100f, 0.3f));
        se.AppendCallback(OnTextShowFinish);
        se.Play();
    }

    private void OnTextShowFinish()
    {
        tipsTextGameObj.SetActive(false);
        ShowTips();
    }

    private void Awake()
    {
        tipsTextGameObj = GameObject.Find("tipsText");
        tipsTextGameObj.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
