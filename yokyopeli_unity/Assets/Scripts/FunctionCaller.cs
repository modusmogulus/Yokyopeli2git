using UnityEngine;
using System;
using System.Collections.Generic;

// Serializable class to hold function references
[Serializable]
public class FunctionInfo
{
    public GameObject targetObject; // Object containing the function
    public string functionName;     // Name of the function to call
    public bool value;
}

public class FunctionCaller : MonoBehaviour
{
    public List<FunctionInfo> functionsList = new List<FunctionInfo>(); // List of function references

    // Method to call functions from the list
    public void CallFunctions()
    {
        foreach (FunctionInfo info in functionsList)
        {

            if (info.targetObject != null)
            {
                // Find the method by its name and invoke it on the target object
                info.targetObject.SendMessage(info.functionName, info.value, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                
                Debug.LogWarning("Target object is null in the function reference.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PASIPISSA");
            CallFunctions();
        }
    }
}