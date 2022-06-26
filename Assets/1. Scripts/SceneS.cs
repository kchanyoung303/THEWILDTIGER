using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneS : MonoBehaviour
{
    private PlayerCtrl playerctrl;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        playerctrl = player.GetComponent<PlayerCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();
    }
    void GameOver()
    {
        if (playerctrl.hp <= 0f || playerctrl.water <= 0f || playerctrl.hungry <= 0f)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
