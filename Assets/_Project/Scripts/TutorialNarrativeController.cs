using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Playables;

public class TutorialNarrativeController : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialObject;
    [SerializeField] private GameObject _finishTutorialFlowchart;
    [SerializeField] private PlayableDirector _npcLeavingTimeline;

    private Vector3 _finishPos = new Vector3(133, 0.125f, 302);

    public async Task FinishStartDialog()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _npcLeavingTimeline.Play();

        await Task.Delay(3500);

        GameManager.sInstance.GetInputListener().PauseInput(false);
    }

    public void DisableTutorial()
    {
        _tutorialObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        _finishTutorialFlowchart.SetActive(true);
        GameManager.sInstance.GetInputListener().PauseInput(true);
    }

}
