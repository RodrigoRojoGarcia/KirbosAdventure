﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    private Text healthText;

    private void Awake()
    {
        healthText = GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + GameManager.instance.kirbo.getHealth();
    }
}
