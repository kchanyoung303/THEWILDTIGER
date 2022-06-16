using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRayCast : MonoBehaviour
{
    private bool pickupActivated = false;  // 아이템 습득 가능할시 True 
    public Text actiontext;
    RaycastHit hit;
    private PlayerCtrl playctrl = null;
    private void Update()
    {
        DestroyBox();
        DrinkWater();
        playctrl = GetComponent<PlayerCtrl>();
    }

    void DrinkWater()
    {
        Ray ray = new Ray(this.transform.position - Vector3.forward * 1.5f + Vector3.up * 0.5f,(this.transform.forward-this.transform.up));
        Debug.DrawRay(this.transform.position -Vector3.forward*1.5f+ Vector3.up *0.5f, (this.transform.forward - this.transform.up) * 1f, Color.blue);
        if(Physics.Raycast(ray,out hit,1f))
        {
            if(hit.transform.CompareTag("Water"))
            {
                ItemInfoAppear(2);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(playctrl.watervalue>80f)
                    {
                        playctrl.watervalue = 100f;
                    }
                    else
                    {
                        playctrl.watervalue += 80f;
                    }

                    ItemInfoAppear(2);
                }
            }
        }
        else
        {
            ItemInfoDisappear(2);
        }
    }
    void DestroyBox()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.5f, this.transform.forward);
        Debug.DrawRay(this.transform.position+Vector3.up*0.5f, this.transform.forward * 5f, Color.red);
        if (Physics.Raycast(ray, out hit, 5f))
            {
            if (hit.transform.CompareTag("Item"))
            {
                ItemInfoAppear(1);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(hit.transform.gameObject);
                    ItemInfoDisappear(1);
                }
            }
        }
        else
        {
            ItemInfoDisappear(1);
        }


    }
    private void ItemInfoAppear(int value)
    {
        pickupActivated = true;
        actiontext.gameObject.SetActive(true);

        switch(value)
        {
            case 1:
                actiontext.text = "아이템 획득 " + "<color=yellow>" + "E" + "</color>";
                break;
            case 2:
                actiontext.text = "물 마시기 " + "<color=blue>" + "E" + "</color>";
                break;
        }    

    }
    private void ItemInfoDisappear(int value)
    {
        pickupActivated = false;
        actiontext.gameObject.SetActive(false);
    }
}
