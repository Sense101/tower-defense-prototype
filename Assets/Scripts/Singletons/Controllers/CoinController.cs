using UnityEngine;
using UnityEngine.Events;

public class CoinController : Singleton<CoinController>
{
    // set in inspector

    public UnityEvent<int> onCoinsChanged = new UnityEvent<int>();

    [SerializeField] private int _coins = 0; // serialized for debugging purposes

    // references
    MainInterface _mainInterface;

    private void Start()
    {
        _mainInterface = MainInterface.Instance;
        UpdateCoinText(_coins);
        onCoinsChanged.AddListener(UpdateCoinText);
    }

    public int GetCoins()
    {
        return _coins;
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        onCoinsChanged.Invoke(_coins);
    }

    public bool CanAfford(int amount)
    {
        return _coins >= amount;
    }

    public bool TrySpendCoins(int amount)
    {
        if (CanAfford(amount))
        {
            _coins -= amount;
            onCoinsChanged.Invoke(_coins);

            return true;
        }

        return false;
    }

    private void UpdateCoinText(int coins)
    {
        _mainInterface.coinText.text = coins.ToString();
    }
}
