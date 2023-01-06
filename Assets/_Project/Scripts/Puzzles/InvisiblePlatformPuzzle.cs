using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatformPuzzle : MonoBehaviour
{
    [SerializeField] private List<InvisiblePlatform> _platformList;

    public void DisablePlatforms()
    {
        foreach(InvisiblePlatform platform in _platformList)
        {
            platform.DisablePlatform();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnablePlatforms();
    }

    private void EnablePlatforms()
    {
        foreach(InvisiblePlatform platform in _platformList)
        {
            platform.EnablePlatform();
        }
    }
}
