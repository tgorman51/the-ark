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
    
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    
    void Start()
    {
        _endText = endGameCanvasObject.transform.Find("BlackScreen/EndText").gameObject;
        _endText.SetActive(false);
        endGameCanvasObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        endGameCanvasObject.SetActive(true);
        yield return new WaitForSeconds(endTextAppearDelay);
        _endText.SetActive(true);
        yield return new WaitForSeconds(endTextFadeOutDelay);
        _endText.GetComponent<Animator>().SetBool(FadeOut, true);
        yield return new WaitForSeconds(returnToStartDelay);
        sceneList.LoadNext();
    }
}
