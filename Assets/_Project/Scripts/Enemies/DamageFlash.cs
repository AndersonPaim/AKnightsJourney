using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Renderer _meshRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _flashMaterial;

    public void Flash()
    {
        StartCoroutine(FlashDelay());
    }

    private IEnumerator FlashDelay()
    {
        _meshRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(0.125f);
        _meshRenderer.material = _defaultMaterial;
    }

}