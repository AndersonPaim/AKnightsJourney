using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Narrative : MonoBehaviour
{
    [SerializeField] private Animator _uiOverlayAnimator;

    private void Start()
    {
        _uiOverlayAnimator.SetTrigger("Close");
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.sInstance.GetSceneController().SetScene("Level6");
    }
}
