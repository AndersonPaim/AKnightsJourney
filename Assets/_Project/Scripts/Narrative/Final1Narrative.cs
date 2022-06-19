using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Playables;

public class Final1Narrative : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private GameObject _startTimelineEnemy;
    [SerializeField] private PlayableDirector _startTimeline;
    [SerializeField] private Animator _uiOverlayAnimator;
    [SerializeField] private Animator _screenFadeAnimator;

    public async System.Threading.Tasks.Task FinishStartDialog(bool willReceiveWeapon)
    {
        await System.Threading.Tasks.Task.Delay(1500);
        _uiOverlayAnimator.SetTrigger("Close");
        await System.Threading.Tasks.Task.Delay(1500);
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(_startTimelineEnemy);
        GameManager.sInstance.GetInputListener().PauseInput(false);
    }

    public async void FinishFlowchart()
    {
        _flowchart.gameObject.SetActive(false);
        _screenFadeAnimator.SetTrigger("Fade");
        await System.Threading.Tasks.Task.Delay(6000);
        GameManager.sInstance.GetSceneController().SetScene("Menu");
    }

    private void Start()
    {
        StartCoroutine(EnableStartFlowchart());
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator EnableStartFlowchart()
    {
        yield return new WaitForSeconds((float)_startTimeline.duration);
        _flowchart.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
