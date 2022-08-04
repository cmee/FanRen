using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BattleController : BaseMono
{
    private int width = 30;
    private int height = 30;

    public GameObject gridCubePrefab;
    private GameObject[,] grids;

    private Material gridMat;
    private Material ackGridMat;
    private Material ackGridMouseMoveMat;
    private Material roleCanMoveGridMat;
    //重叠色ackGridMat+ackGridMouseMoveMat
    private Material overlapColorMat;

    //private GameObject sliderAvatarPrefab;

    private List<GameObject> allRole;

    void Start()
    {
        Debug.Log("BattleController Start");
        //MyAudioManager.GetInstance().PlayBGM("BGM/BattleBGM01");
    }

    public void Init(List<GameObject> allRole)
    {
        this.allRole = allRole;
        Terrain terrain = GetComponent<Terrain>();
        width = (int)terrain.terrainData.bounds.size.x;
        height = (int)terrain.terrainData.bounds.size.z;
        grids = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject cube = Instantiate(gridCubePrefab);
                cube.transform.position = new Vector3(x + 0.5f, -0.4f, z + 0.5f);
                cube.name = x + "," + z;
                cube.SetActive(false);
                grids[x, z] = cube;
            }
        }

        gridMat = Resources.Load<Material>("Mat/GridMat");
        ackGridMat = Resources.Load<Material>("Mat/AckGridMat");
        roleCanMoveGridMat = Resources.Load<Material>("Mat/RoleCanMoveMat");
        ackGridMouseMoveMat = Resources.Load<Material>("Mat/AckPlaneMat");
        overlapColorMat = Resources.Load<Material>("Mat/OverlapColorMat");

    }

    public void OnChangeRoleAction(GameObject roleGO)
    {
        DoSelectRole(roleGO);
    }

    //点击了待机按钮回调,此方法只允许外部调用，不允许内部调用
    public void OnClickPass()
    {
        if (isPlayingAnim) return;
        ResetMouseAckRange();

        BaseRole selectedRoleCS = activingRoleGO.GetComponent<BaseRole>();
        selectedRoleCS.roleInBattleStatus = RoleInBattleStatus.Waiting;
        selectedRoleCS.DoCancelShentong();
        
        if (selectedRoleCS.battleToPosX != selectedRoleCS.battleOriginPosX) selectedRoleCS.battleOriginPosX = selectedRoleCS.battleToPosX;
        if (selectedRoleCS.battleToPosZ != selectedRoleCS.battleOriginPosZ) selectedRoleCS.battleOriginPosZ = selectedRoleCS.battleToPosZ;

        //GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().OnClickPassButton();
        activingRoleGO = null;

        for(int i=0; i<width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                this.grids[i, j].SetActive(false);
            }
        }
    }

    //回退
    public void OnClickReset()
    {
        if (isPlayingAnim) return;
        ResetMouseAckRange();
        BaseRole selectedRoleCS = activingRoleGO.GetComponent<BaseRole>();
        selectedRoleCS.DoCancelShentong();
        
        selectedRoleCS.battleToPosX = selectedRoleCS.battleOriginPosX;
        selectedRoleCS.battleToPosZ = selectedRoleCS.battleOriginPosZ;
        activingRoleGO.transform.position = new Vector3(selectedRoleCS.battleOriginPosX+0.5f, 0, selectedRoleCS.battleOriginPosZ+0.5f);

        ChangeGridOnClickRoleOrShentong();
    }

    

    GameObject activingRoleGO = null;

    // Update is called once per frame
    void Update()
    {
        if (base.IsClickUpOnUI()) return;
        OnMouseLeftClick();
        OnMouseMoveToCanAckGrid();
    }

    //===========> 换神通或者待机，需要重新初始化
    private GameObject lastMoveGameObject;//避免反复执行flag
    private GameObject lastChangeColorGOForPoint;

    private List<GameObject> lastNeedChangeColorGameObjects = new List<GameObject>();
    private List<GameObject> needChangeColorGameObjects = new List<GameObject>();

    private List<GameObject> lastChangeColorGOsForPlane = new List<GameObject>();
    //<===========

    private void OnMouseMoveToCanAckGrid()
    {
        if (isPlayingAnim) return;
        if (activingRoleGO != null)
        {
            BaseRole roleCS = activingRoleGO.GetComponent<BaseRole>();
            if(roleCS.selectedShentong != null && roleCS.selectedShentong.effType == ShentongEffType.Gong_Ji)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject clickGridGameObj = hitInfo.collider.gameObject;

                    //避免反复执行
                    if (lastMoveGameObject != clickGridGameObj) 
                    {                        
                        //只在红色色块上移动鼠标有效
                        if ("canAck".Equals(clickGridGameObj.tag))
                        {

                            if (roleCS.selectedShentong.rangeType == ShentongRangeType.Point)
                            {
                                if (lastChangeColorGOForPoint != null)
                                {
                                    lastChangeColorGOForPoint.tag = "canAck";
                                    lastChangeColorGOForPoint.GetComponent<Renderer>().material = ackGridMat;
                                }
                                clickGridGameObj.GetComponent<Renderer>().material = ackGridMouseMoveMat;
                                lastChangeColorGOForPoint = clickGridGameObj;
                            }
                            else if (roleCS.selectedShentong.rangeType == ShentongRangeType.Line)
                            {                                
                                if(!lastNeedChangeColorGameObjects.Contains(clickGridGameObj))
                                {

                                    //还原
                                    if (lastNeedChangeColorGameObjects.Count > 0)
                                    {
                                        foreach(GameObject tmp in lastNeedChangeColorGameObjects)
                                        {
                                            tmp.tag = "canAck";
                                            tmp.GetComponent<Renderer>().material = ackGridMat;
                                        }
                                        lastNeedChangeColorGameObjects.Clear();
                                    }

                                    string[] pos = clickGridGameObj.name.Split(',');
                                    int x = int.Parse(pos[0]);
                                    int z = int.Parse(pos[1]);
                                    needChangeColorGameObjects.Clear();

                                    //遍历x轴存起来
                                    //GameObject roleGrid = null;
                                    for (int i = 0; i < width; i++)
                                    {
                                        if (this.grids[x, i].tag.Equals("canAck"))
                                        {
                                            needChangeColorGameObjects.Add(this.grids[x, i]);
                                        }
                                        else
                                        {
                                            if(roleCS.battleToPosX == x && roleCS.battleToPosZ == i)
                                            {
                                                needChangeColorGameObjects.Add(this.grids[x, i]); //加入角色所站的grid，是为了给后面的循环做标记                                                
                                            } 
                                        }
                                    }
                                    
                                    if (needChangeColorGameObjects.Count == 1) //只有1个说明遍历x轴错了
                                    {                                        
                                        needChangeColorGameObjects.Clear();                                        
                                        for (int i = 0; i < height; i++) //应该遍历z轴存起来
                                        {
                                            if (this.grids[i, z].tag.Equals("canAck"))
                                            {
                                                needChangeColorGameObjects.Add(this.grids[i, z]);
                                            }
                                            else
                                            {
                                                if (roleCS.battleToPosX == i && roleCS.battleToPosZ == z)
                                                {
                                                    needChangeColorGameObjects.Add(this.grids[i, z]); //加入角色所站的grid，是为了给后面的循环做标记                                                    
                                                }
                                            }
                                        }
                                    }                                    

                                    //获取鼠标所在的grid处于集合的位置，然后双向循环
                                    int clickGOIndex = needChangeColorGameObjects.IndexOf(clickGridGameObj);
                                    for(int i=clickGOIndex; i< int.MaxValue; i++)
                                    {
                                        if (i >= needChangeColorGameObjects.Count || !needChangeColorGameObjects[i].tag.Equals("canAck"))//如果遇到角色所站的grid，则停止
                                        {
                                            break;
                                        }
                                        needChangeColorGameObjects[i].GetComponent<Renderer>().material = ackGridMouseMoveMat;
                                        lastNeedChangeColorGameObjects.Add(needChangeColorGameObjects[i]);                                        
                                    }
                                    for (int i = clickGOIndex; i >= 0; i--)
                                    {
                                        if (!needChangeColorGameObjects[i].tag.Equals("canAck"))//如果遇到角色所站的grid，则停止
                                        {
                                            break;
                                        }
                                        needChangeColorGameObjects[i].GetComponent<Renderer>().material = ackGridMouseMoveMat;
                                        lastNeedChangeColorGameObjects.Add(needChangeColorGameObjects[i]);
                                    }
                                    
                                }
                            }
                            else if (roleCS.selectedShentong.rangeType == ShentongRangeType.Plane)
                            {
                                if(lastChangeColorGOsForPlane.Count > 0)
                                {
                                    foreach (GameObject tmp in lastChangeColorGOsForPlane)
                                    {
                                        if (tmp.tag.Equals("Untagged"))
                                        {
                                            tmp.GetComponent<Renderer>().material = gridMat;
                                        }
                                        else if (tmp.tag.Equals("canMove"))
                                        {
                                            tmp.GetComponent<Renderer>().material = roleCanMoveGridMat;
                                        }
                                        else if (tmp.tag.Equals("canAck"))
                                        {
                                            tmp.GetComponent<Renderer>().material = ackGridMat;
                                        }
                                    }
                                    lastChangeColorGOsForPlane.Clear();
                                }                                

                                string[] pos = clickGridGameObj.name.Split(',');
                                int x = int.Parse(pos[0]);
                                int z = int.Parse(pos[1]);
                                int planeR = activingRoleGO.GetComponent<BaseRole>().selectedShentong.planeRadius;
                                //把需要循环的范围缩小
                                int minX = x - planeR < 0 ? 0 : x - planeR;
                                int maxX = x + planeR >= width ? width : x + planeR;
                                int minZ = z - planeR < 0 ? 0 : z - planeR;
                                int maxZ = z + planeR >= height ? height : z + planeR;
                                for(int i = minX; i <= maxX; i++)
                                {
                                    for(int j = minZ; j <= maxZ; j++)
                                    {
                                        if ((Mathf.Abs(x-i) + Mathf.Abs(z-j)) <= planeR)
                                        {
                                            if(i>=0 && j>=0 && i<width && j < height)
                                            {
                                                if(this.grids[i, j].tag.Equals("canAck"))
                                                {
                                                    //重叠色                                                    
                                                    this.grids[i, j].GetComponent<Renderer>().material = overlapColorMat;
                                                }
                                                else
                                                {
                                                    this.grids[i, j].GetComponent<Renderer>().material = ackGridMouseMoveMat;
                                                }                                                
                                                lastChangeColorGOsForPlane.Add(this.grids[i, j]);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (roleCS.selectedShentong.rangeType == ShentongRangeType.Point && lastChangeColorGOForPoint != null)
                            {
                                Debug.Log("clear lastChangeColorGOForPoint");
                                lastChangeColorGOForPoint.GetComponent<Renderer>().material = ackGridMat;
                            }
                            else if (roleCS.selectedShentong.rangeType == ShentongRangeType.Line && lastNeedChangeColorGameObjects.Count > 0)
                            {

                                foreach (GameObject tmp in lastNeedChangeColorGameObjects)
                                {
                                    tmp.tag = "canAck";
                                    tmp.GetComponent<Renderer>().material = ackGridMat;
                                }
                                Debug.Log("clear lastNeedChangeColorGameObjects");
                                lastNeedChangeColorGameObjects.Clear();
                            }
                            else if (roleCS.selectedShentong.rangeType == ShentongRangeType.Plane && lastChangeColorGOsForPlane.Count > 0)
                            {
                                foreach (GameObject tmp in lastChangeColorGOsForPlane)
                                {
                                    if (tmp.tag.Equals("Untagged"))
                                    {
                                        tmp.GetComponent<Renderer>().material = gridMat;
                                    }
                                    else if (tmp.tag.Equals("canMove"))
                                    {
                                        tmp.GetComponent<Renderer>().material = roleCanMoveGridMat;
                                    }
                                    else if (tmp.tag.Equals("canAck"))
                                    {
                                        tmp.GetComponent<Renderer>().material = ackGridMat;
                                    }
                                }
                                Debug.Log("clear lastChangeColorGOsForPlane");
                                lastChangeColorGOsForPlane.Clear();
                            }
                        }

                        

                        lastMoveGameObject = clickGridGameObj;
                    }
                    
                        



                }
            }
        }
    }

    private bool HasRoleOnTheGrid(GameObject clickGrid)
    {
        //GameObject[] allRoles = GameObject.FindGameObjectWithTag("RootBattleInit").GetComponent<RootBattleInit>().roles;
        foreach (GameObject roleGO in this.allRole)
        {
            if (roleGO == null || !roleGO.activeInHierarchy || !roleGO.activeSelf) continue;
            BaseRole roleCS = roleGO.GetComponent<BaseRole>();
            string[] pos = clickGrid.name.Split(',');
            if (roleCS.battleOriginPosX == int.Parse(pos[0])
                && roleCS.battleOriginPosZ == int.Parse(pos[1]))
            {
                return true;
            }
        }
        return false;
    }

    private void OnMouseLeftClick()
    {
        if (isPlayingAnim) return;
        if (Input.GetMouseButtonUp(0))
        {
            //从摄像机发出到点击坐标的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                //划出射线，只有在scene视图中才能看到
                //Debug.DrawLine(ray.origin, hitInfo.point);
                GameObject clickGameObj = hitInfo.collider.gameObject;

                //Debug.Log("click object name is " + clickGameObj.tag);

                if (activingRoleGO == clickGameObj || clickGameObj.tag.Equals("Untagged") || clickGameObj.tag.Equals("Terrain"))
                {
                    return;
                }



                //点击了可移动的地板
                if (clickGameObj.tag.Equals("canMove") && activingRoleGO != null && activingRoleGO.GetComponent<BaseRole>().selectedShentong == null)
                {
                    //点击了可移动的地板
                    //selectedGO.transform.LookAt(clickGameObj.transform);
                    //this.transform.Translate(Vector3.forward * 10);

                    if (HasRoleOnTheGrid(clickGameObj))
                    {
                        return;
                    }

                    List<Vector3> path = new List<Vector3>();
                    path.Add(new Vector3(activingRoleGO.transform.position.x, activingRoleGO.transform.position.y + 5, activingRoleGO.transform.position.z));
                    path.Add(new Vector3(clickGameObj.transform.position.x, 3f, clickGameObj.transform.position.z));
                    path.Add(new Vector3(clickGameObj.transform.position.x, 0f, clickGameObj.transform.position.z));

                    activingRoleGO.transform.LookAt(path[path.Count - 1]);

                    Hashtable args = new Hashtable();
                    //lookahead
                    //args.Add("lookahead", 0.9f);
                    args.Add("path", path.ToArray());
                    args.Add("looptype", iTween.LoopType.none);
                    args.Add("time", 1);
                    //args.Add();
                    //args.Add();
                    args.Add("oncomplete", "OnComplete");
                    args.Add("onCompleteTarget", this.gameObject);
                    //args.Add("speed", 7);
                    //args.Add("orienttopath", true);
                    //args.Add("position", );
                    isPlayingAnim = true;
                    iTween.MoveTo(activingRoleGO, args);



                    string[] indexs = clickGameObj.name.Split(',');
                    BaseRole role = activingRoleGO.GetComponent<BaseRole>();
                    role.battleToPosX = int.Parse(indexs[0]);
                    role.battleToPosZ = int.Parse(indexs[1]);

                    
                }
                else if (clickGameObj.GetComponent<BaseMono>().gameObjectType == GameObjectType.Role) //点击在了角色身上
                {
                    Debug.LogError("???????????????????点击在了角色身上??????? 这行log永远都不会打印，打印了必然有逻辑错误 " + clickGameObj.name);

                    //看不懂这里的逻辑
                    if(activingRoleGO != null && activingRoleGO.GetComponent<BaseRole>().roleInBattleStatus == RoleInBattleStatus.Activing) //全部头像都在移动的时候 activingRoleGO is null
                    {
                        return;
                    }
                    //看不懂这里的逻辑
                    DoSelectRole(clickGameObj);
                }
                else if (clickGameObj.tag.Equals("canAck") && activingRoleGO != null && activingRoleGO.GetComponent<BaseRole>().selectedShentong != null)
                {
                    bool flag = true;
                    isPlayingAnim = true;
                    Debug.Log("开始播放人物攻击动画和神通动画");
                    Shentong shentong = activingRoleGO.GetComponent<BaseRole>().selectedShentong;
                    if (shentong.rangeType == ShentongRangeType.Point)
                    {
                        MyAudioManager.GetInstance().PlaySE(shentong.soundEffPath);

                        GameObject shentongEffPrefab = Resources.Load<GameObject>(shentong.effPath);
                        ParticleSystem particleSystem = shentongEffPrefab.GetComponent<ParticleSystem>();
                        MainModule mainModule = particleSystem.main;
                        mainModule.stopAction = ParticleSystemStopAction.Callback;

                        //this.requestCode++;
                        //shentongEffPrefab.GetComponent<EffController>().requestCode = this.requestCode;
                        GameObject stEffGO = Instantiate(shentongEffPrefab);
                        stEffGO.transform.position = new Vector3(clickGameObj.transform.position.x, 1, clickGameObj.transform.position.z);
                    }
                    else if (shentong.rangeType == ShentongRangeType.Line)
                    {

                        MyAudioManager.GetInstance().PlaySE(shentong.soundEffPath);

                        GameObject shentongEffPrefab = Resources.Load<GameObject>(shentong.effPath);
                        ParticleSystem particleSystem = shentongEffPrefab.GetComponent<ParticleSystem>();
                        MainModule mainModule = particleSystem.main;
                        mainModule.stopAction = ParticleSystemStopAction.Destroy;

                        foreach (GameObject tmp in lastNeedChangeColorGameObjects)
                        {
                            GameObject stEffGO = Instantiate(shentongEffPrefab);
                            if (flag)
                            {
                                flag = false;
                                particleSystem = stEffGO.GetComponent<ParticleSystem>();
                                mainModule = particleSystem.main;
                                mainModule.stopAction = ParticleSystemStopAction.Callback;
                            }
                            stEffGO.transform.position = new Vector3(tmp.transform.position.x, 1, tmp.transform.position.z);
                        }
                    }
                    else if (shentong.rangeType == ShentongRangeType.Plane)
                    {

                        MyAudioManager.GetInstance().PlaySE(shentong.soundEffPath);

                        GameObject shentongEffPrefab = Resources.Load<GameObject>(shentong.effPath);
                        ParticleSystem particleSystem = shentongEffPrefab.GetComponent<ParticleSystem>();
                        MainModule mainModule = particleSystem.main;
                        mainModule.stopAction = ParticleSystemStopAction.Destroy;

                        foreach (GameObject tmp in lastChangeColorGOsForPlane)
                        {
                            GameObject stEffGO = Instantiate(shentongEffPrefab);
                            if (flag)
                            {
                                flag = false;
                                particleSystem = stEffGO.GetComponent<ParticleSystem>();
                                mainModule = particleSystem.main;
                                mainModule.stopAction = ParticleSystemStopAction.Callback;
                            }
                            stEffGO.transform.position = new Vector3(tmp.transform.position.x, 1, tmp.transform.position.z);
                        }
                    }
                    enemyCount = HandleAfterAck();
                }

            }
        }
    }

    //人物正在移动
    private bool isPlayingAnim = false;

    //还没死的敌人数量
    int enemyCount;

    private void OnComplete()
    {
        isPlayingAnim = false;
    }

    public void OnShentongParticleSystemStopped()
    {
        isPlayingAnim = false;
        if (enemyCount == 0)
        {
            //敌人死光了，显示对应画面
            if (this.activingRoleGO.GetComponent<HanLi>() != null)
            {
                Debug.Log("我方胜利，显示对应画面");
                PlayerPrefs.SetInt(RootBattleInit.triggerToBattleGameObjUnionPreKey, 1); //关闭战斗触发器
                GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().OnBattleEnd(true);
            }
            else
            {
                Debug.Log("我方失败，显示对应画面");
                GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().OnBattleEnd(false);
            }
        }
        else
        {
            //OnClickPass();
            GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().OnClickPassButton();
        }
    }

    //返回  攻击后还剩下多少敌人没有死
    private int HandleAfterAck()
    {
        BaseRole activingRoleCS = activingRoleGO.GetComponent<BaseRole>();
        Shentong selectedShentong = activingRoleCS.selectedShentong;
        int enemyCount = 0;
        if (selectedShentong.rangeType == ShentongRangeType.Point)
        {
            string[] xz = lastChangeColorGOForPoint.name.Split(',');
            foreach (GameObject roleGO in this.allRole)
            {
                if (roleGO == null || !roleGO.activeInHierarchy || !roleGO.activeSelf) continue;
                BaseRole roleCS = roleGO.GetComponent<BaseRole>();
                if (roleCS.teamNum != activingRoleCS.teamNum) //敌人
                {
                    enemyCount++;
                    if (roleCS.battleOriginPosX == int.Parse(xz[0])
                    && roleCS.battleOriginPosZ == int.Parse(xz[1])) //点击的grid上有人
                    {
                        if (activingRoleCS.DoAck(roleCS))
                        {
                            //this.allRole.Remove(roleGO);
                            enemyCount--;
                        }
                    }
                }
            }
        }
        else if (selectedShentong.rangeType == ShentongRangeType.Line)
        {
            Dictionary<string, GameObject> pos_gridGO = new Dictionary<string, GameObject>();
            foreach (GameObject ackRangeGrid in lastNeedChangeColorGameObjects)
            {
                pos_gridGO[ackRangeGrid.name] = ackRangeGrid;
            }
            GameObject valueOut = null;
            foreach (GameObject roleGO in this.allRole)
            {
                if (roleGO == null || !roleGO.activeInHierarchy || !roleGO.activeSelf) continue;
                BaseRole roleCS = roleGO.GetComponent<BaseRole>();
                if (roleCS.teamNum != activingRoleCS.teamNum) //敌人
                {
                    enemyCount++;
                    if (pos_gridGO.TryGetValue(roleCS.battleOriginPosX + "," + roleCS.battleOriginPosZ, out valueOut)) //点击的grid上有人
                    {
                        if (activingRoleCS.DoAck(roleCS))
                        {
                            enemyCount--;
                        }
                    }
                }
            }
        }
        else if (selectedShentong.rangeType == ShentongRangeType.Plane)
        {
            Dictionary<string, GameObject> pos_gridGO = new Dictionary<string, GameObject>();
            foreach (GameObject ackRangeGrid in lastChangeColorGOsForPlane)
            {
                pos_gridGO[ackRangeGrid.name] = ackRangeGrid;
            }
            GameObject valueOut = null;
            foreach (GameObject roleGO in this.allRole)
            {
                if (roleGO == null || !roleGO.activeInHierarchy || !roleGO.activeSelf) continue;
                BaseRole roleCS = roleGO.GetComponent<BaseRole>();
                if (roleCS.teamNum != activingRoleCS.teamNum)
                {
                    enemyCount++;
                    if (pos_gridGO.TryGetValue(roleCS.battleOriginPosX + "," + roleCS.battleOriginPosZ, out valueOut)) //点击的grid上有人
                    {
                        if (activingRoleCS.DoAck(roleCS))
                        {
                            enemyCount--;
                        }
                    }
                }
            }
        }
        return enemyCount;
    }

    private void DoSelectRole(GameObject activingGameObj)
    {
        this.activingRoleGO = activingGameObj;

        //Debug.Log("click object name is " + clickGameObj.name);

        BaseRole selectRoleCS = this.activingRoleGO.GetComponent<BaseRole>();
        selectRoleCS.roleInBattleStatus = RoleInBattleStatus.Activing;

        //GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<BattleUIControl>().OnRoleSelected(selectRoleCS);
        //passButton?.SetActive(true);
        //resetButton?.SetActive(true);

        ChangeGridOnClickRoleOrShentong();

        BattleCameraController rcc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BattleCameraController>();
        rcc.SetSelectedRole(activingRoleGO);

    }

    private void ResetMouseAckRange()
    {
        lastMoveGameObject = null;
        lastChangeColorGOForPoint = null;
        lastNeedChangeColorGameObjects.Clear();
        needChangeColorGameObjects.Clear();
        lastChangeColorGOsForPlane.Clear();
    }

    public void OnRoleSelectedShentong(Shentong shentong)
    {
        ResetMouseAckRange();
        ChangeGridOnClickRoleOrShentong();
    }

    //public Shentong selectedShentong;

    public void ChangeGridOnClickRoleOrShentong()
    {

        BaseRole selectedRoleCS = activingRoleGO.GetComponent<BaseRole>();
        if(selectedRoleCS == null)
        {
            Debug.LogError("ChangeGridOnClickRole() baseRole is null");
            return;
        }

        //string[] posIndex = selectedGO.name.Split(',');
        int clickRoleOriginX = selectedRoleCS.battleOriginPosX;
        int clickRoleOriginZ = selectedRoleCS.battleOriginPosZ;
        //Debug.Log("click object name is " + gameObj.name);

        //Renderer renderer = gameObj.GetComponent<Renderer>();
        //Material material = renderer.material;                

        Renderer renderer;
        GameObject gridGO;
        int disX;
        int disY;

        int disToX;
        int disToY;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {                

                gridGO = grids[x, z];
                gridGO.SetActive(true);
                renderer = gridGO.GetComponent<Renderer>();

                
                if(selectedRoleCS.selectedShentong != null 
                    && selectedRoleCS.selectedShentong.effType == ShentongEffType.Gong_Ji)
                {

                    disToX = Math.Abs(x - selectedRoleCS.battleToPosX);
                    disToY = Math.Abs(z - selectedRoleCS.battleToPosZ);

                    if (selectedRoleCS.selectedShentong.rangeType == ShentongRangeType.Line)
                    {
                        if (selectedRoleCS.battleToPosX == x || selectedRoleCS.battleToPosZ == z)
                        {
                            if((disToX + disToY) <= selectedRoleCS.selectedShentong.unitDistance && (disToX + disToY) != 0)
                            {
                                //变红                           
                                gridGO.tag = "canAck";
                                if (renderer.material.color.r != ackGridMat.color.r)
                                {
                                    renderer.material = ackGridMat;
                                }
                                continue;
                            }                            
                        }
                    }
                    else if (selectedRoleCS.selectedShentong.rangeType == ShentongRangeType.Point || selectedRoleCS.selectedShentong.rangeType == ShentongRangeType.Plane)
                    {
                        
                        if ((disToX + disToY) <= selectedRoleCS.selectedShentong.unitDistance && (disToX + disToY) != 0)
                        {
                            //变红                           
                            gridGO.tag = "canAck";
                            if (renderer.material.color.r != ackGridMat.color.r) 
                            {
                                renderer.material = ackGridMat;
                            }
                            continue;
                        }
                    }  
                    
                }


                disX = Math.Abs(x - clickRoleOriginX);
                disY = Math.Abs(z - clickRoleOriginZ);

                if ((disX + disY) < selectedRoleCS.GetMoveDistanceInBattle()) //404EFF蓝139,150,219,107,    A4D7A3绿164,214,163,107 
                    {
                        //变绿
                        //Debug.Log("可以移动: " + x + "," + z);
                        gridGO.tag = "canMove";
                        if (renderer.material.color.r != roleCanMoveGridMat.color.r) renderer.material = roleCanMoveGridMat;
                    }
                    else
                    {
                        //变蓝
                        //grids[x, z].GetComponent<Renderer>().material = gridMat;
                        gridGO.tag = "Untagged";
                        if (renderer.material.color.r != gridMat.color.r) renderer.material = gridMat;
                    }
                
                
            }
        }
    }

    private void OnDestroy()
    {
        //MyAudioManager.GetInstance().StopBGM();
        Resources.UnloadUnusedAssets();
    }

}


