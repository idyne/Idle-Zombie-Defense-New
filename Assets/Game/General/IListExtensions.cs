using System;
using System.Collections.Generic;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    public static int IndexOfMin(this IList<int> self, int startIndexInclusive, int endIndexInclusive)
    {
        if (self == null)
        {
            throw new ArgumentNullException("self");
        }

        if (self.Count == 0)
        {
            throw new ArgumentException("List is empty.", "self");
        }

        int min = self[startIndexInclusive];
        int minIndex = startIndexInclusive;

        for (int i = startIndexInclusive + 1; i <= endIndexInclusive; ++i)
        {
            if (self[i] < min)
            {
                min = self[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public static int IndexOfMax(this IList<int> self, int startIndexInclusive, int endIndexInclusive)
    {
        if (self == null)
        {
            throw new ArgumentNullException("self");
        }

        if (self.Count == 0)
        {
            throw new ArgumentException("List is empty.", "self");
        }

        int max = self[startIndexInclusive];
        int maxIndex = startIndexInclusive;

        for (int i = startIndexInclusive + 1; i <= endIndexInclusive; ++i)
        {
            if (self[i] > max)
            {
                max = self[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }
}