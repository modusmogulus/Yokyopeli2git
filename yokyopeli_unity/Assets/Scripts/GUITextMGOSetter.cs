using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GUItextMGOSetter : MonoBehaviour
{
    public TMPro.TMP_Text thistext;
    public string element_type = "bottlesText";
    //OH MY GOOOOSSSHH WASTED FUCKING UHHH 6h
    //The UI element which has this script NEEDS to be enabled. Check both this script checkbox enabled and the whole object!
    void Awake()
    {
        Invoke("DelayedAssignment", 0.1f);
    }

    void DelayedAssignment()
    {
        thistext = gameObject.GetComponent<TMPro.TMP_Text>();
        if (element_type == "storyText")
        {
            MainGameObject.Instance.storyTextBox = gameObject;

        }
        if (element_type == "interactText")
        {
            MainGameObject.Instance.interactText = gameObject;

        }
        if (element_type == "scoreText")
        {
            MainGameObject.Instance.moneyText = thistext;

        }
        if (element_type == "bottlesText")
        {
            MainGameObject.Instance.bottlesText = thistext;

        }
        if (element_type == "worthText")
        {
            MainGameObject.Instance.worthText = thistext;

        }
        if (element_type == "money")
        {
            MainGameObject.Instance.moneyText = thistext;

        }
    }
}
