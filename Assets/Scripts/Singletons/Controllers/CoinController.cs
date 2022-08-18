using UnityEngine;

public class CoinController : Singleton<CoinController>
{
    private int _coins = 0;
    private void Start()
    {

    }

    private void Update()
    {

    }

    public void AddCoins(int amount)
    {
        _coins += amount;
    }

    public void TrySpendCoins(int amount)
    {

    }
}
