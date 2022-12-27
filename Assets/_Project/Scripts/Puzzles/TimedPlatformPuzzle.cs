using System.Collections;
using UnityEngine;

public class TimedPlatformPuzzle : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _meshRenderer;
    [SerializeField] private Material _enableMaterial;
    [SerializeField] private Material _disableMaterial;
    [SerializeField] private bool _startState;
    [SerializeField] private float _changeStateDelay;

    private void Start()
    {
        if(_startState)
        {
            StartCoroutine(DisablePlatform());
        }
        else
        {
            StartCoroutine(EnablePlatform());
        }
    }

    private IEnumerator EnablePlatform()
    {
        yield return new WaitForSeconds(_changeStateDelay);
        _meshRenderer.material = _enableMaterial;
        _collider.enabled = true;
        StartCoroutine(DisablePlatform());
    }

    private IEnumerator DisablePlatform()
    {
        yield return new WaitForSeconds(_changeStateDelay);
        _meshRenderer.material = _disableMaterial;
        _collider.enabled = false;
        StartCoroutine(EnablePlatform());
    }
}
