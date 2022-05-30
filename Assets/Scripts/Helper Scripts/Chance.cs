using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores a chance of something happening between zero and ten
/// </summary>
[System.Serializable]
public class Chance
{
    [Range(0, 10)]
    public int chance;

    /// <summary>
    /// sets the chance of something happening, between zero and ten
    /// </summary>
    public Chance(int chance)
    {
        this.chance = chance;
    }

    /// <summary>
    /// Tries the chance
    /// </summary>
    /// <returns>true or false</returns>
    public bool Try()
    {
        int result = Random.Range(1, 11);
        return result <= chance;
    }

    /// <summary>
    /// picks an item from a list with different weights
    /// </summary>
    /// <param name="chances">The list of chance values</param>
    /// <returns>The index of the chance value picked</returns>
    public static int Try(List<int> chances)
    {
        int totalChance = 0;
        foreach (int chance in chances)
        {
            totalChance += chance;
        }

        int result = Random.Range(1, totalChance + 1);

        int usedChance = 0;
        for (int i = 0; i < chances.Count; i++)
        {
            if (chances[i] + usedChance >= result)
            {
                return i;
            }
            else
            {
                usedChance += chances[i];
            }
        }
        Debug.LogError("Chance fell through, which should not be possible");
        return 0;
    }


    /// <summary>
    /// A random direction
    /// </summary>
    /// <returns></returns>
    public static int RandomDirection()
    {
        return (int)Mathf.Sign(Random.Range(-1, 1));
    }
}
