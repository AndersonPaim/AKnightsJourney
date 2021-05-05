using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;

    private int _heartIndex;

    private Color _blackColor;

    private void Start()
    {
        Initialize();
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void Initialize()
    {
        _heartIndex = _hearts.Length - 1;
        _blackColor = new Color(0, 0, 0);
    }

    private void SetupDelegates()
    {
        GameManager.sInstance.playerController.OnDeath += PlayerDie;

    }

    private void RemoveDelegates()
    {
        GameManager.sInstance.playerController.OnDeath -= PlayerDie;
    }

    private void PlayerDie()
    {
        _hearts[_heartIndex].color = _blackColor;          
        _heartIndex--;
    }
}
