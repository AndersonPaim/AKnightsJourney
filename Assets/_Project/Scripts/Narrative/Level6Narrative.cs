using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class Level6Narrative : MonoBehaviour
{
    [SerializeField] private Flowchart _flowchart;

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
