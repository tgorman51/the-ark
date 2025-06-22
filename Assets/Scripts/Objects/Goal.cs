using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public event Action Collected;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collected?.Invoke();
            Destroy(gameObject);
        }
    }
}
