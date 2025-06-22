using UnityEngine;
using UnityEngine.InputSystem;

public class GhostReplayer : MonoBehaviour
{
    public Ghost ghost;
    public ResetEventManager resetEventManager;
    
    private float _currentTime = 0;
    private int _index1;
    private int _index2;

    void Start()
    {
        resetEventManager.ResetTriggered += HandleReset;
    }
    
    void OnDestroy()
    {
        resetEventManager.ResetTriggered -= HandleReset;
    }
    
    private void HandleReset()
    {
        _currentTime = 0;
    }
    
    private void GetIndex()
    {
        for (int i = 0; i < ghost.TimeSeries.Count - 2; i++)
        {
            if (Mathf.Approximately(ghost.TimeSeries[i].TimeStamp, _currentTime))
            {
                _index1 = i;
                _index2 = i;
                return;
            }
            else if (ghost.TimeSeries[i].TimeStamp < _currentTime && _currentTime < ghost.TimeSeries[i + 1].TimeStamp)
            {
                _index1 = i;
                _index2 = i + 1;
                return;
            }
        }
        
        _index1 = ghost.TimeSeries.Count - 1;
        _index2 = ghost.TimeSeries.Count - 1;
    }

    private void SetTransform()
    {
        if (_index1 == _index2)
        {
            transform.position = ghost.TimeSeries[_index1].Position;
            transform.rotation = ghost.TimeSeries[_index1].Rotation;
        }
        else
        {
            float interpolationFactor = (_currentTime - ghost.TimeSeries[_index1].TimeStamp) /
                                        (ghost.TimeSeries[_index2].TimeStamp - ghost.TimeSeries[_index1].TimeStamp);
            transform.position = Vector3.Lerp(ghost.TimeSeries[_index1].Position, ghost.TimeSeries[_index2].Position, interpolationFactor);
            transform.rotation = Quaternion.Lerp(ghost.TimeSeries[_index1].Rotation, ghost.TimeSeries[_index2].Rotation, interpolationFactor);
        }
    }
    
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (ghost.replay && ghost.TimeSeries.Count > 0)
        {
            GetIndex();
            SetTransform();
        }
    }
}
