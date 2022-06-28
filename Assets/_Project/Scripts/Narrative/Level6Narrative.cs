using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fungus;
using UnityEngine;

public class Level6Narrative : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;
    [SerializeField] private Animator _screenFade;

    public IEnumerator LoadFinalScene(string scene)
    {
        _screenFade.SetTrigger("Fade2");
        yield return new WaitForSeconds(3);
        GameManager.sInstance.GetScoreManager().SaveCoins();
        GameManager.sInstance.GetSceneController().SetScene(scene);
    }

    private void Start()
    {
        StartCoroutine(StartNarrative());
    }

    private IEnumerator StartNarrative()
    {
        yield return new WaitForSeconds(2.5f);
        _flowchart.SendFungusMessage("Start");
    }
}
