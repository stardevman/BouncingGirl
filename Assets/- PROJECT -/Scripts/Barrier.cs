using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public bool isFallBack = false;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag != "Player")
            return;

        PlayerController controller = col.GetComponent<PlayerController>();

        if (isFallBack)
            controller.FallBack();
        else
            controller.FallOnTheGround();     
    }
}
