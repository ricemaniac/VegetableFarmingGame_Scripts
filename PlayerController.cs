using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static PlayerController;

[System.Serializable]
public class ActionType
{
    public PlayerState actionState;
    public AudioClip actionSE;
    public string animationStates;
    public TileBase[] targetTiles;
    public TileBase replaceTile;
}

[System.Serializable]
public class SeedingType
{
    public string seedName;
    public TileBase sedTargetTile;
    public TileBase sedReplaceTile;
    public string seedAnimStates;
}

[System.Serializable]
public class WateringTile
{
    public TileBase watTargetTile;
    public TileBase watReplaceTile;
}

[System.Serializable]
public class HarvestTile
{
    public string havVegName;
    public TileBase havTargetTile;
}

public class PlayerController : MonoBehaviour
{
    // プレイヤーの移動に関する変数   
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;
    Vector2 notMove = new Vector2(0, 0);

    // アニメーションに関する変数
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // プレイヤーの農業アクションに関する変数
    public Transform tileCheck;
    public Tilemap tilemap;
    public Tilemap baseTilemap;
    public Tilemap wateringTilemap;
    public Tilemap fieldTilemap;
    public SeedingType[] seedingTypes;
    public string nowSeedName;
    bool isFarming = false;
    public PlayerState nowState = PlayerState.Default;
    public enum PlayerState
    {
        Default,
        Fertilize,
        Cultivate,
        Seeding,
        Watering,
        Harvest
    }
    public ActionType[] actionTypes;
    [SerializeField] private Vector3Int minAllowedPos; // チュートリアル中に許可する左下座標
    [SerializeField] private Vector3Int maxAllowedPos; // チュートリアル中に許可する右上座標
    public TileBase[] wateringTiles;
    public TileBase[] watTargetTiles;
    public HarvestTile[] harvestTiles;

    //プレイヤーのキーボード操作に関する変数
    public InputAction MoveAction;
    public InputAction FarmingAction;

    //プレイヤーのタップ操作に関する変数
    bool up = false;
    bool down = false;
    bool right = false;
    bool left = false;
    bool isWalk = false;

    //プレイヤーのSEに関する変数
    private AudioSource audioSource; // 歩行音用のAudioSource
    [Header("歩行SE")] public AudioClip walkSE;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        /*
        //プレイヤーのキーボード操作に関する命令（公開にあたり一時無効化）
        MoveAction.Enable();
        FarmingAction.Enable();
        FarmingAction.performed += Farming;
        FarmingAction.canceled += Farming;
        */
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのキーボード操作に関する命令
        move = MoveAction.ReadValue<Vector2>();
        
        if (up)
        {
            move = new Vector2(0, 1);
        }
        else if (down)
        {
            move = new Vector2(0, -1);
        }
        else if (right)
        {
            move = new Vector2(1, 0);
        }
        else if (left)
        {
            move = new Vector2(-1, 0);
        }        

        if (move == notMove) //動いていないとき、歩行音を鳴らさない
        {
            isWalk = false;
        }
        else
        {
            isWalk = true;
        }

        // --- SE再生処理 ---
        // ツールを選択していて、かつ移動ボタンを押している（isFarming）場合 -> 農作業SE
        if (isFarming && nowState != PlayerState.Default)
        {
            foreach(ActionType action in actionTypes)
            {
                if(action.actionState == nowState)
                {
                    audioSource.clip = action.actionSE;
                }
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if(isWalk) // ツール未選択の移動、または通常の歩行 -> 歩行SE
        {
            if (audioSource.clip != walkSE)
            {
                audioSource.clip = walkSE;
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        foreach (ActionType action in actionTypes)
        {
            if (action.actionState == nowState)
            {
                foreach(SeedingType seed in seedingTypes)
                {
                    if(seed.seedName == nowSeedName)
                    {
                        animator.SetBool(seed.seedAnimStates, true);
                    }
                    else
                    {
                        animator.SetBool(seed.seedAnimStates, false);
                    }
                }
                animator.SetBool(action.animationStates, true);
            }
            else
            {
                animator.SetBool(action.animationStates, false);
            }
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // --- アクション処理 ＆ アニメーション解除 ---
        if (isFarming && nowState != PlayerState.Default)
        {
            Vector3Int tileCheckPos = tilemap.WorldToCell(tileCheck.position);

            // 畑の上にいる場合
            // tilemap.HasTile -> タイルが設定(描画)されている座標であるか判定
            if (tilemap.HasTile(tileCheckPos))
            {
                animator.SetBool("isAction", true);

                foreach (ActionType action in actionTypes)
                {
                    if (action.actionState == nowState)
                    {

                        if (TutorialManager.Instance != null && !TutorialManager.Instance.IsTutorialCompleted)
                        {
                            if (!IsInsideAllowedArea(tileCheckPos))
                            {
                                Debug.Log("チュートリアル中はここを書き換えられません！");
                                break; // 処理を中断（タイルを置かせない）
                            }
                        }
                        switch (nowState)
                        {
                            case PlayerState.Fertilize: //肥料の場合
                                if (fieldTilemap.GetTile(tileCheckPos) == null && wateringTilemap.GetTile(tileCheckPos) == null)
                                {
                                    for (int i = 0; i < action.targetTiles.Length; i++)
                                    {

                                        if (baseTilemap.GetTile(tileCheckPos) == action.targetTiles[i])
                                        {
                                            fieldTilemap.SetTile(tileCheckPos, action.replaceTile);
                                            break;
                                        }
                                    }
                                }
                                break;

                            case PlayerState.Cultivate: //耕す場合
                                if (wateringTilemap.GetTile(tileCheckPos) == null)
                                {
                                    for (int i = 0; i < action.targetTiles.Length; i++)
                                    {

                                        if (fieldTilemap.GetTile(tileCheckPos) == action.targetTiles[i])
                                        {
                                            fieldTilemap.SetTile(tileCheckPos, action.replaceTile);
                                            break;
                                        }
                                    }
                                }
                                break;

                            case PlayerState.Seeding: //種まきの場合
                                if (wateringTilemap.GetTile(tileCheckPos) == null)
                                {
                                    foreach (SeedingType seed in seedingTypes)
                                    {
                                        if (seed.seedName == nowSeedName)
                                        {
                                            if (fieldTilemap.GetTile(tileCheckPos) == seed.sedTargetTile)
                                            {
                                                // 特定のタイルと一致している場合は別のタイルを設定する
                                                fieldTilemap.SetTile(tileCheckPos, seed.sedReplaceTile);
                                            }
                                        }
                                    }
                                }
                                break;

                            case PlayerState.Watering: //水やりの場合
                                if (wateringTilemap.GetTile(tileCheckPos) == null)
                                {
                                    foreach (SeedingType seed in seedingTypes)
                                    {
                                        if (fieldTilemap.GetTile(tileCheckPos) == seed.sedReplaceTile)
                                        {
                                            for (int j = 0; j < wateringTiles.Length; j++)
                                            {
                                                if (baseTilemap.GetTile(tileCheckPos) == watTargetTiles[j])
                                                {
                                                    wateringTilemap.SetTile(tileCheckPos, wateringTiles[j]);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                            case PlayerState.Harvest: //収穫の場合
                                foreach (HarvestTile havTile in harvestTiles)
                                {
                                    if (fieldTilemap.GetTile(tileCheckPos) == havTile.havTargetTile)
                                    {
                                        fieldTilemap.SetTile(tileCheckPos, null);
                                        GManager.instance.AddVegetableNum(havTile.havVegName);
                                    }
                                }
                                break;
                        }
                        

                    }
                }
            }

        }
        else
        {
            animator.SetBool("isAction", false);
        }
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    // 方向と押し下げ状態（true=押した, false=離した）をまとめて受け取るメソッド
    public void SetDirectionInput(string dirName)
    {
        // 押しっぱなし・ドラッグ中の連続呼び出しによるガタつきを防止
        if (dirName == "Up" && up) return;
        if (dirName == "Down" && down) return;
        if (dirName == "Left" && left) return;
        if (dirName == "Right" && right) return;

        // フラグの更新
        up = (dirName == "Up");
        down = (dirName == "Down");
        left = (dirName == "Left");
        right = (dirName == "Right");

        isFarming = true;
    }

    // すべての入力を一括リセットするメソッド
    public void ResetAllInput()
    {
        up = false;
        down = false;
        left = false;
        right = false;
        isFarming = false;
    }


    public void Farming(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isFarming = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isFarming = false;
        }
    }

    public void Fertilize()
    {
        if (nowState != PlayerState.Fertilize)
        {
            nowState = PlayerState.Fertilize;
        }
        else
        {
            nowState = PlayerState.Default;
        }
        nowSeedName = null;
    }

    public void Cultivate()
    {
        if (nowState != PlayerState.Cultivate)
        {
            nowState = PlayerState.Cultivate;
        }
        else
        {
            nowState = PlayerState.Default;
        }
        nowSeedName = null;
    }

    public void Seeding(string inputSedName)
    {
        foreach(SeedingType seed in seedingTypes)
        {
            if (seed.seedName == inputSedName)
            {
                if (nowState != PlayerState.Seeding)
                {
                    nowState = PlayerState.Seeding;
                    nowSeedName = inputSedName;
                }
                else
                {
                    if (nowSeedName != inputSedName)
                    {
                        nowSeedName = inputSedName;
                    }
                    else
                    {
                        nowState = PlayerState.Default;
                        nowSeedName = null;
                    }
                }
            }
        }  
    }

    public void Watering()
    {
        if (nowState != PlayerState.Watering)
        {
            nowState = PlayerState.Watering;
        }
        else
        {
            nowState = PlayerState.Default;
        }
        nowSeedName = null;
    }

    public void Harvest()
    {
        if (nowState != PlayerState.Harvest)
        {
            nowState = PlayerState.Harvest;
        }
        else
        {
            nowState = PlayerState.Default;
        }
        nowSeedName = null;
    }

    // 範囲内かどうかをチェックするヘルパーメソッド
    private bool IsInsideAllowedArea(Vector3Int pos)
    {
        return pos.x >= minAllowedPos.x && pos.x <= maxAllowedPos.x &&
               pos.y >= minAllowedPos.y && pos.y <= maxAllowedPos.y;
    }
}
