using UnityEngine;
using TMPro;

public class FinishScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _lifesText;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    public void Setup(float stars, float kills, float killsTarget, float coins, float coinsTarget, float lifes, float lifesTarget)
    {
        _killsText.SetText(kills.ToString() + "/" + killsTarget.ToString());
        _coinsText.SetText(coins.ToString() + "/" + coinsTarget.ToString());
        _lifesText.SetText(lifes.ToString() + "/" + lifesTarget.ToString());

        if(stars > 0)
        {
            _star1.SetActive(true);
        }
        if(stars > 1)
        {
            _star2.SetActive(true);
        }
        if(stars > 2)
        {
            _star3.SetActive(true);
        }
    }
}