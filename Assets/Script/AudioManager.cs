using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    private AudioSource bgm;

    private AudioSource soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Instance = this;
        bgm = gameObject.AddComponent<AudioSource>();
        bgm.loop = true;

        soundEffects = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(string path)
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        bgm.clip = audioClip;
        bgm.volume = 0.5f;
        bgm.Play();
    }

    public void StopBGM()
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
    }

    public void PlaySE(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        soundEffects.clip = audioClip;
        soundEffects.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
