using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Playables;
using Fungus;
using TMPro;

public class TutorialNarrativeController : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialObject;
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private PlayableDirector _npcLeavingTimeline;
    [SerializeField] private Animator _uiOverlayAnimator;
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private Character _playerCharacter;

    private Vector3 _finishPos = new Vector3(133, 0.125f, 302);

    public async System.Threading.Tasks.Task FinishStartDialog()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _uiOverlayAnimator.SetTrigger("Close");
        _npcLeavingTimeline.Play();

        await System.Threading.Tasks.Task.Delay(3500);

        GameManager.sInstance.GetInputListener().PauseInput(false);
    }

    public void SetUsername()
    {
        SaveData data = SaveSystem.localData;
        data.username = _usernameInput.text;
        SaveSystem.Save();
        _playerCharacter.SetStandardText(_usernameInput.text);
    }

    public void DisableTutorial()
    {
        _tutorialObject.SetActive(false);
    }

    public void LoadNextLevel(string scene)
    {
        GameManager.sInstance.GetSceneController().SetScene(scene);
    }

    private void Start()
    {
        _flowchart.SendFungusMessage("Start");
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        _uiOverlayAnimator.SetTrigger("Open");
        _flowchart.SendFungusMessage("Finish");
        GameManager.sInstance.GetInputListener().PauseInput(true);
    }

}
