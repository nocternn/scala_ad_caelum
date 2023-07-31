using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _doorWing;
    
    [Header("Interaction Configs")]
    public bool isOpen;
    public bool isOpenable;

    [Header("Rotation Configs")]
    [SerializeField] private bool _isRotatingDoor;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationAmount;
    [SerializeField] private float _forwardDirection;

    private Vector3 _startRotation;
    private Vector3 _forward;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        isOpen = false;
        isOpenable = false;

        _isRotatingDoor = true;
        _speed = 1.0f;
        _rotationAmount = 45.0f;
        _forwardDirection = 0;
        
        _startRotation = transform.rotation.eulerAngles;
        _forward = transform.right; // transform.forward is pointing into the door frame so we choose another "forward"
    }

    #region Triggers

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpenable = !isOpen;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpenable = false;
        }
    }

    #endregion

    #region Animation

    public void StopAnimation()
    {
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
    }

    public void StartAnimation(Enums.CharacterInteractionType type)
    {
        switch (type)
        {
            case Enums.CharacterInteractionType.OpenDoor:
                if (_isRotatingDoor)
                {
                    float dot = Vector3.Dot(_forward,
                        (PlayerManager.Instance.transform.position - transform.position).normalized);
                    _animationCoroutine = StartCoroutine(DoRotationOpen(dot));
                }
                break;
            default:
                if (_isRotatingDoor)
                {
                    _animationCoroutine = StartCoroutine(DoRotationClose());
                }
                break;
        }
    }
    
    #endregion

    #region Animations

    public IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = _doorWing.transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= _forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, _startRotation.y + _rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, _startRotation.y - _rotationAmount, 0));
        }

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            _doorWing.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    public IEnumerator DoRotationClose()
    {
        Quaternion startRotation = _doorWing.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotation);

        isOpen = false;
        
        float time = 0;
        while (time < 1)
        {
            _doorWing.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }

        this.gameObject.SetActive(false);
    }

    #endregion
}
