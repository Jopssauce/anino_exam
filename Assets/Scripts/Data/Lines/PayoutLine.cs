using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Payout Line", menuName = "ScriptableObjects/Line/Payout Line", order = 2)]
public class PayoutLine : ScriptableObject
{
    public int[] line = new int[5];

    //Returns Symbols from Payout Line
    public SymbolData[] GetPayoutLineResults(SymbolData[,] results)
    {
        SymbolData[] lineResult = new SymbolData[5];

        for (int i = 0; i < line.Length; i++)
        {
            int y = line[i];
            int x = i;
            lineResult[i] = results[x, y];
        }

        return lineResult;
    }

}
