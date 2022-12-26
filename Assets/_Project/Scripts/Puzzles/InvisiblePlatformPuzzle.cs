using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatformPuzzle : MonoBehaviour
{
    [SerializeField] private List<GameObject> _platformList;
    [SerializeField] private Material _disabledMaterial;
    [SerializeField] private Material _enabledMaterial;

    public void DisablePlatforms()
    {
        foreach(GameObject platform in _platformList)
        {
            platform.GetComponent<Collider>().enabled = false;
            platform.GetComponent<MeshRenderer>().material = _disabledMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnablePlatforms();
    }

    private void EnablePlatforms()
    {
        foreach(GameObject platform in _platformList)
        {
            platform.GetComponent<Collider>().enabled = true;
            platform.GetComponent<MeshRenderer>().material = _enabledMaterial;
        }
    }
}
