using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonthDisplay : MonoBehaviour
{
    public Text UIMonthText;
    public Slider UICurrentMonthProgressSlider;

    private void Awake()
    {
        UpdateManager.Instance.OnMonthTick += UpdateMonth;
        UpdateMonth();
    }

    private void Update()
    {
        UICurrentMonthProgressSlider.value = UpdateManager.Instance.MonthProgress;
    }

    private void UpdateMonth()
    {
        UIMonthText.text = UpdateManager.Instance.CurrentMonth.ToString();
    }
}
