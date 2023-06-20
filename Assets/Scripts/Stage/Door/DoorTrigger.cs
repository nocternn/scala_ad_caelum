using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Door _door;

    void Awake()
    {
        _door = GameObject.FindObjectsOfType<Door>(true)[0];
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _door.isOpenable = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _door.isOpenable = false;
        }
    }
}
