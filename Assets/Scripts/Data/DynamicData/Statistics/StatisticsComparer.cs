using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsComparer : IComparer
{
    public int Compare(object StatsPlayer1, object StatsPlayer2)
    {
        return(new CaseInsensitiveComparer()).Compare(
            ((Statistics)StatsPlayer1).meta.numberOfLocalBattlesWon,
            ((Statistics)StatsPlayer2).meta.numberOfLocalBattlesWon);
    }
}
