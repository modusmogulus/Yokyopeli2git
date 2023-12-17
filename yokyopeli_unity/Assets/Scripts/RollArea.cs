using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollArea : MonoBehaviour
{
    private Q3Movement.Q3PlayerController controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller = other.GetComponent<Q3Movement.Q3PlayerController>();
            controller.m_inRollZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.m_inRollZone = false;
        }
    }
}
