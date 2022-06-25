using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Playables;

public class Level1Narrative : MonoBehaviour
{
    [SerializeField] private GameObject _map;
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private GameObject _avelynNpc;
    [SerializeField] private GameObject _lewisNpc;
    [SerializeField] private Animator _uiOverlayAnimator;
    [SerializeField] private float _avelynTime;

    private float _time = 0;
    private bool _canCountTime = true;

    public void LoadNextLevel(string scene)
    {
        GameManager.sInstance.GetSceneController().SetScene(scene);
    }

    public void OpenMap()
    {
        _map.SetActive(true);
    }

    private void OnEnable()
    {
        _time = 0;
    }

    private void Update()
    {
        if(_time <= _avelynTime && _canCountTime)
        {
            _time += Time.deltaTime;

            if(_time >= _avelynTime)
            {
                _lewisNpc.SetActive(true);
                _avelynNpc.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _canCountTime = false;
        GameManager.sInstance.GetInputListener().PauseInput(true);
        Cursor.lockState = CursorLockMode.None;
        _uiOverlayAnimator.SetTrigger("Open");

        if(_time < _avelynTime)
        {
            _flowchart.SendFungusMessage("Avelyn");
        }
        else
        {
            _flowchart.SendFungusMessage("Lewis");
        }
    }
}
