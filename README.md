# anino_exam (John Joseph Asuncion)
**This project uses Unity 2019.3.0f6**
# Documentation

#### Slot Machine Specs
- Size: 5 reels, 3 rows
- Payout lines: *10*

Unfortunately for the specs I could only come up with 10 lines but the code is flexible enough to support any number of lines.

## Symbols
There are 3 classes that make up the Symbols. Symbol Data, Symbols Container and Symbol.

#### Symbol Data
For Symbols I've chosen to go with scriptable objects as my choice of editing and serializing data. Unity's SO's allow for quick prototyping and editing of data in the editor. 

```
[CreateAssetMenu(fileName = "Symbol Data", menuName = "ScriptableObjects/Symbol/Symbol Data", order = 2)]
public class SymbolData : ScriptableObject
{
    private int id = 0;
    public string symbolName;
    public int[] payout = new int[5];
    public Sprite sprite;

    public int GetId()
    {
        return id;
    }
}
```
#### Symbol Container
This class is scriptable object that merely acts as a list of all my symbols that I can access from any class with ease. The container class allows me to put any number of elements without having to update any part of the code that relies on it.

#### Symbol 
Symbol is a monobehaviour class that makes use of the Symbol Data Class to Initialize itself the information acquired from the Scriptable Object.
```
public class Symbol : MonoBehaviour
{
    public SymbolData symbolData;
    public SpriteRenderer spriteRenderer;

    public void Initialize(SymbolData symbolData)
    {
        this.symbolData = symbolData;
        spriteRenderer.sprite = symbolData.sprite;
    }
}
```

## Lines
The payout lines follow almost exactly the same framework as the symbols above. This consists of only two classes, Payout Line and Payout Lines Container

#### Payout Line 
Since lines have been used exclusively as a reference for results it was best for me to save them as a Scriptable Object.
```
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
```

#### Payout Lines Container
This class is the exact same as the symbols container. It allows me ease of access to the payoutlines anywhere including the UI.

## Reels
There are a total of 5 reels with each their own symbols and animations that land on the predetermined results before the spin.
Reel Data is Initialized using the Initialize function in the class

```
public void Initialize(int rows, List<SymbolData> symbols)
    {
        for (int i = 0; i < rows; i++)
        {
            GameObject symbolObject = Instantiate(symbolPrefab, reelContent.transform.position, reelContent.transform.rotation, reelContent.transform);
        }

        for (int i = 0; i < reelContent.transform.childCount; i++)
        {
            children.Add(reelContent.transform.GetChild(i).gameObject);
        }

        children[0].transform.position = new Vector3(transform.position.x, 0.5f, 0);
        for (int i = 0; i < children.Count; i++)
        {
            if (i + 1 < children.Count)
            {
                children[i + 1].transform.position = children[i].transform.position - new Vector3(0, 0.5f, 0);
            }
        }
        children[children.Count - 1].transform.position = new Vector3(transform.position.x, 1f, 0);

        SetSymbols(symbols.ToArray());

        targetSymbol = children[0];
    }
```

And I can replace symbol data with the Set Symbols function.
```
public void SetSymbols(SymbolData[] symbols)
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponent<Symbol>().Initialize(symbols[i]);
        }
    }
```
## Reel Spin
For Spinning the Reels and generating the results of the reels I make use of a class called Slot Machine which handles these things for me.
This function in slot machine populates the 2D array with symbols ID that I can later use with the payout lines to determine the payout.
```
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
```
Then I have this function the calls on all 5 reels from a list to toggle its spinning on or off
```
    public void Spin()
    {
        SpinReels();
        onComplete?.Invoke();
    }

```
Now going back to the Reels Class this block of code is how I decided to go about animating the spin. I can probably improve this by using something similar to lerp to lock 
the symbols movement in a timestep but I was spending too much time on this feauture that I've gone for a dirty implementation instead. I also did not have time to solve problems that may occur in screen space, anchored positions etc, which is why the reels are not part of the UI.
```
public void Spin()
    {
        GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (!stop)
            {
                children[i].transform.position += Vector3.down * 4f * Time.deltaTime;
                isSpinning = true;
            }
            else
            {
                if (children[0].GetComponent<Symbol>().symbolData.GetId() == targetSymbol.GetComponent<Symbol>().symbolData.GetId())
                {
                    isSpinning = false;
                    return;
                }
                children[i].transform.position += Vector3.down * 4f * Time.deltaTime;
            }

            if (children[i].transform.position.y < minHeight)
            {
                children[i].transform.position = new Vector3(children[i].transform.position.x, maxHeight, 0);
            }

            if (children[i].transform.position.y >= 0.5f && children[i].transform.position.y <= 0.6f)
            {
                children[i].transform.SetAsFirstSibling();
            }

        }
    }
```
## UI
Normally I would have the UI in a seperate scene that can be accessed with a singleton but I thought that a singleton would be overkill for this project and decided against it.
Instead what I've done is go for the traditinal Game Manager Class which contains all the methods that the UI may request from the game.
So the Game Manager would have these methods.
```
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
```
While the UI would simply look clean like this. Where all the important UI Elements can be found to get assigned to.
```
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
```
## Build
[Game Build](https://drive.google.com/file/d/1vQV8o0mvpPaUc5tXtVEp9JcA_zopw2ek/view?usp=sharing)
