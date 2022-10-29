using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour
{
    public float turnSpeed;

    void Update() {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }
}
