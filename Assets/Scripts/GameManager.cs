using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coins = 4000;
    public int currentBet = 20;
    public int winnings;

    public PayoutLinesContainer payoutLinesContainer;
    public SlotMachine slotMachine;

    public void SpinSlotMachine()
    {
        if (CanSpin())
        {
            if (slotMachine.IsSpinning())
            {
                slotMachine.Spin();
                winnings = currentBet * slotMachine.payout;
                coins += winnings;
            }
            else
            {
                coins -= GetTotalBetValue(currentBet);
                slotMachine.PrepareResults();
                slotMachine.Spin();
            }
            
        }
    }

    bool CanSpin()
    {
        int totalBetValue = GetTotalBetValue(currentBet);
        if (coins > totalBetValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWinnings(int bet)
    {
        return bet * slotMachine.payout;
    }

    public int GetTotalBetValue(int bet)
    {
        return bet * payoutLinesContainer.payoutLines.Count;
    }

    public void AddBet(int amount)
    {
        currentBet += amount;
    }

    public void DeductBet(int amount)
    {
        currentBet -= amount;
        if (currentBet <= 0)
        {
            currentBet = 0;
        }
    }

}
