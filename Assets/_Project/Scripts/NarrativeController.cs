using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeController : MonoBehaviour
{
    [SerializeField] private GameObject _cursorVisibility;

    private void Start()
    {
        _cursorVisibility.SetActive(true);
    }

    public void FinishStartDialog()
    {
        _cursorVisibility.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.sInstance.GetInputListener().PauseInput(false);
    }
}
