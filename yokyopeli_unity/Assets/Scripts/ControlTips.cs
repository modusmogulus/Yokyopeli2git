using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTips : MonoBehaviour
{
    void Start()
    {
        MainGameObject.Instance.controlTips = this.gameObject.GetComponent<TMPro.TMP_Text>();
    }
}
