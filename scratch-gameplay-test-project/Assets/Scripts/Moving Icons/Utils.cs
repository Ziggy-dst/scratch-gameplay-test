using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    public static string CalculateMultiProbability(List<IconItemSO.IconItem> probabilityList)
    {
        var sortedDistribution = probabilityList.OrderBy(x => x.probability);

        float rand = Random.value;
        float accumulatedProbability = 0;
        foreach (var d in sortedDistribution)
        {
            accumulatedProbability += d.probability;
            if (rand <= accumulatedProbability)
            {
                // Debug.Log($"accumulatedProbability: {accumulatedProbability}");
                return d.id;
            }
        }

        return default;
    }
}
