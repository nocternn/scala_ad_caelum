using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Information")]
    public Sprite primaryIcon;
    public Sprite secondaryIcon;
    public new string name;
    public string description;
    public int level;
}
