using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleContactProvider : MonoBehaviour
{
    public bool colliding = false;
    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }
}
