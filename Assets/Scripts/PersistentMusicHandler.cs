using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusicHandler : MonoBehaviour
{
    private static PersistentMusicHandler _instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EndGame")
        {
            audioSource.Stop();
        }
    }
}
