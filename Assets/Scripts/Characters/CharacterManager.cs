using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected StageManager _stage;
    
    public Transform lockOnTransform;
    public bool isInteracting;
    public bool canRotate;
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }
}
