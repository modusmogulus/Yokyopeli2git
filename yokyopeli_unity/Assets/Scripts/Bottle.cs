using UnityEngine;

public class Bottle : MonoBehaviour
{
    public bool interact = false;
    private bool isInRange = false;
    public string interactTextString = "'E'";
    public string soundToPlay = "SFX_Good";
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            if (!interact) {
                if (soundToPlay != "") { AudioManager.Instance.PlayAudio(soundToPlay);  }
                MainGameObject.Instance.bottles += 1;
                MainGameObject.Instance.setBottlesText();
                Destroy(gameObject);
            }
            else
            {
                MainGameObject.Instance.setInteractText(interactTextString);
                MainGameObject.Instance.interactText.SetActive(true);
                MainGameObject.Instance.bottles += 1;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        MainGameObject.Instance.interactText.SetActive(false);
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            MainGameObject.Instance.interactText.SetActive(false);
        }
    }
    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && interact)
        {
            if (soundToPlay != "") { AudioManager.Instance.PlayAudio(soundToPlay); }
        }
    }
}
