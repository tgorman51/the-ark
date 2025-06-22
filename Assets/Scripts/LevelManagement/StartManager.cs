using System.Collections;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public SceneListSO sceneList;
    public GameObject startMenu;
    public Transform camera;
    public float startDelay = 10f;
    public float linearForce = 0.1f;
    public float angularForce = 0.1f;

    private Rigidbody _cameraRigidbody;
    
    void Start()
    {
        sceneList.ResetIndex();
        _cameraRigidbody = camera.gameObject.GetComponent<Rigidbody>();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        startMenu.SetActive(false);
        _cameraRigidbody.AddForce(-camera.transform.forward * linearForce, ForceMode.Impulse);
        _cameraRigidbody.AddTorque(camera.transform.forward * angularForce, ForceMode.Impulse);
        yield return new WaitForSeconds(startDelay);
        sceneList.LoadNext();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
