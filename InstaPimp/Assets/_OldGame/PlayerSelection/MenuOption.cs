using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    public Text NotActive;
    public Text Active;
       
    public void Activate()
    {
        NotActive.gameObject.SetActive(false);
        Active.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        NotActive.gameObject.SetActive(true);
        Active.gameObject.SetActive(false);
    }

    void Awake()
    {
        this.Deactivate();
    }
}
