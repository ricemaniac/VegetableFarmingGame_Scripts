using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using TMPro;

public class VegetableGrow : MonoBehaviour
{
    public Tilemap tilemap; // InspectorからTilemapを設定する
    public Tilemap wateringTilemap; // InspectorからTilemapを設定する
    public Tilemap fieldTilemap; // InspectorからTilemapを設定する
    public VegetableGrowTiles[] vegGrowTiles;
    public float timeToSprout = 3.0f;
    public float timeToGrow = 3.0f; // タイマーの秒数
    private Dictionary<Vector3Int, float> sprTimerDic = new Dictionary<Vector3Int, float>();
    private Dictionary<Vector3Int, float> groTimerDic = new Dictionary<Vector3Int, float>();
    private List<Vector3Int> cellsToResetSprTimer = new List<Vector3Int>();
    private List<Vector3Int> cellsToResetGroTimer = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        SetTimer();
        Sprout();
        Grow();
        ResetTimer();
    }

    public void SetTimer()
    {
        foreach (var pos in fieldTilemap.cellBounds.allPositionsWithin)
        {
            // 取り出した位置情報からタイルマップ用の位置情報(セル座標)を取得
            Vector3Int cellPosition = new Vector3Int(pos.x, pos.y, pos.z);
            if(wateringTilemap.GetTile(cellPosition) != null)
            {
                for (int i = 0; i < vegGrowTiles.Length; i++)
                {
                    if (fieldTilemap.GetTile(cellPosition) == vegGrowTiles[i].targetTile[0])
                    {
                        if (!sprTimerDic.ContainsKey(cellPosition))
                        {
                            float sproutingTimer = 0;
                            sprTimerDic.Add(cellPosition, sproutingTimer);
                        }
                    }
                    if (fieldTilemap.GetTile(cellPosition) == vegGrowTiles[i].targetTile[1])
                    {
                        if (!groTimerDic.ContainsKey(cellPosition))
                        {
                            float growingTimer = 0;
                            groTimerDic.Add(cellPosition, growingTimer);
                        }
                    }
                }
            } 
        }
    }

    public void Sprout()
    {
        List<Vector3Int> cellPositionList = new List<Vector3Int>(sprTimerDic.Keys);

        foreach (Vector3Int cellPosition in cellPositionList)
        {
            if (sprTimerDic[cellPosition] < timeToSprout)
            {
                // 辞書内の値を更新
                sprTimerDic[cellPosition] += Time.deltaTime;
            }
            else
            {
                if (wateringTilemap.GetTile(cellPosition) != null)
                {
                    for (int j = 0; j < vegGrowTiles.Length; j++)
                    {
                        if (fieldTilemap.GetTile(cellPosition) == vegGrowTiles[j].targetTile[0])
                        {
                            fieldTilemap.SetTile(cellPosition, vegGrowTiles[j].replaceTile[0]);
                        }
                        cellsToResetSprTimer.Add(cellPosition);
                    }
                }
            }

        }
    }

    public void Grow()
    {
        List<Vector3Int> cellPositionList = new List<Vector3Int>(groTimerDic.Keys);

        foreach (Vector3Int cellPosition in cellPositionList)
        {
            if (groTimerDic[cellPosition] < timeToGrow)
            {
                // 辞書内の値を更新
                groTimerDic[cellPosition] += Time.deltaTime;
            }
            else
            {
                if (wateringTilemap.GetTile(cellPosition) != null)
                {
                    for (int k = 0; k < vegGrowTiles.Length; k++)
                    {
                        if (fieldTilemap.GetTile(cellPosition) == vegGrowTiles[k].targetTile[1])
                        {
                            fieldTilemap.SetTile(cellPosition, vegGrowTiles[k].replaceTile[1]);
                            wateringTilemap.SetTile(cellPosition, null);
                        }
                        cellsToResetGroTimer.Add(cellPosition);
                    }
                }
            }

        }
    }

    public void ResetTimer()
    {
        for (int i = cellsToResetSprTimer.Count - 1; i >= 0; i--)
        {
            var cell = cellsToResetSprTimer[i];
            sprTimerDic.Remove(cell); // sprTimerDicから削除
            cellsToResetSprTimer.RemoveAt(i); // cellsToResetSprTimerから該当のcellのみ削除
        }

        for (int i = cellsToResetGroTimer.Count - 1; i >= 0; i--)
        {
            var cell = cellsToResetGroTimer[i];
            groTimerDic.Remove(cell); // groTimerDicから削除
            cellsToResetGroTimer.RemoveAt(i); // cellsToResetGroTimerから該当のcellのみ削除
        }
    }
}

[System.Serializable]
public class VegetableGrowTiles
{
    public TileBase[] targetTile;
    public TileBase[] replaceTile;
}