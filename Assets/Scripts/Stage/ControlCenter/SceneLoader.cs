using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // [IMPORTANT] This file has to be named "SceneLoader"
    
    public static SceneLoader Instance { get; private set; }

    [Header("Scene Transition")]
    public float transitionTime = 1.0f;
    public Enums.SceneType sceneType;
    public Enums.SceneType previousSceneType;
    public Enums.MenuType previousMenuType;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneLoader.Instance.sceneType = Enums.SceneType.Menu;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
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
            if (previousSceneType == Enums.SceneType.Game)
            {
                StageManager.Instance.gameObject.SetActive(false);
                
                MenuManager.Instance.menuType = Enums.MenuType.Main;
                MenuManager.Instance.Initialize();
            }
        }
        else
        {
            StageManager.Instance.gameObject.SetActive(true);
            bool firstInit = false;
            if (previousSceneType == Enums.SceneType.Menu)
            {
                if (previousMenuType == Enums.MenuType.Main)
                {
                    StageManager.Instance.isLocalBattle = false;
                    StageManager.Instance.hudType = Enums.HUDType.Dialogue;
                    StageManager.Instance.id = StatisticsManager.Instance.playerStats.progress.stage;

                    firstInit = true;
                }
                else
                {
                    StageManager.Instance.isLocalBattle = true;
                    StageManager.Instance.hudType = Enums.HUDType.Combat;
                    StageManager.Instance.id = 0;
                }
            }
            StageManager.Instance.Initialize(firstInit);
        }
    }
    
    void PlayCrossfade(bool start)
    {
        // Play Crossfade animation
        Animator transition = GetComponentInChildren<Animator>();
        transition.SetBool("Start", start);
    }

    public IEnumerator LoadScene(int offset, bool backToMain = false)
    {
        PlayCrossfade(true);
        
        // Wait
        yield return new WaitForSeconds(transitionTime);
        
        // Load next scene
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (backToMain)
            offset = -buildIndex;
        SceneManager.LoadScene(buildIndex + offset);
    }

}
