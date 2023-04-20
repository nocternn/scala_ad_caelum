using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager singleton;
    public int id = 1;

    [SerializeField] private PlayerManager _player;
    [SerializeField] private EnemyManager _enemy;
    [SerializeField] private HUDManager _hud;
    
    public CameraHandler cameraHandler;

    private void Awake()
    {
        singleton = this;

        cameraHandler = GameObject.Find("Camera Holder").GetComponent<CameraHandler>();
        
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _enemy  = GameObject.Find("Enemy").GetComponent<EnemyManager>();
        _hud    = GameObject.Find("HUD").GetComponent<HUDManager>();

        _player.SetManager(singleton);

        // Temporarily init right away. Change later to call when switching scenes. 
        Initialize();
    }

    public void Initialize()
    {
        _player.Initialize();
        _enemy.Initialize();
        _hud.Initialize();

        _enemy.SetEnemyType(EnemyManager.Types[id - 1]);
    }
}
