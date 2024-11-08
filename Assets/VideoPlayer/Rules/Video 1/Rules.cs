using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Rules : MonoBehaviour
{
    [SerializeField] private RawImage RulesVideo;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private Button Next;
    [SerializeField] private Button Back;

    [SerializeField] private VideoClip Rule1;
    [SerializeField] private VideoClip Rule2;

    public void StartARScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeRuleVideo()
    {
        videoPlayer.clip = Rule2;
        videoPlayer.Play();
    }

    public void NextDisable()
    {
        Next.interactable = true;
        Back.interactable = false;
    }

    public void BackDisable()
    {
        Next.interactable = false;
        Back.interactable = true;

        videoPlayer.clip = Rule1;
        videoPlayer.Play();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }


}
