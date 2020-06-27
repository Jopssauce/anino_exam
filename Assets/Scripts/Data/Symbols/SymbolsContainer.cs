using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Symbol Data Container", menuName = "ScriptableObjects/Symbol/Container", order = 1)]
public class SymbolsContainer : ScriptableObject
{
    public List<SymbolData> symbols;
}
