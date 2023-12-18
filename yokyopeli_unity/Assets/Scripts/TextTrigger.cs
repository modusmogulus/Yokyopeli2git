using UnityEngine;
using TMPro;








//------------------------------------- TEXT BOX HAS TO HAVE TMP_TEXT COMPONENT AND ANIMATION COMPONENT! ------------------------------

public class TextTrigger : MonoBehaviour
{
    public string newtext;
    public bool restartTextFade = true;
    public bool destroyOnEnter = false;
    public TypewriterEffect tpw;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainGameObject.Instance.storyTextBox.GetComponent<TMP_Text>().text = newtext;
            MainGameObject.Instance.storyTextBox.GetComponent<Animation>().Rewind();
            MainGameObject.Instance.storyTextBox.GetComponent<Animation>().Play();
            
            //tpw = MainGameObject.Instance.storyTextBox.GetComponent<TypewriterEffect>();
            //tpw.StartCoroutine(tpw.ShowText());
            if (destroyOnEnter) { Destroy(this); }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        MainGameObject.Instance.interactText.SetActive(false);
        if (other.CompareTag("Player"))
        {
            MainGameObject.Instance.interactText.SetActive(false);
        }
    }
}
