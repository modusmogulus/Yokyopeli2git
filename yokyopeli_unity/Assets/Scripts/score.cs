using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    void Start()
    {
        MainGameObject.Instance.scoreText = this.gameObject.GetComponent<TMPro.TMP_Text>();
    }
}
