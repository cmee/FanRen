using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private Animator animator;
    private CharacterController characterController;
    //public float speed = 0.000000001F;
    public float gravity = 20.0F;

    TalkButtonController talkButtonController = null;

    // Start is called before the first frame update

    private void Awake()
    {
        //debug
        PlayerPrefs.DeleteAll();
    }

    void Start()
    {

        




        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        talkButtonController = GameObject.FindGameObjectWithTag("DoTalkButton")?.GetComponent<TalkButtonController>();

        mRigidbody = GetComponent<Rigidbody>();
    }

    //private bool IsAcking()
    //{
    //    return animator.GetCurrentAnimatorStateInfo(0).IsName("Ekard_Attack_01") || animator.GetCurrentAnimatorStateInfo(0).IsName("Ekard_Attack_02");
    //}

    Rigidbody mRigidbody;
    Vector3 moveDir;
    
    private void DoMove(Vector3 dir)
    {
        //if (characterController.isGrounded)
        //{
        //    characterController.SimpleMove(dir*speed);
        //}
        //else
        //{
        //    Vector3 d = Vector3.zero;
        //    d.y -= gravity * Time.deltaTime;
        //    characterController.Move(d);
        //}

        this.moveDir = dir;
        this.moveDir.y = 0f;

        //Debug.Log("animator.deltaPosition.magnitude " + animator.deltaPosition.magnitude);
        //mRigidbody.MovePosition(mRigidbody.position + this.moveDir * speed);
        //mRigidbody.MovePosition(mRigidbody.position + dir * speed);

    }

    float horizontal;
    float vertical;

    // Update is called once per frame
    void Update()
    {

        if(talkButtonController != null && talkButtonController.IsTalkUIShowing())
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    animator.SetBool("isAck", true);
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    animator.SetBool("isAck", false);
        //}        

    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) //斜
        {
            //镜头指向人物的水平向量
            //Vector3 camToPlayer = new Vector3(transform.position.x - Camera.main.transform.position.x, 0, transform.position.z - Camera.main.transform.position.z);
            //与【镜头指向人物的水平向量】垂直的水平向量
            //Vector3 vTo_CamToPlayer = horizontal > 0 ? GetRightSideVerticalDir(Camera.main.transform.position, transform.position, camToPlayer) : -GetRightSideVerticalDir(Camera.main.transform.position, transform.position, camToPlayer);
            //判断向前还是向后
            //camToPlayer = vertical > 0 ? camToPlayer : -camToPlayer;
            //人物最终朝向
            //Vector3 finalNormalized = (camToPlayer.normalized + vTo_CamToPlayer).normalized;
            //人物最终朝向点
            //Vector3 targetPos = new Vector3(finalNormalized.x + transform.position.x, transform.position.y, finalNormalized.z + transform.position.z);
            //transform.LookAt(targetPos);
            //DoMove((targetPos - transform.position) * speed);

            Vector3 camForword = Camera.main.transform.forward;
            camForword.y = 0f;
            Vector3 dir = (horizontal > 0 ? Camera.main.transform.right : -Camera.main.transform.right) + (vertical > 0 ? camForword : -camForword);

            transform.LookAt(transform.position + dir);
            DoMove(dir.normalized);
        }
        else if (horizontal != 0 && vertical == 0) //左右
        {
            //Vector3 camToPlayer = new Vector3(transform.position.x - Camera.main.transform.position.x, 0, transform.position.z - Camera.main.transform.position.z);
            //Vector3 vTo_CamToPlayer = GetRightSideVerticalDir(Camera.main.transform.position, transform.position, camToPlayer);
            //vTo_CamToPlayer = horizontal > 0 ? vTo_CamToPlayer : -vTo_CamToPlayer;
            //Vector3 targetPos = new Vector3(vTo_CamToPlayer.x + transform.position.x, transform.position.y, vTo_CamToPlayer.z + transform.position.z);
            //transform.LookAt(targetPos);
            transform.LookAt(horizontal > 0 ? transform.position + Camera.main.transform.right : transform.position - Camera.main.transform.right);
            DoMove(horizontal > 0 ? Camera.main.transform.right : -Camera.main.transform.right);
            //DoMove((targetPos - transform.position) * speed);


            //Debug.Log("transform.position || Camera.main.transform.right " + transform.position + " || " + Camera.main.transform.right);
            //Debug.Log("Camera.main.transform.right " + Camera.main.transform.right);
        }
        else if (horizontal == 0 && vertical != 0) //前后
        {
            //Vector3 camToPlayer = new Vector3(transform.position.x - Camera.main.transform.position.x, 0, transform.position.z - Camera.main.transform.position.z);
            //Vector3 dirNormalized = vertical > 0 ? camToPlayer.normalized : -camToPlayer.normalized;
            //Vector3 targetPos = new Vector3(dirNormalized.x + transform.position.x, transform.position.y, dirNormalized.z + transform.position.z);
            //transform.LookAt(targetPos);
            //Debug.Log("transform.position || Camera.main.transform.forward " + transform.position + " || " + Camera.main.transform.forward);
            Vector3 camForword = Camera.main.transform.forward;
            camForword.y = 0f;
            transform.LookAt(vertical > 0 ? transform.position + camForword : transform.position - camForword);
            DoMove(vertical > 0 ? camForword : -camForword);
            //DoMove((targetPos - transform.position) * speed);

        }

        if (horizontal != 0f || vertical != 0f)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        //Debug.Log("OnAnimatorMove()"); //mAnimator.deltaPosition.magnitude
        if (horizontal != 0f || vertical != 0f)
        {
            //if (mRigidbody.IsSleeping()) {
            //    mRigidbody.WakeUp();
            //}
            //mRigidbody.MovePosition(mRigidbody.position + this.moveDir * 0.12f);
            mRigidbody.AddForce(this.moveDir * 300f, ForceMode.Force);
        }
        else
        {
            //mRigidbody.Sleep();
        }

    }

    //private void OnAnimatorMove()
    //{
    //    //Debug.Log("OnAnimatorMove()"); //mAnimator.deltaPosition.magnitude
    //    if(horizontal != 0f || vertical != 0f)
    //    {
    //        //if (mRigidbody.IsSleeping()) {
    //        //    mRigidbody.WakeUp();
    //        //}
    //        mRigidbody.MovePosition(mRigidbody.position + this.moveDir * 0.12f);
    //    }
    //    else
    //    {
    //        //mRigidbody.Sleep();
    //    }
    //}

    //获取垂直水平向量(dir的正右边)
    //public static Vector3 GetRightSideVerticalDir(Vector3 startPos, Vector3 endPos, Vector3 _dir)
    //{        
    //    Vector3 result;
    //    if (_dir.x == 0)
    //    {
    //        result = new Vector3(1, 0, 0);
    //    }
    //    else
    //    {
    //        result = new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
    //    }        
    //    //如果终点在起点右上角，即dir指向右上角(在坐标系中)
    //    if(endPos.x > startPos.x && endPos.z > startPos.z) 
    //    {
    //        //如果垂直向量指向dir正右边(即指向右下角)，那么必然x>0，z<0
    //        if (result.x > 0 && result.z < 0) {
    //            return result;
    //        }
    //        else
    //        {
    //            return -result;
    //        }
    //    }else if(endPos.x > startPos.x && endPos.z < startPos.z)
    //    {
    //        if (result.x < 0 && result.z < 0)
    //        {
    //            return result;
    //        }
    //        else
    //        {
    //            return -result;
    //        }
    //    }
    //    else if (endPos.x < startPos.x && endPos.z > startPos.z)
    //    {
    //        if (result.x > 0 && result.z > 0)
    //        {
    //            return result;
    //        }
    //        else
    //        {
    //            return -result;
    //        }
    //    }
    //    else if (endPos.x < startPos.x && endPos.z < startPos.z)
    //    {
    //        if (result.x < 0 && result.z > 0)
    //        {
    //            return result;
    //        }
    //        else
    //        {
    //            return -result;
    //        }
    //    }
    //    return result;
    //}

}
