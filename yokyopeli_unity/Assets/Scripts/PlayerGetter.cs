using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerGetter : MonoBehaviour
{
    public GameObject player;
    public MainGameObject MGO;
    public Yarn.Unity.DialogueRunner dialogueRunner;
    public string dialogueNode;
    public void Start()
    {
        MGO = MainGameObject.Instance;
        player = MainGameObject.Instance.player;
    }

    public void PlayTypewriterSound()
    {
        AudioManager.Instance.PlayAudio("SFX_Typewriter");
    }

    public void HideUI()
    {
        MainGameObject.Instance.playerController.m_MouseLook.SetCursorLock(true);
    }
    public void ShowUI()
    {
        MainGameObject.Instance.playerController.m_MouseLook.SetCursorLock(false);
    }

    public void playDialogue(string dialogueNode)
    {
        if (dialogueNode != null)
        {
            dialogueRunner.StartDialogue(dialogueNode);
        }
    }
}
