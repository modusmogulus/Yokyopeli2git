using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    //OH MY GOOOOSSSHH WASTED FUCKING UHHH 6h
    //The UI element which has this script NEEDS to be enabled. Check both this script checkbox enabled and the whole object!
    void Awake()
    {
        Invoke("DelayedAssignment", 0.1f);
    }

    void DelayedAssignment()
    {
        MainGameObject.Instance.interactText = this.gameObject;
        gameObject.SetActive(false);
    }
}
