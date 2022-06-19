using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private List<Renderer> _meshRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _flashMaterial;
    public void Flash()
    {
        StartCoroutine(FlashDelay());
    }

    private IEnumerator FlashDelay()
    {
        foreach(Renderer mesh in _meshRenderer)
        {
            mesh.material = _flashMaterial;
        }

        yield return new WaitForSeconds(0.125f);

        foreach(Renderer mesh in _meshRenderer)
        {
            mesh.material = _defaultMaterial;
        }
    }
}