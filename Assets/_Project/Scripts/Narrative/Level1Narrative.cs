using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Level1Narrative : MonoBehaviour
{
    [SerializeField] private GameObject _finishFlowchart;
    [SerializeField] private Animator _uiOverlayAnimator;

    public void LoadNextLevel(string scene)
    {
        SceneController.SetScene(scene);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        _uiOverlayAnimator.SetTrigger("Open");
        _finishFlowchart.SetActive(true);
        GameManager.sInstance.GetInputListener().PauseInput(true);
    }
}
