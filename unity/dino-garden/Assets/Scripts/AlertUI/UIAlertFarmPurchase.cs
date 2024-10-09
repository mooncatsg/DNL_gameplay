using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlertFarmPurchase : UIAlertBase
{
    public void showAlert(int startAmount, float rate)
    {
        this.SetStartAmountAndRate(startAmount, rate);
        base.Show();
    }
}
