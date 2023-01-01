using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisualEffects : MonoBehaviour
{
    public static VisualEffects sInstance;

    [SerializeField] private ScriptableRendererFeature _rendererFeature;
    [SerializeField] private Camera _cam;
    [SerializeField] private Material _rippleMat;
    [SerializeField] private Animator _camAnimator;

    private float _timeProgress = 0;

    private void Awake()
    {
        if(sInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            sInstance = this;
        }
    }

    private void Update()
    {
        _timeProgress += Time.deltaTime;
        _rippleMat.SetFloat("_Progress", _timeProgress);
    }

    public void CameraShake()
    {
        _camAnimator.SetTrigger("Shake");
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
