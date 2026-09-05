using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Money : MonoBehaviour
{
    public TMP_Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        if (moneyText != null && GManager.instance != null)
        {
            moneyText.SetText("$" + GManager.instance.money);
        }
        else if (moneyText = null)
        {
            Debug.Log("インスペクターにmoneyTextがセットされていません");
            Destroy(this);
        }
        else
        {
            Debug.Log("ゲームマネージャーが存在しません");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.SetText("$" + GManager.instance.money);
    }
}
