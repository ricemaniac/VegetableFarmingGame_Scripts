using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class SellVegetableType
{
    public string sellVegName;
    public TMP_Text counter;
    public int selectVegNum;
    public TMP_Text vegPriceTag;
    public int vegPrice;
    public int totalPrice;
}

public class SellVegetable : MonoBehaviour
{
    private int totalPriceAll = 0;
    public TMP_Text totalPriceAllText;

    public GameObject sellCheck;
    public bool isActive = false;
    public TMP_Text totalPriceAllCkText;

    public SellVegetableType[] sellVegTypes;

    //野菜選択数プラスボタン、マイナスボタンに関する関数
    public bool plusButtonDownFlag = false;
    public bool minusButtonDownFlag = false;
    public float switchTime;
    public float switchTimer = 0.0f;
    public bool holdFlag = false;
    public float holdTime;
    public float holdTimer = 0.0f;
    public string nowInputVegName;

    // Start is called before the first frame update
    void Start()
    {
        totalPriceAllText.SetText("$" + totalPriceAll);

        foreach(SellVegetableType sellVegType in sellVegTypes)
        {
            sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
            sellVegType.vegPriceTag.SetText("$" + sellVegType.vegPrice.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (plusButtonDownFlag) //buttonDownFlagがtrueの時
        {
            if (switchTimer < switchTime)
            {
                switchTimer += Time.deltaTime;
            }
            else
            {
                holdFlag = true;
            }

            if (holdFlag)
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdTime)
                {
                    holdTimer = 0.0f;

                    totalPriceAll = 0;

                    foreach (SellVegetableType sellVegType in sellVegTypes)
                    {
                        if (sellVegType.sellVegName == nowInputVegName)
                        {
                            foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
                            {
                                if (vegetable.vegetableName == nowInputVegName && sellVegType.selectVegNum < 999 && sellVegType.selectVegNum < vegetable.vegetableNum)
                                {
                                    sellVegType.selectVegNum++;
                                }
                            }
                            sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
                            sellVegType.totalPrice = sellVegType.vegPrice * sellVegType.selectVegNum;
                        }
                    }

                    foreach (SellVegetableType sellVegType in sellVegTypes)
                    {
                        totalPriceAll = totalPriceAll + sellVegType.totalPrice;
                    }

                    totalPriceAllText.SetText("$" + totalPriceAll);
                }
            }
        }
        else if (minusButtonDownFlag) //buttonDownFlagがtrueの時
        {
            if (switchTimer < switchTime)
            {
                switchTimer += Time.deltaTime;
            }
            else
            {
                holdFlag = true;
            }

            if (holdFlag)
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdTime)
                {
                    holdTimer = 0.0f;

                    totalPriceAll = 0;

                    foreach (SellVegetableType sellVegType in sellVegTypes)
                    {
                        if (sellVegType.sellVegName == nowInputVegName)
                        {
                            if (sellVegType.selectVegNum > 0)
                            {
                                sellVegType.selectVegNum--;
                            }
                            sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
                            sellVegType.totalPrice = sellVegType.vegPrice * sellVegType.selectVegNum;
                        }
                    }

                    foreach (SellVegetableType sellVegType in sellVegTypes)
                    {
                        totalPriceAll = totalPriceAll + sellVegType.totalPrice;
                    }

                    totalPriceAllText.SetText("$" + totalPriceAll);
                }
            }
        }
        else
        {
            holdFlag = false;
            switchTimer = 0.0f;
            holdTimer = 0.0f;
        }
    }

    public void AddSellVegetable(string inputVegName)
    {
        plusButtonDownFlag = true;
        nowInputVegName = inputVegName;

        totalPriceAll = 0;

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            if(sellVegType.sellVegName == inputVegName)
            {
                foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
                {
                    if (vegetable.vegetableName == inputVegName && sellVegType.selectVegNum < 999 && sellVegType.selectVegNum < vegetable.vegetableNum)
                    {
                        sellVegType.selectVegNum++;
                    }
                }
                sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
                sellVegType.totalPrice = sellVegType.vegPrice * sellVegType.selectVegNum;
            } 
        }

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            totalPriceAll = totalPriceAll + sellVegType.totalPrice;
        }

        totalPriceAllText.SetText("$" + totalPriceAll);
        
    }

    public void PlusButtonUp()
    {
        plusButtonDownFlag = false;
    }

    public void SubtractSellVegetable(string inputVegName)
    {
        minusButtonDownFlag = true;
        nowInputVegName = inputVegName;

        totalPriceAll = 0;

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            if (sellVegType.sellVegName == inputVegName)
            {
                if (sellVegType.selectVegNum > 0)
                {
                    sellVegType.selectVegNum--;
                }
                sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
                sellVegType.totalPrice = sellVegType.vegPrice * sellVegType.selectVegNum;
            }   
        }

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            totalPriceAll = totalPriceAll + sellVegType.totalPrice;
        }

        totalPriceAllText.SetText("$" + totalPriceAll);
        
    }
    public void MinusButtonUp()
    {
        minusButtonDownFlag = false;
    }

    public void AddSellVegetableAll(string inputVegName)
    {
        totalPriceAll = 0;

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            if (sellVegType.sellVegName == inputVegName)
            {
                foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
                {
                    if (vegetable.vegetableName == inputVegName && sellVegType.selectVegNum < 999 && sellVegType.selectVegNum < vegetable.vegetableNum)
                    {
                        sellVegType.selectVegNum = vegetable.vegetableNum;
                    }
                }
                sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
                sellVegType.totalPrice = sellVegType.vegPrice * sellVegType.selectVegNum;
            }
        }

        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            totalPriceAll = totalPriceAll + sellVegType.totalPrice;
        }

        totalPriceAllText.SetText("$" + totalPriceAll);

    }

    public void Sell()
    {
        if (!isActive && totalPriceAll != 0)
        {
            sellCheck.SetActive(true);
            isActive = true;
            totalPriceAllCkText.SetText("$" + totalPriceAll);
        }
    }

    public void SellYes()
    {
        GManager.instance.money = GManager.instance.money + totalPriceAll;

        foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
        {
            foreach (SellVegetableType sellVegType in sellVegTypes)
            {
                if (vegetable.vegetableName == sellVegType.sellVegName)
                {
                    vegetable.vegetableNum = vegetable.vegetableNum - sellVegType.selectVegNum;
                }
            }
                   
        }
        ResetNumber();
        sellCheck.SetActive(false);
        isActive = false;
        GManager.instance.SaveData();
    }

    public void SellNo()
    {
        sellCheck.SetActive(false);
        isActive = false;
    }

    public void ResetNumber()
    {
        foreach (SellVegetableType sellVegType in sellVegTypes)
        {
            sellVegType.totalPrice = 0;
            totalPriceAll = 0;
            totalPriceAllText.SetText("$" + totalPriceAll);
            sellVegType.selectVegNum = 0;
            sellVegType.counter.SetText(sellVegType.selectVegNum.ToString("D3"));
        }
    }
}
