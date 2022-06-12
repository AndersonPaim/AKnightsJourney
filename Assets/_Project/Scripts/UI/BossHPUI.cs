using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [SerializeField] private BeeBoss _boss;
    [SerializeField] private Slider _hpSlider;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _boss.OnUpdateBossHP += UpdateHPSlider;
        _boss.OnStartBossHP += StartHPSlider;
    }

    private void RemoveDelegates()
    {
        _boss.OnUpdateBossHP -= UpdateHPSlider;
        _boss.OnStartBossHP -= StartHPSlider;
    }

    private void StartHPSlider(float hp)
    {
        _hpSlider.maxValue = hp;
        _hpSlider.value = hp;
    }

    private void UpdateHPSlider(float hp)
    {
        _hpSlider.value = hp;
    }
}
