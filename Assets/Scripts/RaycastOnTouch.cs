using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RaycastOnTouch : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CubePositionManager cubePositionManager;

    [SerializeField] private GameObject startButton;
    private GameObject firstTappedObject = null;
    private Vector3 firstTappedObjectOriginalPosition;

    [SerializeField] private TextMeshProUGUI matchOrNot;

    private GameObject secondTappedObject = null;
    private Vector3 secondTappedObjectOriginalPosition;
    private bool isFirstObjectTapped = false;

    private bool isTouchInputEnabled = true;

    [SerializeField] private int pairsSelected = 0;

    [SerializeField] private AudioSource BGAudioPlayer;
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip tapSFX;
    [SerializeField] private AudioClip failSFX;

    [SerializeField] private GameObject stopSignGreen;
    [SerializeField] private GameObject stopSignRed;

    [SerializeField] private AudioSource signsPlayer;
    [SerializeField] private AudioClip right;
    [SerializeField] private AudioClip wrong;

    [SerializeField] private GameObject matchedPairs;
    [SerializeField] private GameObject timeSlider;

    [SerializeField] private Animator animator;
    private string animationBoolParameter = "StartSpawnAnim";

    [SerializeField] private GameObject LifeLeftWorldSpaceCanvas;
    [SerializeField] private GameObject TimeLefttWorldSpaceCanvas;


    private void Awake()
    {
    
    }

    void Update()
    {
        if (isTouchInputEnabled) 
        {
            touchInput();
        }
    }

    private void touchInput()
    {
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastFromTouch(ray);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastFromTouch(ray);
        }
    }

    private void RaycastFromTouch(Ray ray)
    {
        RaycastHit hit;
        int layerMask = 1 << 9;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            GameObject tappedObject = hit.collider.gameObject;

            if (!isFirstObjectTapped || (isFirstObjectTapped && tappedObject != firstTappedObject))
            {
                if (!isFirstObjectTapped)
                {
                    playTapSFX();

                    isFirstObjectTapped = true;
                    firstTappedObject = tappedObject;
                    firstTappedObjectOriginalPosition = tappedObject.transform.position;
                    cubePositionManager.ChangeSpawnOffset(firstTappedObject, null);
                }
                else
                {
                    playTapSFX();

                    secondTappedObject = tappedObject;
                    secondTappedObjectOriginalPosition = tappedObject.transform.position;
                    cubePositionManager.ChangeSpawnOffset(secondTappedObject, null);

                    StartCoroutine(ProcessTappedObjects());
                }
            }
        }
    }

    private IEnumerator ProcessTappedObjects()
    {
        isTouchInputEnabled = false;

        yield return new WaitForSeconds(1.5f);

        if (firstTappedObject == secondTappedObject)
        {
            matchOrNot.text = "Not Allowed";
        }
        else if (firstTappedObject.tag == secondTappedObject.tag)
        {
            Debug.Log("Objects with tag " + secondTappedObject.tag + " tapped. Deleting...");
            Destroy(firstTappedObject);
            Destroy(secondTappedObject);

            cubePositionManager.DisableCubeForFish(firstTappedObject);
            cubePositionManager.DisableCubeForFish(secondTappedObject);

            setStopSignGreen();

            pairsSelected++;

            ScoreIncrease();

            CheckWinCondition();
        }
        else
        {
            firstTappedObject.transform.position = firstTappedObjectOriginalPosition;
            secondTappedObject.transform.position = secondTappedObjectOriginalPosition;

            setStopSignRed();

            scoreManager.GetDecreaseLife();

        }

        isFirstObjectTapped = false;
        firstTappedObject = null;
        secondTappedObject = null;

        isTouchInputEnabled = true;
    }


    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ScoreIncrease()
    {
        scoreManager.IncreaseScore();
    }

    public void StartScene()
    {
        //Debug.Log("hi");
        cubePositionManager.StoreCubePositions();
        cubePositionManager.SpawnFishRandomly();
        cubePositionManager.StartChangeSpwanedFishPositionCoroutine();

        StartCoroutine(StartButtonTimer());

        

        scoreManager.startIncreaseTimeRemaining();
    }

    public IEnumerator StartButtonTimer()
    {
        playTapSFX();

        yield return new WaitForSeconds(4.5f);

        startButton.SetActive(false);

        matchedPairs.SetActive(true);
        //timeSlider.SetActive(true);
    }

    private void CheckWinCondition()
    {
        if(pairsSelected >= 5)
        {
            SceneManager.LoadScene(3);
        }
    }

    private void playTapSFX()
    {
        AudioPlayer.clip = tapSFX;
        AudioPlayer.Play();
    }

    public void playFailSFXAndRestartScene()
    {
        StartCoroutine(failSFXandRestartScene());
    }

    private IEnumerator failSFXandRestartScene()
    {
        BGAudioPlayer.Stop();

        AudioPlayer.clip = failSFX;
        AudioPlayer.Play();

        yield return new WaitForSeconds(2.5f);

        RestartScene();
    }

    private void setStopSignGreen()
    {
        stopSignGreen.SetActive(true);
        stopSignRed.SetActive(false);

        signsPlayer.clip = right;
        signsPlayer.Play();

        StartCoroutine(GreenSignDisable());
    }

    private void setStopSignRed()
    {
        stopSignGreen.SetActive(false);
        stopSignRed.SetActive(true);

        signsPlayer.clip = wrong;
        signsPlayer.Play();

        StartCoroutine(RedSignDisable());
    }

    private IEnumerator RedSignDisable()
    {
        yield return new WaitForSeconds(2);

        stopSignRed.SetActive(false);
    }

    private IEnumerator GreenSignDisable()
    {
        yield return new WaitForSeconds(2);

        stopSignGreen.SetActive(false);
    }

    public void TriggerAnimation()
    {
        animator = GetComponentInChildren<Animator>();

        if(animator != null)
        {
            animator.SetBool(animationBoolParameter, true);

            StartCoroutine(DisableLifeCanvas());
        }

        else
        {
            Debug.Log("animator not found");
        }
    }

    private IEnumerator DisableLifeCanvas()
    {
        yield return new WaitForSeconds(4f);

        LifeLeftWorldSpaceCanvas.SetActive(true);
        TimeLefttWorldSpaceCanvas.SetActive(true);
    }
}
