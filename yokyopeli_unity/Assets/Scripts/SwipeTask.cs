using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwipeTask : MonoBehaviour
{
    public List<SwipePoint> _swipePoints = new List<SwipePoint>();
    public float _countdownMax = 0.5f; public GameObject _greenOn;
    public GameObject _redOn; private int _currentSwipePointIndex = 0;
    private float _countdown = 0;
    public GameObject door;
    public string BadSound = "SFX_Pasi";
    public string GoodSound = "SFX_Coin";

    private void Update()
    {
        _countdown -= Time.deltaTime;

        if (_currentSwipePointIndex != 0 && _countdown <= 0)
        {
            _currentSwipePointIndex = 0;
            StartCoroutine(FinishTask(false));
        }
    }
    private IEnumerator FinishTask(bool wasSuccessful)
    {
        if (wasSuccessful)
        {
            _greenOn.SetActive(true);
            if (door != null) { door.SetActive(false); }
            AudioManager.Instance.PlayAudio(GoodSound);
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
        else
        {
            _redOn.SetActive(true);
            AudioManager.Instance.PlayAudio(BadSound);
        }

        yield return new WaitForSeconds(1.5f);
        _greenOn.SetActive(false);
        _redOn.SetActive(false);
    }
    public void SwipePointTrigger(SwipePoint swipePoint)
    {
        if (swipePoint == _swipePoints[_currentSwipePointIndex])
        {
            _currentSwipePointIndex++;
            _countdown = _countdownMax;
        }
        if (_currentSwipePointIndex >= _swipePoints.Count)
        {
            _currentSwipePointIndex = 0;
            StartCoroutine(FinishTask(true));
        }
    }
}