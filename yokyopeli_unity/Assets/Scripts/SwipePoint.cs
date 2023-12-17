using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SwipePoint : MonoBehaviour
{

    private SwipeTask _swipeTask;

    private void Awake()
    {
        _swipeTask = GetComponentInParent<SwipeTask>();
        if (_swipeTask) { print("Swipe task found"); }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Triggered");
        _swipeTask.SwipePointTrigger(this);
    }
}