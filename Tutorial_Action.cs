using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tutorial_Action : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject tutorialObject; // 歩くチュートリアルのゲームオブジェクト
    [SerializeField] private Tilemap targetTilemap; // チェック対象のタイルマップ
    [SerializeField] private TileBase targetTile; // 目標となるタイルのアセット
    [SerializeField] private Vector3Int minPosition; // チェックする範囲の左下（最小のXY座標）
    [SerializeField] private Vector3Int maxPosition; // チェックする範囲の右上（最大のXY座標）
    [SerializeField] private PlayerController playerController; //プレイヤーコントローラー

    [Header("ステータス（確認用）")]
    [SerializeField] private bool isCleared = false; // 2回以上呼ばれないためのガード

    void OnEnable()
    {
        if (tutorialObject != null)
        {
            tutorialObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCleared) return;
        OnActionStarted();
    }

    // 時間に達したときの処理
    private void OnActionStarted()
    {
        // タイル置換完了確認
        if (CheckAllTilesFilled())
        {
            Debug.Log("すべての指定範囲タイル置換完了。チュートリアルを進めます。");

            if (playerController != null)
            {
                // 相手のメソッドを呼び出す
                playerController.ResetAllInput();
            }
            else
            {
                Debug.LogError("PlayerControllerがインスペクターにセットされていません");
            }

            isCleared = true;
            TutorialManager.Instance.AdvanceTutorial();
        }
    }

    // 指定された範囲のタイルをすべて調べる自作メソッド
    private bool CheckAllTilesFilled()
    {
        // X座標の最小から最大までループ
        for (int x = minPosition.x; x <= maxPosition.x; x++)
        {
            // Y座標の最小から最大までループ
            for (int y = minPosition.y; y <= maxPosition.y; y++)
            {
                // 調べたいマスの座標を作成
                Vector3Int currentPos = new Vector3Int(x, y, minPosition.z);

                // そのマスのタイルを調べる
                TileBase currentTile = targetTilemap.GetTile(currentPos);

                // もし1箇所でも指定タイルではない場所があったら、その時点でfalseを返す
                if (currentTile != targetTile)
                {
                    return false;
                }
            }
        }

        // すべてのマスが指定タイルだった場合のみ、ここを通過してtrueを返す
        return true;
    }
}
