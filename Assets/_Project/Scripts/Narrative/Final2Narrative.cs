using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Playables;

public class Final2Narrative : MonoBehaviour
{
    [SerializeField] private Animator _screenFadeAnimator;

    private async void FinishFlowchart()
    {
        await System.Threading.Tasks.Task.Delay(12000);
        _screenFadeAnimator.SetTrigger("Fade");
        await System.Threading.Tasks.Task.Delay(3000);
        GameManager.sInstance.GetSceneController().SetScene("Menu");
    }

    private void Start()
    {
        FinishFlowchart();
    }
}
