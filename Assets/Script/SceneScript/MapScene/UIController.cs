using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        
    //»•ŒÂ¿Ôπµ
    public void OnWuLiGouClick()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

}
