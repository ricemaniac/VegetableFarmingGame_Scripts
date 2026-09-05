using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // シングルトン
    public static TutorialManager Instance;

    [Header("--- チュートリアル設定 ---")]
    public GameObject[] tutorialSteps; 
    [SerializeField] private bool isTutorialCompleted = false;
    public bool IsTutorialCompleted => isTutorialCompleted || (currentIndex >= tutorialSteps.Length);

    [Header("--- デバッグ設定 ---")]
    [Tooltip("チェックを入れると、下の指定した番号からチュートリアルを強制開始します")]
    [SerializeField] private bool useDebugSkip = false;
    [SerializeField] private int debugStartIndex = 0;

    private int currentIndex = 0;

    void Awake()
    {
        // 初期設定
        Instance = this;
        LoadData();
    }

    void Start()
    {
        // エディタ実行かつデバッグフラグがONなら、開始位置を上書き
        //#if UNITY_EDITOR

        // 条件1: スキップフラグON ＆ 開始位置が正しい範囲内
        bool condition1 = useDebugSkip && debugStartIndex >= 0 && debugStartIndex < tutorialSteps.Length;
        // 条件2: スキップフラグON ＆ チュートリアル完了フラグON
        bool condition2 = useDebugSkip && isTutorialCompleted;

        if (condition1 || condition2)
        {
            currentIndex = debugStartIndex;
            Debug.Log($"<color=yellow>[Debug]</color> チュートリアルをインデックス {currentIndex} から強制開始します。");
        }
        //#endif

        // 最初のチュートリアルだけをオンにする
        if (!IsTutorialCompleted)
        {
            ActivateStep(currentIndex);
        }
    }

    // チュートリアル進行
    public void AdvanceTutorial()
    {
        Debug.Log($"AdvanceTutorialが呼ばれました。現在のインデックス: {currentIndex}");
        // 今のチュートリアルを非表示にして終了する
        tutorialSteps[currentIndex].SetActive(false);

        // 次の番号に進む
        currentIndex++;

        // もし次のチュートリアルがあれば、それを開始する
        if (currentIndex < tutorialSteps.Length)
        {
            ActivateStep(currentIndex);
        }
        else
        {
            Debug.Log("すべてのチュートリアルが終了しました");
            isTutorialCompleted = true;
            SaveData();

            if (GManager.instance != null)
            {
                GManager.instance.SaveData();
            }
        }
    }

    // 指定された番号のチュートリアルだけをアクティブにする
    void ActivateStep(int index)
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            // 一致する番号のオブジェクトだけをtrueにする
            tutorialSteps[i].SetActive(i == index);
        }
    }

    // セーブ処理
    public void SaveData()
    {
        if (!isTutorialCompleted)
        {
            Debug.Log("[セーブスキップ] チュートリアル途中のため、チュートリアル状態をセーブしません。");
            return;
        }

        // チュートリアル完了フラグ（完了なら1、未完了なら0）
        int tutorialCompleteFlag = isTutorialCompleted ? 1 : 0;
        PlayerPrefs.SetInt("IsTutorialCompleted", tutorialCompleteFlag);

        // データの確定書き込み
        PlayerPrefs.Save();
        Debug.Log("オートセーブ完了");
    }

    // ロード処理
    public void LoadData()
    {
        if (useDebugSkip)
        {
            Debug.Log("[ロード完了] デバッグスキップが有効のため、セーブデータのロードを無視しました。");
            return;
        }
        // セーブデータが無ければ初期値 0 が入る
        isTutorialCompleted = (PlayerPrefs.GetInt("IsTutorialCompleted", 0) == 1);
        Debug.Log($"[ロード完了] IsTutorialCompleted: {isTutorialCompleted}");
    }
}
