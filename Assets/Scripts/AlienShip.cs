using System.Collections;
using UnityEngine;

public class AlienShip : MonoBehaviour
{
    public float moveDelay;
    public float speed;
    
    private bool _canMove;

    void Start()
    {
        StartCoroutine(MoveDelay());
    }

    private IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(moveDelay);
        _canMove = true;
    }

    void Update()
    {
        if (_canMove)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}
