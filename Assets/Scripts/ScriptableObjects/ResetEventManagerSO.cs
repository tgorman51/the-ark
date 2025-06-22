using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResetEventManager", menuName = "Scriptable Objects/ResetEventManager")]
public class ResetEventManager : ScriptableObject
{
    public event Action ResetTriggered;

    public void TriggerResetEvent()
    {
        ResetTriggered?.Invoke();
    }
}
