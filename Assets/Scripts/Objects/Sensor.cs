using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Animator[] triggeredObjects;
    public Material untriggeredMaterial;
    public Material triggeredMaterial;

    private int _count = 0;
    private List<Renderer> _hologramRenderers = new List<Renderer>();
    private static readonly int Triggered = Animator.StringToHash("Triggered");

    
    void Start()
    {
        foreach (Transform child in transform.Find("Hologram"))
        {
            Renderer rend = child.GetComponent<Renderer>();
            _hologramRenderers.Add(rend);
            rend.material = untriggeredMaterial;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ship") && !other.CompareTag("Player")) return;
        if (_count++ != 0) return;  // Only update sensor state if this is the first ship to enter
        foreach (Animator triggeredObject in triggeredObjects)
            triggeredObject.SetBool(Triggered, !triggeredObject.GetBool(Triggered));
        foreach (Renderer rend in _hologramRenderers)
            rend.material = triggeredMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ship") && !other.CompareTag("Player")) return;
        if (--_count != 0) return;  // Only update trigger state if no ships remain
        foreach (Animator triggeredObject in triggeredObjects)
            triggeredObject.SetBool(Triggered, !triggeredObject.GetBool(Triggered));
        foreach (Renderer rend in _hologramRenderers)
            rend.material = untriggeredMaterial;

    }
}
