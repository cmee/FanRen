using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : BaseMono
{

    public GameObject videoPlayerGO;
    public GameObject buttonContainer;
    public GameObject jumpVideoButton;

    VideoPlayer vp;

    // Start is called before the first frame update
    void Start()
    {
        vp = videoPlayerGO.GetComponent<VideoPlayer>();
        vp.loopPointReached += EndVideo;

        if(PlayerPrefs.GetInt("videoHasLookComplete", 0) == 1)
        {
            jumpVideoButton.SetActive(true);
        }
    }

    void EndVideo(VideoPlayer video)
    {
        Debug.Log("video end");
        //buttonContainer <-
        PlayerPrefs.SetInt("videoHasLookComplete", 1);
        jumpVideoButton.SetActive(false);
        MyAudioManager.GetInstance().PlaySE("SoundEff/jianChuQiao2");
        EnterAnim();
    }

    public void OnJumpButtonClick()
    {
        vp.frame = (long)vp.frameCount - 30;
        //vp.Stop();
        jumpVideoButton.SetActive(false);
        //EnterAnim();
    }

    public void EnterAnim()
    {
        RectTransform rt = buttonContainer.GetComponent<RectTransform>();
        rt.DOAnchorPosX(-250f, 1f);
    }

    public void OutAnim()
    {
        RectTransform rt = buttonContainer.GetComponent<RectTransform>();
        rt.DOAnchorPosX(1000f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //RaycastHit hitInfo;
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    GameObject clickGameObj = hitInfo.collider.gameObject;
        //    Debug.Log("clickGameObj " + clickGameObj);
        //}
        //else
        //{
        //    //Debug.Log("hitInfo " + hitInfo);
        //}
    }

    public void OnStartButtonClick()
    {
        Debug.Log("OnStartButtonClick");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync(0, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    public void OnReplayButtonClick()
    {
        if (!vp.isPlaying)
        {
            vp.Play();
            OutAnim();
            jumpVideoButton.SetActive(true);
            //buttonContainer ->

        }
    }

    public void OnExitButtonClick()
    {
        Debug.Log("OnExitButtonClick");
        //Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
