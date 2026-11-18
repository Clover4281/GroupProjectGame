using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // updating coin count in ui
    void Update()
    {
        coinText.text = "Coin Count: " + coinCount.ToString();
    }
}
