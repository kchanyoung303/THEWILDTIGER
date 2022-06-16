using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCtrl playerctrl = null;
    public Slider hpbar;
    public float maxValue;
    public float currentValue;

    void Start()
    {
        playerctrl = GetComponent<PlayerCtrl>();
    }
    void Update()
    {

    }
    void barCurrent(float _value)
    {
        hpbar.value = _value / maxValue;
    }
}
