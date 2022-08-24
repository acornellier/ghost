using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T GetComponentInDirectChildren<T>(this GameObject gameObject)
        where T : Component
    {
        var length = gameObject.transform.childCount;
        for (var i = 0; i < length; i++)
        {
            var comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) return comp;
        }

        return null;
    }

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

    public static bool IsLayerInMask(this GameObject gameObject, LayerMask mask)
    {
        return mask == (mask | (1 << gameObject.layer));
    }
}