using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUITriggerButton : MonoBehaviour
{
	private Button btn;
	public string sound;
	public bool updateHasJob;
	public bool hasJob;
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		AudioManager.Instance.PlayAudio(sound);
		if (updateHasJob)
        {
			MainGameObject.Instance.hasJob = hasJob;
        }
	}
}
