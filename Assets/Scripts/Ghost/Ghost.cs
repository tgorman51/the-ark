using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ghost", menuName = "Scriptable Objects/Ghost")]
public class Ghost : ScriptableObject
{
    public struct FrameData
    {
        public float TimeStamp;
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public float recordFrequency = 250f;
    public bool record;
    public bool replay;
    public List<FrameData> TimeSeries = new List<FrameData>();

    public void ResetData()
    {
        TimeSeries.Clear();
    }

    public void Record()
    {
        record = true;
        replay = false;
    }

    public void Replay()
    {
        record = false;
        replay = true;
    }
}
