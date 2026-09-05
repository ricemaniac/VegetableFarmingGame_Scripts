using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedPopup : MonoBehaviour
{
    [SerializeField] private AudioClip fanfareSE;
    private AudioSource audioSource;

    // ポップアップが SetActive(true) になった瞬間に自動で発動
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

        if (fanfareSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(fanfareSE);
        }
    }

}
