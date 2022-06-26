using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageText : MonoBehaviour
{
    public float Movespeed;
    public float alphaSpeed;
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    private PlayerCtrl playerctrl;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        playerctrl = player.GetComponent<PlayerCtrl>();
        text = GetComponent<TextMeshPro>();
        text.text = playerctrl.AtkDamege.ToString();
        alpha = text.color;
        Invoke("DestroyText",destroyTime);
    }
    void Update()
    {
        transform.Translate(new Vector3(0, Movespeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime*alphaSpeed);
        text.color = alpha;
    }
    private void DestroyText()
    {
        Destroy(gameObject);
    }
}
