using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentSceneIndex { get; private set; }

    //

    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        //audio = GetComponent<AudioSource>();
        //audio.loop = true;
        DontDestroyOnLoad(gameObject);
    }
    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        currentSceneIndex = sceneIndex;
    }
    public void PlayMusic(AudioClip newSoundTrack)
    {
        //audio.clip = newSoundTrack;
        //audio.Play();
    }
}
