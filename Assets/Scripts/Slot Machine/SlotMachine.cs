using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public int x = 5;
    public int y = 3;
    public int payout = 0;

    public int reelRows = 10;
    public GameObject[] reels;

    public SymbolsContainer symbolsContainer;
    public PayoutLinesContainer payoutLinesContainer;


    public SymbolData[,] results;

    public delegate void OnComplete();
    public event OnComplete onComplete;

    private void Awake()
    {
        results = new SymbolData[x, y];
    }

    private void Start()
    {
        InitializeReels();
    }


    public void PrepareResults()
    {
        payout = 0;
        PopulateGrid(symbolsContainer.symbols.Count);

        for (int i = 0; i < payoutLinesContainer.payoutLines.Count; i++)
        {
            SymbolData[] lineResult = payoutLinesContainer.payoutLines[i].GetPayoutLineResults(results);
            payout += GetPayout(GetStreak(lineResult));
        }
        SetReels();
        Debug.Log("Total Payout: " + payout);
    }

    public void Spin()
    {

        SpinReels();
        onComplete?.Invoke();
    }


    public void PopulateGrid(int symbolCount)
    {
        for (int y = 0; y < this.y; y++)
        {
            for (int x = 0; x < this.x; x++)
            {
                results[x, y] = symbolsContainer.symbols[Random.Range(0, symbolsContainer.symbols.Count)];
            }
        }
    }

    public List<SymbolData> GetStreak(SymbolData[] lineResult)
    {
        List<int> symbolIndexes = new List<int>();
        List<SymbolData> symbolStreak = new List<SymbolData>();

        SymbolData head = lineResult[0];
        symbolStreak.Add(head);
        SymbolData next = null;
        SymbolData prev = null;

        for (int i = 0; i < lineResult.Length; i++)
        {
            if (i - 1 > -1)
                prev = lineResult[i];
            if (i + 1 < lineResult.Length)
                next = lineResult[i + 1];

            if (head == next)
            {
                symbolStreak.Add(next);
                //Debug.Log(head.symbolName + " " + next.symbolName);
            }
            else if (head != next & symbolStreak.Count >= 3)
            {
                break;
            }
            //Set new head and find streak
            else if (head != next && symbolStreak.Count < 3)
            {
                symbolStreak.Clear();
                head = next;
                symbolStreak.Add(head);
            }

            //If next is length then break
            if (i + 1 == lineResult.Length - 1)
            {
                break;
            }
        }

        if (symbolStreak.Count >= 3)
        {
            return symbolStreak;
        }
        else
        {
            return null;
        }
     
    }

    int GetPayout(List<SymbolData> streak)
    {
        if (streak != null)
        {
            Debug.Log(streak[0].symbolName + " " + streak[0].payout[streak.Count - 1]);
            return streak[0].payout[streak.Count - 1];
        }
        return 0;
    }

    public void SetReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            SymbolData[] reelSymbols = new SymbolData[10];

            //Set the first 3 symbols with the results
            for (int y = 0; y < this.y; y++)
            {
                reelSymbols[y] = (results[i, y]);
            }
            //Random Symbols
            for (int w = 3; w < reelSymbols.Length; w++)
            {
                reelSymbols[w] = (symbolsContainer.symbols[Random.Range(0, 9)]);
            }


            reels[i].GetComponent<Reel>().SetSymbols(reelSymbols);
        }
    }

    public void SpinReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            Reel reel = reels[i].GetComponent<Reel>();
            if (!reel.stop)
            {
                reel.stop = true;
            }
            else
            {
                reel.stop = false;
            }
        }
    }

    public bool IsSpinning()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i].GetComponent<Reel>().isSpinning == true)
            {
                return true;
            }
        }
        return false;
    }

    public void InitializeReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].GetComponent<Reel>().Initialize(10, symbolsContainer.symbols);
        }
    }
}
