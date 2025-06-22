using UnityEngine;

public class WinConditionManager : MonoBehaviour
{
    public BoolScriptableObject winConditionMet;

    private int _goalCount;
    private int _collectedCount;

    void Start()
    {
        winConditionMet.value = false;
        
        Goal[] goals = FindObjectsByType<Goal>(FindObjectsSortMode.None);
        _goalCount = goals.Length;
        // Debug.Log($"Goal count {_goalCount}");
        foreach (Goal goal in goals)
        {
            goal.Collected += HandleGoalCollection;
        }
    }

    private void HandleGoalCollection()
    {
        _collectedCount++;
        if (_collectedCount >= _goalCount)
        {
            winConditionMet.value = true;
            // Debug.Log("Win Condition Met!");
        }
    }
}
