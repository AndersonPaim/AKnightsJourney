using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

}
