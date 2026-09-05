using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Walk : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject tutorialObject; // 歩くチュートリアルのゲームオブジェクト
    [SerializeField] private float targetTime = 3.0f; // 歩くチュートリアルの秒数
    [SerializeField] private PlayerController playerController; //プレイヤーコントローラー

    [Header("ステータス（確認用）")]
    [SerializeField] private float currentTotalTime = 0.0f; // 現在の合計時間
    [SerializeField] private bool isCleared = false; // 2回以上呼ばれないためのガード

    private int pressedButtonCount = 0;    // 現在押されているボタンの数

    void Update()
    {
        if (isCleared) return;

        if (pressedButtonCount > 0)        // いずれかのボタンが押されている場合、時間を進める
        {
            currentTotalTime += Time.deltaTime;
            if (currentTotalTime >= targetTime)            // 目標時間に達したか判定
            {
                OnTimerReached();
            }
        }
    }

    // ボタンが押されたときに実行するメソッド
    public void PushDown()
    {
        pressedButtonCount++;
    }

    // ボタンが離されたときに実行するメソッド
    public void PushUp()
    {
        pressedButtonCount = Mathf.Max(0, pressedButtonCount - 1);        // 念のためマイナスにならないように制限
    }

    // 時間に達したときの処理
    private void OnTimerReached()
    {
        if (playerController != null)
        {
            playerController.ResetAllInput();
        }
        else
        {
            Debug.LogError("PlayerControllerがインスペクターにセットされていません");
        }

        // 時間をリセットし、チュートリアル番号を次に進める
        currentTotalTime = 0;
        pressedButtonCount = 0;
        isCleared = true;
        TutorialManager.Instance.AdvanceTutorial();
    }
}
