using TMPro;
using UnityEngine;

namespace SlotLogic.UI
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text CoinText;
        private int currentCoin;

        private void Start()
        {
            currentCoin = 0;
            CoinText.text = currentCoin.ToString();
        }

        public void IncreaseCoin(int value)
        {
            currentCoin += value;
            CoinText.text = currentCoin.ToString();
        }
    }
}