using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.UI;

[System.Serializable]
public class SeedType
{
    public string seedName;
    public TMP_Text sedPriceTag;
    public Image sedImage;
    public int sedPrice;
    public GameObject buyButton;
    public GameObject soldOut;
    public GameObject unavailable;
    public string prevSedName;
    public bool isPrevSedAvailable;
}


public class BuySeed : MonoBehaviour
{
    public SeedType[] seedTypes;
    [SerializeField] private string checkedSedName = null;
    [SerializeField] private int checkedSedPrice = 0;
    [SerializeField] private GameObject checkedBuyButton = null;
    [SerializeField] private GameObject checkedSoldOut = null;

    public GameObject buyCheck;
    public TMP_Text checkedSedPriceText;
    public Image checkedSedImage;
    public GameObject cantBuy;
    public bool isBuyCkActive = false;
    public bool isCantBuyActive = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SeedType seed in seedTypes)
        {
            seed.sedPriceTag.SetText("$" + seed.sedPrice.ToString());

            if (seed.isPrevSedAvailable == true || string.IsNullOrWhiteSpace(seed.prevSedName))
            {
                seed.unavailable.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SeedType seed in seedTypes)
        {
            foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
            {
                if (seed.prevSedName == vegetable.vegetableName)
                {
                    if (vegetable.isActive)
                    {
                        seed.isPrevSedAvailable = true;
                    }
                }
            }
        }

         foreach (SeedType seed in seedTypes)
        {
            if (seed.isPrevSedAvailable)
            {
                seed.unavailable.SetActive(false);
            }
        }
    }

    public void Buy(string inputSedName)
    {
        if (!isBuyCkActive && !isCantBuyActive)
        {
            foreach (SeedType seed in seedTypes)
            {
                if(seed.seedName == inputSedName)
                {
                    checkedSedPrice = seed.sedPrice;

                    if (checkedSedPrice > GManager.instance.money)
                    {
                        cantBuy.SetActive(true);
                        isCantBuyActive = true;
                    }
                    else
                    {
                        buyCheck.SetActive(true);
                        isBuyCkActive = true;
                        checkedSedName = seed.seedName;
                        checkedSedImage.sprite = seed.sedImage.sprite;
                        checkedSedPriceText.SetText("$" + seed.sedPrice);

                        foreach (VegetableType vegetable in GManager.instance.vegetableTypes)
                        {
                            if (vegetable.vegetableName == inputSedName)
                            {
                                checkedBuyButton = vegetable.vegBuyButton;
                                checkedSoldOut = vegetable.vegSoldOut;
                                break;
                            }
                        }
                    }
                }
            }  
        }

    }

    public void BuyYes()
    {
        GManager.instance.money = GManager.instance.money - checkedSedPrice;
        checkedSoldOut.SetActive(true);
        checkedBuyButton.SetActive(false);
        GManager.instance.VedetableManager(checkedSedName);
        ResetNumber();
        buyCheck.SetActive(false);
        isBuyCkActive = false;
        GManager.instance.SaveData();
    }

    public void BuyNo()
    {
        ResetNumber();
        buyCheck.SetActive(false);
        isBuyCkActive = false;
    }

    public void cantBuyClose()
    {
        cantBuy.SetActive(false);
        isCantBuyActive = false;
        ResetNumber();
    }

    public void ResetNumber()
    {
        checkedSedName = null;
        checkedSedPrice = 0;
        checkedBuyButton = null;
        checkedSoldOut = null;
    }

    public void ButtonCheck()
    {
        Debug.Log("click");
    }
}
