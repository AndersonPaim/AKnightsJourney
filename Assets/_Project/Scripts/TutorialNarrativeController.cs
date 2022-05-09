using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNarrativeController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _inputManager;
    [SerializeField] private GameObject _finishTutorialFlowchart;
    private Vector3 _finishPos = new Vector3(133, 0.125f, 302);
    private Animator _playerAnim;

    private void Start()
    {
        _playerAnim = _player.GetComponent<Animator>();
    }

    public void FinishStartDialog()
    {
        _inputManager.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.sInstance.GetInputListener().PauseInput(false);
    }

    private void FinishTutorial()
    {
        _playerAnim.Play("Player@Idle");
        _finishTutorialFlowchart.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerAnim.Play("Player@Idle");
        Cursor.lockState = CursorLockMode.None;
        _inputManager.SetActive(false);
    }

}
