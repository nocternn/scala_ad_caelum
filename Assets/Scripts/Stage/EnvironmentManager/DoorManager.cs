using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private Door _door;

    private void Awake()
    {
        _door = GameObject.FindObjectsOfType<Door>(true)[0];
    }

    public void Initialize()
    {
        Close();
    }

    public void ToggleVisibility(bool visible)
    {
        _door.gameObject.SetActive(visible);
        if (!visible)
            _door.isOpenable = false;
    }
    
    public bool IsOpen()
    {
        return _door.isOpen;
    }
    public bool IsOpenable()
    {
        return _door.isOpenable;
    }

    public void Open()
    {
        if (_door.isOpen)
            return;
        
        _door.StopAnimation();
        _door.StartAnimation(Enums.CharacterInteractionType.OpenDoor);
    }
    
    public void Close()
    {
        if (!_door.isOpen)
            return;

        _door.StopAnimation();
        _door.StartAnimation(Enums.CharacterInteractionType.CloseDoor);
    }
}
