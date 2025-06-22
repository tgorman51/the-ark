using System.Collections.Generic;
using UnityEngine;

public class GhostDebugger : MonoBehaviour
{
    public Ghost ghost;
    public List<float> timeStamps;
    public List<Vector3> positions;
    public List<Quaternion> rotations;

    void Update()
    {
        timeStamps.Clear();
        positions.Clear();
        rotations.Clear();
        foreach (Ghost.FrameData frame in ghost.TimeSeries)
        {
            timeStamps.Add(frame.TimeStamp);
            positions.Add(frame.Position);
            rotations.Add(frame.Rotation);
        }
    }
}
