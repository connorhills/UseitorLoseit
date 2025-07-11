using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MethodManager : MonoBehaviour
{
    public static List<T> CreateList<T>(int capacity) => Enumerable.Repeat(default(T), capacity).ToList();

    public static void UpgradeCheck<T>(List<T> list, int length) where T : new()
    {
        try
        {
            if (list.Count == 0) list = new T[length].ToList();
            while (list.Count < length) list.Add(item: new T());
        }
        catch 
        {
            list = CreateList<T>(length);
        }
    }
}
