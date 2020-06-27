using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Payout Line Container", menuName = "ScriptableObjects/Line/Container", order = 1)]
public class PayoutLinesContainer : ScriptableObject
{
    public List<PayoutLine> payoutLines;
}
