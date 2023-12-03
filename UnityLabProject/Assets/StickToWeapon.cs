using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToWeapon : MonoBehaviour
{
    private Transform effector;
    private GameObject grip;
    void Start()
    {
        effector = GameObject.Find("LeftHandEffector").transform;
        grip = GameObject.Find("LeftHandGrip");
    }

    // Update is called once per frame
    void Update()
    {
        if (effector != null && grip != null)
        {
            effector.position = grip.transform.position;
        }
    }
}
