using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSkill : MonoBehaviour
{
    protected WeaponItem _weapon;

    protected PlayerManager _player;
    protected EnemyManager _enemy;

    public bool isUsable;

    public void Initialize(WeaponItem weapon, PlayerManager player = null, EnemyManager enemy = null)
    {
        _weapon = weapon;

        _player = player;
        _enemy  = enemy;

        isUsable = true;
    }

    public void UseSkill()
    {
      switch (_weapon.id)
      {
          case 1:
              Pistol();
              break;
          case 2:
              Greatsword();
              break;
          case 3:
              Gauntlet();
              break;
          case 4:
              Katana();
              break;
          case 5:
              Scythe();
              break;
          default:
              Debug.Log("Invalid weapon ID");
              break;
      }
    }

    protected virtual void Pistol() { }
    protected virtual void Greatsword() { }
    protected virtual void Gauntlet() { }
    protected virtual void Katana() { }
    protected virtual void Scythe() { }
}
