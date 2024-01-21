using UnityEngine;
using UnityEngine.UI;
using Q3Movement;

[RequireComponent(typeof(Button))]
public class SelectButtonByUhh : MonoBehaviour
{

	public KeyCode key;
	private Q3Movement.Q3PlayerController player;
	public Button button { get; private set; }
	public AGDS_Dialogue parentDialogue;
	Graphic targetGraphic;
	Color normalColor;
	public bool changeCursorLock = true;

	void Awake()
	{
		button = GetComponent<Button>();
		button.interactable = false;
		targetGraphic = GetComponent<Graphic>();

		ColorBlock cb = button.colors;
		cb.disabledColor = cb.normalColor;
		button.colors = cb;
		if (parentDialogue != null)
        {
			player = MainGameObject.Instance.playerController;
        }
	}

	void Start()
	{
		button.targetGraphic = null;
		Up();
		player = MainGameObject.Instance.playerController;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(key))
		{
			Down();
		}
		else if (Input.GetKeyUp(key))
		{
			Up();
		}
	}

	void Up()
	{
		StartColorTween(button.colors.normalColor, false);
	}

	void Down()
	{
		if (player != null && changeCursorLock == true) { print("HEI!");  player.m_MouseLook.SetCursorLock(true); }
		StartColorTween(button.colors.pressedColor, false);
		button.onClick.Invoke();
	}

	void StartColorTween(Color targetColor, bool instant)
	{
		if (targetGraphic == null)
			return;

		targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
	}

	void OnApplicationFocus(bool focus)
	{
		Up();
	}

	public void LogOnClick()
	{
		Debug.Log("LogOnClick() - " + GetComponentInChildren<Text>().text);
	}
}