using UnityEngine;
using UnityEngine.InputSystem;

public class GhostRecorder : MonoBehaviour
{
    public Ghost ghost;
    public ResetEventManager resetEventManager;
    
    private float _timer;
    private float _currentTime;

    private void Awake()
    {
        if (ghost && ghost.record)
        {
            ghost.ResetData();
            _timer = 0;
            _currentTime = 0;
        }

        resetEventManager.ResetTriggered += HandleReset;
    }
    
    void OnDestroy()
    {
        resetEventManager.ResetTriggered -= HandleReset;
    }

    private void HandleReset()
    {
        _timer = 0;
        _currentTime = 0;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        _currentTime += Time.deltaTime;
        if (ghost.record && _timer >= 1/ghost.recordFrequency)
        {
            ghost.TimeSeries.Add(new Ghost.FrameData
                { TimeStamp = _currentTime, Position = transform.position, Rotation = transform.rotation });
            
            _timer = 0;
        }
    }
}
