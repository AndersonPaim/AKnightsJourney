using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Level1Narrative : MonoBehaviour
{
    [SerializeField] private GameObject _map;
    [SerializeField] private GameObject _finishFlowchart;
    [SerializeField] private GameObject _finishNpc;
    [SerializeField] private Animator _uiOverlayAnimator;

    public void LoadNextLevel(string scene)
    {
        SceneController.SetScene(scene);
    }

    public void OpenMap()
    {
        _map.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        _uiOverlayAnimator.SetTrigger("Open");
        _finishFlowchart.SetActive(true);
        _finishNpc.SetActive(true);
        GameManager.sInstance.GetInputListener().PauseInput(true);
    }
}
