using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

[SuppressMessage("ReSharper", "IdentifierTypo")]
public class GhostManager : MonoBehaviour
{
    public GhostRecorder recorder;
    public GameObject ghostPrefab;
    public float ghostRecordFrequency = 250f;
    public ResetEventManager resetEventManager;
    
    // private List<GameObject> _ghosts;
    private Ghost _currentGhost;
    private GameObject _currentGhostGameObject;
    
    void Start()
    {
        resetEventManager.ResetTriggered += HandleReset;
        RecordNewGhost();
    }
    
    void OnDestroy()
    {
        resetEventManager.ResetTriggered -= HandleReset;
    }

    private void RecordNewGhost()
    {
        // Create the Ghost object
        _currentGhost = ScriptableObject.CreateInstance<Ghost>();
        _currentGhost.recordFrequency = ghostRecordFrequency;
        
        // Create the ghost Gameobject
        _currentGhostGameObject = Instantiate(ghostPrefab);
        // _ghosts.Add(ghostGameObject);
        ObjectResetter.RegisterObject(_currentGhostGameObject);
        GhostReplayer replayer = _currentGhostGameObject.GetComponent<GhostReplayer>();
        replayer.resetEventManager = resetEventManager;
        replayer.ghost = _currentGhost;
        
        // Set the GhostRecorder
        recorder.ghost = _currentGhost;
        _currentGhost.Record();
        
        // Keep Gameobject inactive until ready to replay
        _currentGhostGameObject.SetActive(false);
    }

    private void HandleReset()
    {
        _currentGhost.Replay();
        _currentGhostGameObject.SetActive(true);
        RecordNewGhost();
    }
    
    /*
     * def CreateNewGhost:
     *   instantiate new ghostPrefab, ghostGameObject
     *   add ghostGameObject to resetObjectList
     *   attach a GhostReplayer to ghostGameObject
     *   create a new Ghost object, newGhost
     *   newGhost.Record()
     *   ghostGameObject.GhostReplayer.ghost = newGhost
     *   recorder.ghost = newGhost
     *
     * when scene starts:
     *   CreateNewGhost
     *
     * when reset occurs:
     *   set current ghost to replay:
     *     currentGhost.Replay()
     *     activate ghost GameObject
     *   create new ghost and set to record
     *     CreateNewGhost
     */
}
