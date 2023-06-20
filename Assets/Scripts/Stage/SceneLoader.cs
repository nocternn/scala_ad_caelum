using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public float transitionTime = 1.0f;
    public WeaponItem weapon;
    public Enums.SceneType sceneType;

    private GameObject _stage;

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
        
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
        
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        PlayCrossfade(false);

        if (sceneType == Enums.SceneType.Menu)
        {
            if (_stage != null)
                _stage.SetActive(false);
        }
        else
        {
            if (_stage == null)
            {
                _stage = GameObject.Find("StageManager");
            }

            _stage.SetActive(true);
            
            StageManager stageManager = _stage.GetComponent<StageManager>();
            stageManager.Initialize();
        }
    }

    public IEnumerator LoadScene(int offset)
    {
        PlayCrossfade(true);
        
        // Wait
        yield return new WaitForSeconds(transitionTime);
        
        // Load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + offset);
    }

    private void PlayCrossfade(bool start)
    {
        // Play Crossfade animation
        Animator transition = GetComponentInChildren<Animator>();
        transition.SetBool("Start", start);
    }
}
