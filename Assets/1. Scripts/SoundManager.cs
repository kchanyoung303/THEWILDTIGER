using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private AudioClip[] BgmClip;
    [SerializeField]
    private AudioClip[] EfectClip;

    private AudioSource bgm = null;
    private AudioSource effectSound = null;


    void Awake()
    {
        TryGetComponent(out bgm);
        this.transform.GetChild(0).TryGetComponent(out effectSound);
    }

    public void SetBackGroundSoundClip(int state)
    {
        bgm.Stop();
        bgm.clip = BgmClip[(int)state];
        bgm.Play();
    }
    public void SetEffectSoundClip(int state)
    {
        effectSound.Stop();
        effectSound.clip = EfectClip[(int)state];
        effectSound.Play();
    }
    //public void bgmSetVolume()
    //{
    //    bgm.volume = bgmScrollbar.value;
    //}
    //public void effetSetVolume()
    //{
    //    effectSound.volume = effectScrollbar.value;
    //}
    //void Update()
    //{
    //    //bgmSetVolume();
    //    //effetSetVolume();
    //}



}