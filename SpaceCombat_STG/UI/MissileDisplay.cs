using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Image cooldownImage;
    static Text _text;
    
    void Awake()
    {
        _text = transform.Find("MissileAmount").GetComponent<Text>();
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }

    public static void UpdateAmountText(int amount) => _text.text = amount.ToString();
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;

}
