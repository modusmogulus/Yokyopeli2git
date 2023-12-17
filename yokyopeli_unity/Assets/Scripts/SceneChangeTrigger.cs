using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public bool changeSceneOnlyIfHasJob = true;
    public TransitionSettings transition;
    public float loadDelay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MainGameObject.Instance.interactText.SetActive(true);
            if (!changeSceneOnlyIfHasJob || (changeSceneOnlyIfHasJob && MainGameObject.Instance.hasJob))
            {
                //SceneManager.LoadScene(sceneToLoad);
                TransitionManager.Instance().Transition(sceneToLoad, transition, loadDelay);
            }
            else
            {
                Debug.Log("Cannot change scene: 'hasJob' is false.");

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //MainGameObject.Instance.interactText.SetActive(false);
        if (other.CompareTag("Player"))
        {
            MainGameObject.Instance.interactText.SetActive(false);
        }
    }
}
