using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{
	private Slider _slider;
	private TMP_Text _txtValue;
	private TMP_Text _txtMaxValue;

    void Awake()
    {
		_slider      = GetComponent<Slider>();
		_txtValue    = transform.Find("Stat").transform.Find("Value").GetComponent<TMP_Text>();
		_txtMaxValue = transform.Find("Stat").transform.Find("MaxValue").GetComponent<TMP_Text>();
    }

    public void SetMaxValue(int maxValue)
	{
		_slider.maxValue = maxValue;
		_slider.value = maxValue;
	}

	public void SetValue(int currentValue)
	{
		_slider.value = currentValue;
	}

	public void UpdateUI(int currentValue, int maxValue)
	{
		SetMaxValue(maxValue);
		SetValue(currentValue);

		_txtValue.text    = currentValue.ToString();
		_txtMaxValue.text = maxValue.ToString();
	}
}
