using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string soundToPlay = "";
    public bool destroyOnEnter = true;
    public bool playOnStart = false;
    private void Start()
    {
        if (playOnStart) {
            if (soundToPlay != "") { AudioManager.Instance.PlayAudio(soundToPlay); }
            if (destroyOnEnter) { Destroy(this); }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (soundToPlay != "") { AudioManager.Instance.PlayAudio(soundToPlay); }
            if (destroyOnEnter) { Destroy(this); }
        }
    }
}
