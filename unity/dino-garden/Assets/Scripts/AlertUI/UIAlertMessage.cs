using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlertMessage : UIAlertBase
{
    public void showAlert(string title, string content)
    {
        this.SetTitleAndContent(title, content);
        base.Show();
    }
}
