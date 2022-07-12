using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EffController : MonoBehaviour
{
    // Start is called before the first frame update

    //public int requestCode;

    void Start()
    {
        //ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        //MainModule mainModule = particleSystem.main;
        //mainModule.stopAction = ParticleSystemStopAction.Callback;
        //mainModule.loop = false;
    }

    public void OnParticleSystemStopped()
    {
        GameObject.FindGameObjectWithTag("Terrain").GetComponent<BattleController>().OnShentongParticleSystemStopped();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
