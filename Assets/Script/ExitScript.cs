using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{

    //public int selfSceneIndex = -1;
    public int targetSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ExitScript OnCollisionEnter()");
        SceneManager.LoadScene(targetSceneIndex, LoadSceneMode.Single);
        //if (selfSceneIndex >= 0)
        //{
        //    SceneManager.UnloadSceneAsync(selfSceneIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        //}
        
    }

}
