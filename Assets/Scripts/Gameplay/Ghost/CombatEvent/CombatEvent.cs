using UnityEngine;

public abstract class CombatEvent : MonoBehaviour
{
    public bool IsDone { get; protected set; }

    public abstract void Run();
}