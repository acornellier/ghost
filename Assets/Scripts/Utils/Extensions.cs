using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static List<T> GetComponentsInDirectChildren<T>(this GameObject gameObject)
        where T : Component
    {
        var length = gameObject.transform.childCount;
        var components = new List<T>(length);
        for (var i = 0; i < length; i++)
        {
            var comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }

        return components;
    }
}