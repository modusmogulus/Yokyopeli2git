using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class yk_transition_manager : MonoBehaviour
{
    public TransitionSettings transition;
    public float loadDelay;

    public void LoadScene(string _sceneName)
    {
        TransitionManager.Instance().Transition(_sceneName, transition, loadDelay);
    }
}
