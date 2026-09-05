using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static PlayerController;
using UnityEngine.Tilemaps;

[System.Serializable]
public class SeedTranslator
{
    public string seedName;
    public string seedText;
}

public class SelectActionAndSeed : MonoBehaviour
{
    public GameObject selectAct;
    public GameObject selectSeed;
    public Popup_Parent popup_PaScr;
    public Popup popupScr;
    public PlayerController playerCon;
    public TMP_Text selectActBText;
    public string defaultText;
    public string fertilizeText;
    public string cultivateText;
    public string wateringText;
    public string harvestText;
    public PlayerState previousState;
    [SerializeField]private string previousSeedName;
    public SeedTranslator[] seedTranslatorts;

    // Start is called before the first frame update
    void Start()
    {
        previousState = playerCon.nowState;
        previousSeedName = playerCon.nowSeedName;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerCon.nowState)
        {
            case PlayerState.Default:
                selectActBText.SetText(defaultText);
                break;

            case PlayerState.Fertilize:
                selectActBText.SetText(fertilizeText);
                break;

            case PlayerState.Cultivate:
                selectActBText.SetText(cultivateText);
                break;

            case PlayerState.Watering:
                selectActBText.SetText(wateringText);
                break;

            case PlayerState.Harvest:
                selectActBText.SetText(harvestText);
                break;
        }

        if (playerCon.nowState != previousState)
        {
            if (playerCon.nowState != PlayerState.Seeding)
            {
                selectAct.SetActive(false);
                selectSeed.SetActive(false);
                popup_PaScr.isActive = false;
                popupScr.isActive = false;
            }
            previousState = playerCon.nowState;
            previousSeedName = null;
        }

        if (playerCon.nowSeedName != null && playerCon.nowSeedName != previousSeedName)
        {
            foreach (SeedTranslator seed in seedTranslatorts)
            {
                if (seed.seedName == playerCon.nowSeedName)
                {
                    selectActBText.SetText(seed.seedText);
                }
            }
            selectAct.SetActive(false);
            selectSeed.SetActive(false);
            popupScr.isActive = false;
            popup_PaScr.isActive = false;
            previousSeedName = playerCon.nowSeedName;
        }
    }
}
