using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] AudioClip musicPreview;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        source.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (!source.isPlaying)
        {
            source.PlayOneShot(musicPreview);
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has RaGe QuIt!!1111");
    }
}
