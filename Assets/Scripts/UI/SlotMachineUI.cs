using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineUI : MonoBehaviour
{
    public GameManager gameManager;
    public int betAmount = 10;

    public TextMeshProUGUI currentBetText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI winningsText;

    public Button spinButton;
    public Button addButton;
    public Button deductButton;

    private void Awake()
    {
        addButton.onClick.AddListener(AddBet);
        deductButton.onClick.AddListener(DeductBet);
        spinButton.onClick.AddListener(Spin);
        coinsText.text = gameManager.coins.ToString();
        currentBetText.text = gameManager.currentBet.ToString();
    }

    void AddBet()
    {
        gameManager.AddBet(betAmount);
        currentBetText.text = gameManager.currentBet.ToString();
    }

    void DeductBet()
    { 
        gameManager.DeductBet(betAmount);
        currentBetText.text = gameManager.currentBet.ToString();
    }

    void Spin()
    {
        gameManager.SpinSlotMachine();
        winningsText.text = gameManager.winnings.ToString();
        coinsText.text = gameManager.coins.ToString();
    }

}
