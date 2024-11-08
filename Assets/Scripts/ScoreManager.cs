using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ScoreManager : MonoBehaviour
{
    [SerializeField] RaycastOnTouch raycastOnTouch;

    [Space]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int score;

    [Space]
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;
    [SerializeField] private int lifeRemaining = 2;

    [SerializeField] private int spriteRemaining = 3;

    [SerializeField] public Slider timeRemaining;

    [SerializeField] private float timeIncreaseDuration = 60f;

    [SerializeField] private TextMeshProUGUI timeLeft;



    public void IncreaseScore()
    {
        score += 1;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Pairs Matched: " + score.ToString();
    }

    public void GetDecreaseLife()
    {
        DecreaseLife();
    }

    private void DecreaseLife()
    {
        if (lifeRemaining > 0)
        {
            lifeRemaining--;

            spriteRemaining--;

            lifeSprite();

        }
        else if(lifeRemaining == 0)
        {
            raycastOnTouch.playFailSFXAndRestartScene();
        }
    }

    private void lifeSprite()
    {
        if(lifeRemaining == 2)
        {
            life1.SetActive(false);
        }
        else if(lifeRemaining == 1)
        {
            life2.SetActive(false);
        }
        else if(lifeRemaining == 0)
        {
            life3.SetActive(false);
        }
    }

    private IEnumerator IncreaseTimeRemaining()
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeIncreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / timeIncreaseDuration;
            timeRemaining.value = Mathf.Lerp(0, 60, progress);

            float timeRemainingSliderValue = Mathf.Round(timeRemaining.value);


            timeLeft.text = timeRemainingSliderValue.ToString();

            if (timeRemaining.value >= 60)
            {
                raycastOnTouch.playFailSFXAndRestartScene();
                break; 
            }

            yield return null;
        }
        timeRemaining.value = 60; 
    }

    public void startIncreaseTimeRemaining()
    {
        StartCoroutine(IncreaseTimeRemaining());
    }


}

