using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Symbol Data", menuName = "ScriptableObjects/Symbol/Symbol Data", order = 2)]
public class SymbolData : ScriptableObject
{
    [SerializeField]
    private int id = 0;
    public string symbolName;
    public int[] payout = new int[5];
    public Sprite sprite;

    public int GetId()
    {
        return id;
    }
}
