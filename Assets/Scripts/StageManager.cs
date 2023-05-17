using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Enums.StageType type;
    
    public int id = 1;

    public PlayerManager player;
    public EnemyManager enemy;
    
    public HUDManager hud;
    public new CameraHandler camera;

    public List<CardItem> selectedBuffs;
    public GameObject buffHolder;
    
    private void Awake()
    {
        type = Enums.StageType.Buff;

        camera = GameObject.Find("Camera Holder").GetComponent<CameraHandler>();
        hud    = GameObject.Find("HUD").GetComponent<HUDManager>();

        hud.SetManager(this);
        hud.Initialize();
    }
    
    private void Update()
    {
        if (type == Enums.StageType.Buff)
            return;
        
        foreach (CardItem buff in selectedBuffs)
        {
            buff.effect.Apply(buff, player, enemy);
        }
    }

    public void SwitchToCombat(CardItem selectedBuff)
    {
        selectedBuffs.Add(selectedBuff);
        
        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);

        player.SetManager(this);
        enemy.SetManager(this);
        
        player.Initialize();
        enemy.Initialize();

        enemy.SetEnemyType(EnemyManager.Types[id - 1]);
        
        type = Enums.StageType.Combat;
        
        hud.Initialize();
    }
}
