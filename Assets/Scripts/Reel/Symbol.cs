using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
