using UnityEngine;

public class ActivatorTrigger : MonoBehaviour
{
    public GameObject gameObject;
    public bool activeOnEnter = true;
    public bool activeOnExit = true;
    public bool changeStateOnEnter = true;
    public bool changeStateOnExit = false;
    public bool destroyOnEnter = false;
    public bool interact = false;
    private bool isInRange = false;
    public string interactTextString = "'E'";
    public string soundToPlay = "";
    public float delay = 0f; // New variable for delay
    private bool delayInProgress = false;
    private float delayTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && changeStateOnEnter)
        {
            isInRange = true;

            if (!interact)
            {
                if (delay <= 0f)
                    ActivateObject();
                else
                {
                    delayInProgress = true;
                    delayTimer = 0f;
                }
            }
            else
            {
                MainGameObject.Instance.setInteractText(interactTextString);
                MainGameObject.Instance.interactText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MainGameObject.Instance.interactText.SetActive(false);
        if (other.CompareTag("Player") && changeStateOnExit)
        {
            isInRange = false;
            gameObject.SetActive(activeOnExit);
            MainGameObject.Instance.interactText.SetActive(false);
        }
    }

    private void Update()
    {
        if (delayInProgress)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delay)
            {
                delayInProgress = false;
                ActivateObject();
            }
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E) && interact)
        {
            if (soundToPlay != "") { AudioManager.Instance.PlayAudio(soundToPlay); }

            if (delay <= 0f)
                ActivateObject();
            else
            {
                delayInProgress = true;
                delayTimer = 0f;
            }
        }
    }

    private void ActivateObject()
    {
        gameObject.SetActive(activeOnEnter);
        if (destroyOnEnter) { Destroy(this); }
    }
}
