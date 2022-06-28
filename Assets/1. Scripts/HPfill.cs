using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPfill : MonoBehaviour
{

    public Slider hpBar;
    public Slider hungryBar;
    public Slider WaterBar;
    private PlayerCtrl playerctrl;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        playerctrl = player.GetComponent<PlayerCtrl>();
    }
    void Update()
    {

        hpBar.value = (float)playerctrl.hp / (float)playerctrl.hpvalue;
        WaterBar.value = (float)playerctrl.water / (float)playerctrl.watervalue;
        hungryBar.value = (float)playerctrl.hungry / (float)playerctrl.hungryvalue;

    }
}