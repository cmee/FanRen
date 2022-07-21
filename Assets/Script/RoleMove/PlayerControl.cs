using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Camera playerCamera;
    private Animator animator;

    TalkButtonController talkButtonController = null;


    private void Awake()
    {
        //debug
        PlayerPrefs.DeleteAll();
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        talkButtonController = GameObject.FindGameObjectWithTag("DoTalkButton")?.GetComponent<TalkButtonController>();

        mRigidbody = GetComponent<Rigidbody>();

        if (playerCamera == null) playerCamera = Camera.main;
    }

    Rigidbody mRigidbody;
    Vector3 moveDir;
    
    private void DoMove(Vector3 dir)
    {
        this.moveDir = dir;
        this.moveDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, this.moveDir, 0.7f, 0f));
    }

    float horizontal;
    float vertical;

    void Update()
    {
        if(talkButtonController != null && talkButtonController.IsTalkUIShowing())
        {
            return;
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) //Ð±
        {
            Vector3 camForword = playerCamera.transform.forward;
            camForword.y = 0f;
            Vector3 dir = (horizontal > 0 ? playerCamera.transform.right : -playerCamera.transform.right) + (vertical > 0 ? camForword : -camForword);
            DoMove(dir.normalized);
        }
        else if (horizontal != 0 && vertical == 0) //×óÓÒ
        {
            DoMove(horizontal > 0 ? playerCamera.transform.right : -playerCamera.transform.right);
        }
        else if (horizontal == 0 && vertical != 0) //Ç°ºó
        {
            Vector3 camForword = playerCamera.transform.forward;
            camForword.y = 0f;
            DoMove(vertical > 0 ? camForword : -camForword);

        }

        if (horizontal != 0f || vertical != 0f)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        if (horizontal != 0f || vertical != 0f)
        {
            mRigidbody.AddForce(this.moveDir * 300f, ForceMode.Force);
        }

    }

}
