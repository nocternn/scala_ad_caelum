using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtTimer;
    
    private int _remainingDuration;
    
    public bool pause;
    public int duration;

    public int GetRewardAmount()
    {
        if (_remainingDuration >= 420) // 7 minutes * 60 seconds
            return 100;
        
        if (_remainingDuration >= 300) // 5 minutes * 60 seconds
            return 50;
        
        if (_remainingDuration >= 180) // 3 minutes * 60 seconds
            return 10;

        return 0;
    }

    private void Start()
    {
        pause = false;
        _txtTimer = GetComponent<TMP_Text>();
        
        duration = 600; // 10 minutes * 60 seconds
        Being(duration);
    }

    private void Being(int second)
    {
        _remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_remainingDuration >= 0)
        {
            if (!pause)
            {
                _txtTimer.text = $"{_remainingDuration / 60:00}:{_remainingDuration % 60:00}";
                _remainingDuration--;

                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        _txtTimer.text = "00:00";
        pause = true;
    }
}
