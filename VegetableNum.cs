using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VegetableNum : MonoBehaviour
{
    //野菜の数に関する変数
    public TMP_Text vegetableNumText;
    public string vegatbleNumName;

    // Start is called before the first frame update
    void Start()
    {
        if (vegetableNumText != null)
        {
            foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
            {
                if (vegetable.vegetableName == vegatbleNumName)
                {
                    vegetableNumText.SetText("/" + vegetable.vegetableNum.ToString("D3"));
                }
            }
        }
        else if (vegetableNumText = null)
        {
            Debug.Log("インスペクターにvegetableNumTextがセットされていません");
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
        foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
        {
            if (vegetable.vegetableName == vegatbleNumName)
            {
                vegetableNumText.SetText("/" + vegetable.vegetableNum.ToString("D3"));
            }
        }
    }
}
