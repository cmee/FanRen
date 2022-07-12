using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager: MonoBehaviour
{

    public static MyAudioManager instance;

    private AudioSource bgm;

    private AudioSource soundEffects;

    public string defaultBGMPath = null;

    public static MyAudioManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        soundEffects = gameObject.AddComponent<AudioSource>();
        bgm = gameObject.AddComponent<AudioSource>();
        bgm.loop = true;
    }

    // Start is called before the first frame update

    //private void Update()
    //{
    //    if (defaultBGMPath != null)
    //    {
    //        PlayBGM(this.defaultBGMPath);
    //    }
    //}

    private void Start()
    {
        if (defaultBGMPath != null)
        {
            PlayBGM(this.defaultBGMPath);
        }
    }

    string bgmPath;

    public void PlayBGM(string path)
    {
        if(this.bgmPath != null && this.bgmPath.Equals(path))
        {
            Debug.Log("重复播放bgm了，返回不执行 " + path);
            return;
        }
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
        this.bgmPath = path;
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        bgm.clip = audioClip;
        bgm.volume = 0.5f;
        bgm.Play();
        Debug.Log("PlayBGM " + path);
    }

    public void PlayDefaultBGM()
    {
        PlayBGM(this.defaultBGMPath);
    }

    public void StopBGM()
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
    }

    string path;

    public void PlaySE(string path)
    {
        if(this.path == null || !this.path.Equals(path))
        {
            this.path = path;
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            soundEffects.clip = audioClip;
        }
        soundEffects.Play();
    }

}
