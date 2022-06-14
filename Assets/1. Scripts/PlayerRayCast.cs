using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRayCast : MonoBehaviour
{
    private bool pickupActivated = false;  // æ∆¿Ã≈€ Ω¿µÊ ∞°¥…«“Ω√ True 
    public Text actiontext;
    RaycastHit hit;
    private void Update()
    {
        DestroyBox();

    }
    void DestroyBox()
    {

        Debug.DrawRay(this.transform.position+Vector3.up*0.5f, this.transform.forward * 5f, Color.red);
        if (Physics.Raycast(this.transform.position+Vector3.up*0.5f, this.transform.forward, out hit, 5f))
            {
            if (hit.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(hit.transform.gameObject);
                    ItemInfoDisappear();
                }
            }
        }
        else
        {
            ItemInfoDisappear();
        }


    }
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actiontext.gameObject.SetActive(true);
        actiontext.text = "æ∆¿Ã≈€ »πµÊ " + "<color=yellow>" + "(E)" + "</color>";
    }
    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actiontext.gameObject.SetActive(false);
    }
}
