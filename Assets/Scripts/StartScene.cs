using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
    [SerializeField] private AudioClip transitionSound;
    [SerializeField] private AudioSource audioSource;

    public void StartARScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadRules()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTransitionMusicAndLoadRulesScene()
    {
        audioSource.clip = transitionSound;
        audioSource.Play();

        StartCoroutine(WaitForTransitionSoundToFinishRules());
    }

    public void PlayTransitionMusicAndLoadARGameScene()
    {
        audioSource.clip = transitionSound;
        audioSource.Play();

        StartCoroutine(WaitForTransitionSoundToFinishARGame());
    }

    private IEnumerator WaitForTransitionSoundToFinishRules()
    {
        yield return new WaitForSeconds(1);

        LoadRules();
    }

    private IEnumerator WaitForTransitionSoundToFinishARGame()
    {
        yield return new WaitForSeconds(1);

        StartARScene();
    }
}
