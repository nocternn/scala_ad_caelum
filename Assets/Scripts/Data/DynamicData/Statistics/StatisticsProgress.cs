using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatisticsProgress
{
    public const int MaxIterations = 5;
    public const int MaxStages = 5;

    public int iteration;
    public int stage;

    public void CopyFrom(StatisticsProgress progress)
    {
        iteration = progress.iteration;
        stage = progress.stage;
    }
}
