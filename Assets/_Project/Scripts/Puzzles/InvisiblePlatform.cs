using System.Collections;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    [SerializeField] private InvisiblePlatformPuzzle _platformPuzzle;
    [SerializeField] private float _disableDelay;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DisableDelay());
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(_disableDelay);
        _platformPuzzle.DisablePlatforms();
    }
}
