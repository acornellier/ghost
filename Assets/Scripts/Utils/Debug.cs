using System.Linq;
using UnityEngine;

public static class D
{
    public static void Log(params object[] v)
    {
        Debug.Log(string.Join(", ", v.Select(s => s.ToString())));
    }
}