using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using EasyTransition;
public class MainGameObject : MonoBehaviour
{

    public bool hasJob = false;
    public GameObject interactText;
    public TMPro.TMP_Text interactTextComponent;
    public GameObject storyTextBox;
    public TMPro.TMP_Text controlTips;
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text bottlesText;
    public TMPro.TMP_Text moneyText;
    public TMPro.TMP_Text worthText;
    public GameObject player;
    public float score;
    public int bottles = 0;
    public float money = 0;
    public float worth = 0;
    public bool s_reduceNausea = false;
    public TransitionSettings transition;
    public float loadDelay;
    public static MainGameObject Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void setJob(bool value)
    {
        hasJob = value;
    }

    public void setInteractText(string text)
    {
        if (interactText.GetComponent<TMP_Text>())
        {
            interactTextComponent = interactText.GetComponent<TMP_Text>();
        }
        interactTextComponent.text = text;
    }

    public void setBottlesText()
    {
        bottlesText.text = bottles.ToString();
        worthText.text = (bottles * 0.2).ToString();
    }

    public void changeScene(string scene)
    {
        //SceneManager.LoadScene(scene);
        TransitionManager.Instance().Transition(scene, transition, loadDelay);
    }

    public void setReduceNausea()
    {
        s_reduceNausea = !s_reduceNausea;
    }
    private void Start()
    {
        if (interactText.GetComponent<TMP_Text>()) {
            interactTextComponent = interactText.GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

}
