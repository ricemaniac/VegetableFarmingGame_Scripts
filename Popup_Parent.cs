using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static PlayerController;

public class Popup_Parent : MonoBehaviour
{
    public GameObject[] popup;
    public bool isActive = false;
    public Popup childPopupScr;

    public void Push()
    {
        if (!isActive)
        {
            popup[0].SetActive(true);
            isActive = true;
        }
        else
        {
            for (int i = 0; i < popup.Length; i++)
            {
                popup[i].SetActive(false);
            }
            isActive = false;
            childPopupScr.isActive = false;
        }
    }
}
