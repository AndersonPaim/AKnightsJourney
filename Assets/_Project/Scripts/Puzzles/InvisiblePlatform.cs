using System.Collections;
using UnityEngine;
using DG.Tweening;

public class InvisiblePlatform : MonoBehaviour
{
    [SerializeField] private InvisiblePlatformPuzzle _platformPuzzle;
    [SerializeField] private float _upPosition;
    [SerializeField] private float _downPosition;
    [SerializeField] private float _disableDelay;

    public void EnablePlatform()
    {
        transform.DOLocalMoveY(_upPosition, 1);
    }

    public void DisablePlatform()
    {
        transform.DOLocalMoveY(_downPosition, 1);
    }

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
        transform.DOShakePosition(_disableDelay);
        yield return new WaitForSeconds(_disableDelay);
        _platformPuzzle.DisablePlatforms();
    }
}
