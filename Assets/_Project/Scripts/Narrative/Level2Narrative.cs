using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fungus;
using UnityEngine;
using UnityEngine.Playables;

public class Level2Narrative : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private GameObject _npc;
    [SerializeField] private GameObject _startTimelineEnemy;
    [SerializeField] private GameObject _swordSelection;
    [SerializeField] private PlayableDirector _startTimeline;
    [SerializeField] private PlayableDirector _npcLeavingTimeline;
    [SerializeField] private Animator _uiOverlayAnimator;

    private bool _willReceiveWeapon = false;

    public async System.Threading.Tasks.Task FinishStartDialog(bool willReceiveWeapon)
    {
        _willReceiveWeapon = willReceiveWeapon;
        _npcLeavingTimeline.Play();
        await System.Threading.Tasks.Task.Delay(1500);
        _uiOverlayAnimator.SetTrigger("Close");
        await System.Threading.Tasks.Task.Delay(1500);
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(_npc);
        Destroy(_startTimelineEnemy);
        GameManager.sInstance.GetInputListener().PauseInput(false);
    }

    public void OpenSwordSelection()
    {
        _swordSelection.SetActive(true);
    }

    public void SetSword()
    {
        _swordSelection.SetActive(false);
        _flowchart.SendFungusMessage("WeaponSelected");
    }

    public void LoadNextLevel(string scene)
    {
        GameManager.sInstance.GetSceneController().SetScene(scene);
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

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        _uiOverlayAnimator.SetTrigger("Open");
        GameManager.sInstance.GetInputListener().PauseInput(true);
        _flowchart.SendFungusMessage("Finish");
    }
}
