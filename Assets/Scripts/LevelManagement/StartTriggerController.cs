using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartTriggerController : MonoBehaviour
{
    public float resetDelay;
    public float startDelay = 1f;
    
    [Header("References")]
    public SceneListSO sceneList;
    public BoolScriptableObject winConditionMet;
    public ResetEventManager resetEventManager;
    public Animator doorAnimator;
    
    [Header("Indicator Light")]
    public Light indicatorLight;
    public Color resettingColor;
    public Color winColor;
    
    private int _shipCount;
    private int _maxCount;
    private bool _playerLeftStart = false;
    private Color _originalLightColor;
    
    private static readonly bool Open = true;
    private static readonly bool Closed = false;
    private static readonly int Triggered = Animator.StringToHash("Triggered");

    void Start()
    {
        _originalLightColor = indicatorLight.color;
        doorAnimator.SetBool(Triggered, Closed);
        StartCoroutine(StartCoroutine());
    }

    private IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        doorAnimator.SetBool(Triggered, Open);
    }
    
    void OnTriggerEnter(Collider other)
    {
        _shipCount++;
        // Debug.Log($"Ship count: {_shipCount}");
        if (_shipCount >= _maxCount)
            _maxCount = _shipCount;

        if (!_playerLeftStart) return;
        if (winConditionMet.value)
        {
            if (other.CompareTag("Player"))
                indicatorLight.color = winColor;
            if (_shipCount == _maxCount)
                StartCoroutine(LevelWinCoroutine());
        }
        else if (other.CompareTag("Player"))
        {
            StartCoroutine(ResetCoroutine());
        }
    }

    void OnTriggerExit(Collider other)
    {
        _shipCount--;
        // Debug.Log($"Ship count: {_shipCount}");
        if (other.CompareTag("Player"))
            _playerLeftStart = true;
    }

    private IEnumerator ResetCoroutine()
    {
        doorAnimator.SetBool(Triggered, Closed);
        indicatorLight.color = resettingColor;
        yield return new WaitForSeconds(resetDelay);
        resetEventManager.TriggerResetEvent();
        yield return null;
        indicatorLight.color = _originalLightColor;
        doorAnimator.SetBool(Triggered, Open);
    }

    private IEnumerator LevelWinCoroutine()
    {
        Debug.Log("Level Completed!");
        doorAnimator.SetBool(Triggered, Closed);
        yield return new WaitForSeconds(resetDelay);
        sceneList.LoadNext();
    }
}
