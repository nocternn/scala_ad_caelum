using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Image _progressFill;

    void Awake()
    {
        _progressFill = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateProgress(int index)
    {
        _progressFill.fillAmount = Arrays.StageProgressFill[index];
    }
}
