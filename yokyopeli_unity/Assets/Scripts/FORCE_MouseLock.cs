using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FORCE_MouseLock : MonoBehaviour
{
    public bool lockCursor = false;
    void Update()
    {
        MainGameObject.Instance.playerController.m_MouseLook.SetCursorLock(lockCursor);
    }

    private void OnDisable()
    {
        MainGameObject.Instance.playerController.m_MouseLook.SetCursorLock(true);
    }
}
