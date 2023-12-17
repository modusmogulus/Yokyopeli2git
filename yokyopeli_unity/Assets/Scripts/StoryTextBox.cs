using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTextBox : MonoBehaviour
{
    void Start()
    {
        MainGameObject.Instance.storyTextBox = this.gameObject; //Other code is in the TextTrigger.cs and GameObject
    }
}
