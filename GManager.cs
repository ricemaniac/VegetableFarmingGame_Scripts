using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

[System.Serializable]
public class VegetableType
{
    public string vegetableName;
    public bool isActive;
    public bool hasHarvested = false;
    public int vegetableNum;
    public GameObject vegNumTopTxt;
    public GameObject selectSedBut;
    public GameObject vegListContent;
    public GameObject vegBuyButton;
    public GameObject vegSoldOut;
}

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public int money = 0;
    private AudioSource audioSource = null;
    public VegetableType[] vegetableTypes;

    [Header("コンプリート判定設定")]
    public GameObject completePopup;  // 野菜コンプリート時に表示するポップアップ
    private bool hasCompletedVegetables = false; // 二重発火防止フラグ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadData();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (completePopup != null)
        {
            completePopup.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (VegetableType vegetable in vegetableTypes)
        {
            if (!vegetable.isActive)
            {
                if (vegetable.vegNumTopTxt != null) vegetable.vegNumTopTxt.SetActive(false);
                if (vegetable.selectSedBut != null) vegetable.selectSedBut.SetActive(false);
                if (vegetable.vegListContent != null) vegetable.vegListContent.SetActive(false);
                if (vegetable.vegBuyButton != null) vegetable.vegBuyButton.SetActive(true);
                if (vegetable.vegSoldOut != null) vegetable.vegSoldOut.SetActive(false);
            }
            else
            {
                if (vegetable.vegNumTopTxt != null) vegetable.vegNumTopTxt.SetActive(true);
                if (vegetable.selectSedBut != null) vegetable.selectSedBut.SetActive(true);
                if (vegetable.vegListContent != null) vegetable.vegListContent.SetActive(true);
                if (vegetable.vegBuyButton != null) vegetable.vegBuyButton.SetActive(false);
                if (vegetable.vegSoldOut != null) vegetable.vegSoldOut.SetActive(true);
            }
        }
    }

    public void AddVegetableNum(string inputName)
    {
        foreach (VegetableType vegetable in vegetableTypes)
        {
            if (vegetable.vegetableName == inputName)
            {
                vegetable.vegetableNum++;
                // 初めて収穫した野菜ならフラグを立てる
                vegetable.hasHarvested = true;
                break;
            }
        }

        // まだコンプしていなければ全種収穫チェックを行う
        if (!hasCompletedVegetables)
        {
            CheckAllCropsHarvested();
        }

        // セーブデータに「野菜収穫実績」「野菜コンプ済み」を保存
        SaveData();
    }

    // 全種類の野菜を収穫したかチェック
    private void CheckAllCropsHarvested()
    {
        // 配列の中のすべての野菜の hasHarvested が true かチェック
        foreach (VegetableType vegetable in vegetableTypes)
        {
            if (!vegetable.hasHarvested)
            {
                // 一つでも未収穫（false）の野菜があれば判定を中断
                return;
            }
        }

        // すべての野菜が true ならコンプ達成処理を呼び出し
        TriggerCompletePopup();
    }

    // コンプ達成処理
    private void TriggerCompletePopup()
    {
        hasCompletedVegetables = true;
        Debug.Log("全野菜収穫！");

        if (completePopup != null)
        {
            completePopup.SetActive(true);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);

        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }

    public void VedetableManager(string inputName)
    {
        foreach(VegetableType vegetable in vegetableTypes)
        {
            if(vegetable.vegetableName == inputName)
            {
                vegetable.isActive = true;
                break;
            }
        }

        // セーブデータに「野菜解放実績」を保存
        SaveData();
    }

    public void SaveData()
    {
        if (TutorialManager.Instance != null && !TutorialManager.Instance.IsTutorialCompleted)
        {
            Debug.Log("[セーブスキップ] チュートリアル未完了のため、データをセーブしません。");
            return;
        }

        // 所持金セーブ
        PlayerPrefs.SetInt("Money", money);
        Debug.Log($"[セーブ完了] 所持金: $ {money}");

        // 野菜解放実績、野菜収穫実績、野菜所持数セーブ
        foreach (VegetableType vegetable in vegetableTypes)
        {
            int activateFlag = vegetable.isActive ? 1 : 0;
            PlayerPrefs.SetInt($"isActive_{vegetable.vegetableName}", activateFlag);
            int harvestedFlag = vegetable.hasHarvested ? 1 : 0;
            PlayerPrefs.SetInt($"HasHarvested_{vegetable.vegetableName}", harvestedFlag);
            PlayerPrefs.SetInt($"VegetableNum_{vegetable.vegetableName}", vegetable.vegetableNum);
            Debug.Log($"[セーブ完了] {vegetable.vegetableName} - 解放: {vegetable.isActive} / 収穫歴: {vegetable.hasHarvested} / 所持数: {vegetable.vegetableNum}");
        }

        // 野菜コンプリート状況セーブ
        int vegetableCompleteFlag = hasCompletedVegetables ? 1 : 0;
        PlayerPrefs.SetInt("HasCompletedVegetables", vegetableCompleteFlag);
        Debug.Log($"[セーブ完了] HasCompletedVegetables: {hasCompletedVegetables}");

        // データの確定書き込み
        PlayerPrefs.Save();
        Debug.Log("オートセーブ完了");
    }

    public void LoadData()
    {
        // 所持金ロード
        money = PlayerPrefs.GetInt("Money", 0);
        Debug.Log($"[ロード完了] 所持金: $ {money}");

        // 野菜収穫実績、野菜所持数ロード
        foreach (VegetableType vegetable in vegetableTypes)
        {
            int isActive_default = vegetable.isActive ? 1 : 0;
            vegetable.isActive = (PlayerPrefs.GetInt($"isActive_{vegetable.vegetableName}", isActive_default) == 1);
            int hasHarvested_default = vegetable.hasHarvested ? 1 : 0;
            vegetable.hasHarvested = (PlayerPrefs.GetInt($"HasHarvested_{vegetable.vegetableName}", hasHarvested_default) == 1);
            vegetable.vegetableNum = PlayerPrefs.GetInt($"VegetableNum_{vegetable.vegetableName}", 0);
            Debug.Log($"[ロード完了] {vegetable.vegetableName} - 解放: {vegetable.isActive} / 収穫歴: {vegetable.hasHarvested} / 所持数: {vegetable.vegetableNum}");
        }

        // 野菜コンプリート状況ロード
        hasCompletedVegetables = (PlayerPrefs.GetInt("HasCompletedVegetables", 0) == 1);
        Debug.Log($"[ロード完了] HasCompletedVegetables: {hasCompletedVegetables}");
    }
}
