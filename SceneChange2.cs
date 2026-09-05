using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    public GameObject[] GameObjectsToDeact;
    public GameObject menu;
    public Popup menuPopupScr;
    private bool isMainScene = true;
    public bool[] isSubScene;
    public Popup_Parent popup_PaScr;
    public Popup popupScr;
    public SellVegetable sellVegScr;
    public BuySeed buySedScr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainScene()
    {
        if (!isMainScene)
        {
            foreach (GameObject obj in GameObjectsToDeact)
            {
                obj.SetActive(false);
            }
            menu.SetActive(false);
            menuPopupScr.isActive = false;
            isMainScene = true;

            for (int j = 0; j < isSubScene.Length; j++)
            {
                isSubScene[j] = false;
            }
            popup_PaScr.isActive = false;
            popupScr.isActive = false;
            sellVegScr.isActive = false;
            buySedScr.isBuyCkActive = false;
            buySedScr.isCantBuyActive = false;
            sellVegScr.ResetNumber();
        }
        
    }

    public void LoadSubScene(int i)
    {
        if (!isSubScene[i])
        {
            foreach (GameObject obj in GameObjectsToDeact)
            {
                obj.SetActive(false);
            }
            GameObjectsToDeact[i].SetActive(true);
            menu.SetActive(false);
            menuPopupScr.isActive = false;
            isMainScene = false;
            for (int j = 0; j < isSubScene.Length; j++)
            {
                isSubScene[j] = false;
            }
            isSubScene[i] = true;
            popup_PaScr.isActive = false;
            popupScr.isActive = false;
            sellVegScr.isActive = false;
            buySedScr.isBuyCkActive = false;
            buySedScr.isCantBuyActive = false;
            sellVegScr.ResetNumber();
        }
        
    }
}
