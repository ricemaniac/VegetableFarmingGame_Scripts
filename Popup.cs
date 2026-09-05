using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static PlayerController;

public class Popup : MonoBehaviour
{
    public GameObject popup;
    public bool isActive = false;

    public void Push()
    {
        if (!isActive)
        {
            popup.SetActive(true);
            isActive = true;
        }
        else
        {
            popup.SetActive(false);
            isActive = false;
        }
    }
}
