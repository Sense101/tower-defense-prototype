using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyContainer : MonoBehaviour
{
    // set in inspector
    [SerializeField] TextMeshProUGUI amountText;
    public Image enemyImage;

    // variables
    public EnemyInfo currentInfo;
    private int currentAmount;

    public void Clear()
    {
        currentInfo = null;
        currentAmount = 0;
    }

    public void AddAmount(int amount)
    {
        currentAmount += amount;
        amountText.text = currentAmount.ToString();
    }
}
