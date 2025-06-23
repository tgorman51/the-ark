using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ProfileDoor : MonoBehaviour//, //BoolState
{
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public bool startOpen;

    private AudioSource _audioSource;
    private Animator _animator;
    private bool _isOpen;
    
    private static readonly int Triggered = Animator.StringToHash("Triggered");
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        SetBool(startOpen);
    }

    public void SetBool(bool value)
    {
        _isOpen = value;
        _animator.SetBool(Triggered, value);
        _audioSource.PlayOneShot(value ? doorOpen : doorClose);
    }

    public bool GetBool()
    {
        return _isOpen;
    }
}
