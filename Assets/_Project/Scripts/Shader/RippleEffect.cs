using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] private ScriptableRendererFeature _rendererFeature;
    [SerializeField] private Camera _cam;
    [SerializeField] private Material _rippleMat;

    private float _timeProgress = 0;

    private void Update()
    {
        _timeProgress += Time.deltaTime;
        _rippleMat.SetFloat("_Progress", _timeProgress);
    }

    public void StartRippleEffect(Vector3 position)
    {
        _timeProgress = 0;
        _rendererFeature.SetActive(true);
        Vector3 worldPoint = _cam.WorldToScreenPoint(position);
        //TODO GET RESOLUTION
        Vector2 pos = new Vector2(worldPoint.x / 1920, worldPoint.y / 1080);
        _rippleMat.SetVector("_FocalPoint", pos);
    }

    public void StopRippleEffect()
    {
        _rendererFeature.SetActive(false);
    }
}
