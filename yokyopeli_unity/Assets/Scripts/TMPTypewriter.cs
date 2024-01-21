using System.Collections;
using TMPro;
using UnityEngine;
public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f;
    private string fullText;

    private TMP_Text textComponent;
    private string currentText = "";

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        fullText = textComponent.text;
    }

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    public IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            AudioManager.Instance.PlayAudio("SFX_Typewriter");
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}