using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HeartProgress : MonoBehaviour
{
    [SerializeField] Image bar_1;
    [SerializeField] Image bar_2;
    [SerializeField] Text txtTitle;

    public void SetData(int value1, int value2, int max)
    {
        this.bar_1.fillAmount = value1 * 1.0f / max;
        this.bar_2.fillAmount = value2 * 1.0f / max;
        if(value1 < value2)
            this.txtTitle.text = String.Format("{0}+{1}", value1, value2 - value1);        
        else this.txtTitle.text = String.Format("{0}", value2);
    }
}
