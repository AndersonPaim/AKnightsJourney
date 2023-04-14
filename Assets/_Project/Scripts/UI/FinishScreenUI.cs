using UnityEngine;
using TMPro;

public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _lifesText;

    public void Setup(float stars, float kills, float killsTarget, float coins, float coinsTarget, float lifes, float lifesTarget)
    {
        _killsText.SetText(kills.ToString() + "/" + killsTarget.ToString());
        _coinsText.SetText(coins.ToString() + "/" + coinsTarget.ToString());
        _lifesText.SetText(lifes.ToString() + "/" + lifesTarget.ToString());
    }
}