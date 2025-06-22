using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectResetter : MonoBehaviour
{
    private struct ResetData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public ResetEventManager resetEventManager;
    public List<GameObject> registerAtStartList;
    
    private Dictionary<GameObject, ResetData> _resetObjectData = new();

    private static ObjectResetter Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        resetEventManager.ResetTriggered += Reset;

        // Save reset objects' data at start
        foreach (GameObject resetObject in registerAtStartList)
        {
            SaveObject(resetObject);
        }
    }

    void OnDestroy()
    {
        resetEventManager.ResetTriggered -= Reset;
    }
    
    public static void RegisterObject(GameObject obj)
    {
        if (Instance)
            Instance.SaveObject(obj);
        else
            Debug.LogWarning("ObjectResetter not instantiated");
    }
    
    private void Reset()
    {
        foreach (GameObject resetObject in _resetObjectData.Keys)
        {
            ResetObject(resetObject);
        }
    }

    private void SaveObject(GameObject obj)
    {
        Vector3 savePosition;
        Quaternion saveRotation;
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            savePosition = rb.position;
            saveRotation = rb.rotation;
        }
        else
        {
            savePosition = obj.transform.position;
            saveRotation = obj.transform.rotation;
        }
        _resetObjectData.Add(obj, new ResetData
        {
            Position = savePosition,
            Rotation = saveRotation
        });
        if (obj.transform.childCount <= 0) return;
        foreach (Transform child in obj.transform) SaveObject(child.gameObject);
    }

    private void ResetObject(GameObject obj)
    {
        ResetData resetData = _resetObjectData[obj];

        if (obj.TryGetComponent(out Rigidbody rb))
        {
            rb.position = resetData.Position;
            rb.rotation = resetData.Rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            obj.transform.localPosition = resetData.Position;
            obj.transform.localRotation = resetData.Rotation;
        }
        if (obj.transform.childCount <= 0) return;
        foreach (Transform child in obj.transform) ResetObject(child.gameObject);
    }
}
