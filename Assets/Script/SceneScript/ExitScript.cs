using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : BaseMono, IColliderWithCC
{

    //public int selfSceneIndex = -1;
    public int targetSceneIndex;

    public void OnPlayerCollisionEnter(GameObject player)
    {
        Debug.Log("ExitScript OnCollisionEnter()");
        SceneManager.LoadScene(targetSceneIndex, LoadSceneMode.Single);
    }

    public void OnPlayerCollisionExit(GameObject player)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ExitScript OnCollisionEnter()");
        //SceneManager.LoadScene(targetSceneIndex, LoadSceneMode.Single);
        //if (selfSceneIndex >= 0)
        //{
        //    SceneManager.UnloadSceneAsync(selfSceneIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        //}
        
    }

}
