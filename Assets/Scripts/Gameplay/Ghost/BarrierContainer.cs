using UnityEngine;

public class BarrierContainer : MonoBehaviour
{
    public void Enable()
    {
        foreach (var t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.SetActive(true);
        }
    }

    public void Disable()
    {
        foreach (var t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.SetActive(false);
        }
    }
}