using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDescScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoStartMenuScene()
    {
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync(5, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

}
