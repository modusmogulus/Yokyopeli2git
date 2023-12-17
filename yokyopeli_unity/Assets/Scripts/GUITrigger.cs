using UnityEngine;
using UnityEngine.UI;
using Q3Movement;

public class UIOnPlayerEnter : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject indicator;
    private bool isInRange = false;
    private bool isUIVisible = false;
    private GameObject player;
    private Q3PlayerController playerController;
    private string interactTextString = "'E'";
    public KeyCode keything = KeyCode.E;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keything == KeyCode.None) { keything = KeyCode.E; }
            MainGameObject.Instance.interactText.SetActive(true);
            if (indicator != null)
            {
                indicator.SetActive(true);
            }
            isInRange = true;
            player = other.gameObject;
            playerController = player.GetComponent<Q3PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainGameObject.Instance.interactText.SetActive(false);
            if (indicator != null) {
                MainGameObject.Instance.setInteractText(interactTextString);
                indicator.SetActive(false);
            }
            isInRange = false;
            HideUI();
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(keything))
        {
            if (isUIVisible)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
    }

    private void ShowUI()
    {
        
        uiPanel.SetActive(true);
        isUIVisible = true;

        if (playerController != null)
        {
            playerController.m_MouseLook.SetCursorLock(false);
        }
    }

    private void HideUI()
    {
        uiPanel.SetActive(false);
        isUIVisible = false;

        if (playerController != null)
        {
            playerController.m_MouseLook.SetCursorLock(true);
        }
    }
}
