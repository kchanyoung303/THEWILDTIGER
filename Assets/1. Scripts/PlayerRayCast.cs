using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    void DestroyBox()
    {
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position,this.transform.up,out hit,5f))
        {
            if(hit.transform.name == "Box")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
