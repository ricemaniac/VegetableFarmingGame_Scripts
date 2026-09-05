# 野菜栽培ゲーム (Vegetable Farming Game)

道具を駆使して野菜を育て、売却・購入を繰り返してコンプリートを目指す、
スマホブラウザで遊べるシンプルなシミュレーションゲームです。

## ゲーム概要
* **開発環境**: Unity (2022.3) / C#
* **主な機能**: 
  * 野菜の成長・水やり・収穫システム
  * 種の購入・野菜の売却機能（所持金管理）
  * `PlayerPrefs` を活用したオートセーブ / ロード機能
  * チュートリアル状態に応じた進行制限
  * 全野菜収穫時のコンプリート判定機能

## 主要スクリプト
* [GManager.cs](./Assets/Scripts/GManager.cs) - 所持金・野菜の取得状況・セーブロード・コンプ判定等の管理
* [PlayerController.cs](./Assets/Scripts/PlayerController.cs) - プレイヤーの移動・農作業アクション・タイル判定制御
* [SellVegetable.cs](./Assets/Scripts/SellVegetable.cs) - 野菜の売却制御
* [BuySeed.cs](./Assets/Scripts/BuySeed.cs) - 種の購入制御