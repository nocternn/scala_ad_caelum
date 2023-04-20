using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Information")]
    public Sprite primaryIcon;
    public Sprite secondaryIcon;
    public string itemName;
}
