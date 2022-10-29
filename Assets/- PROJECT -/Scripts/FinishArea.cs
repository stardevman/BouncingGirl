using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider col) {
        if (col.tag != "Player")
            return;

        col.GetComponent<PlayerController>().Finish();
    }
}
