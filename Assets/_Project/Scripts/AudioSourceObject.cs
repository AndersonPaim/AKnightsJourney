using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    private AudioClip _audioClip;

    public void Disable()
    {
        _audioClip = GetComponent<AudioSource>().clip;
        StartCoroutine(DisableObject(_audioClip.length));
    }

    private IEnumerator DisableObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

}
