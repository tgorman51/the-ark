using System.Collections;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public GameObject endGameCanvasObject;
    public float endTextAppearDelay = 2f;
    public float endTextFadeOutDelay = 20f;
    public float returnToStartDelay = 5f;
    public SceneListSO sceneList;

    private GameObject _endText;
    private Animator _endTextAnimator;
    private AudioSource _audioSource;
    
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    
    void Start()
    {
        _endText = endGameCanvasObject.transform.Find("BlackScreen/EndText").gameObject;
        _endTextAnimator = _endText.GetComponent<Animator>();
        _endText.SetActive(false);
        endGameCanvasObject.SetActive(false);
        
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        _audioSource.Stop();
        endGameCanvasObject.SetActive(true);
        yield return new WaitForSeconds(endTextAppearDelay);
        _endText.SetActive(true);
        yield return new WaitForSeconds(endTextFadeOutDelay);
        _endTextAnimator.SetBool(FadeOut, true);
        yield return new WaitForSeconds(returnToStartDelay);
        sceneList.LoadNext();
    }
}
