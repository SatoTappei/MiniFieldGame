★優先タスク
	マップの自動生成
	フロアに障害物を配置する(移動しない敵)
	敵の移動アルゴリズム
	装備システム(縦、横に広く攻撃)の作成

★用語
	フロア…マップ全体のこと
	タイル…1マスのこと

★備考
	ゲーム内容
	ステージは全部で3つ
	マップの大きさと出てくる敵とコインの量とターン制限が変わる(増える)
	限られたターンでたくさん敵を倒す、コインを集めてゴールに行けばステージクリア
	3ステージのスコアを合算したのがリザルト

	敵は4種類つくりたい(出来れば)
	オイカケ 後を追いかける
	マチブセ 少し前を目指して、先回りをする
	キマグレ 点対称の位置を目指す
	オトボケ ランダムに行動する

	ゲームが重い時の対処
	URPAssetのShadow内のSoftShadowsのチェックを外す、DirectionalLightのShadowsのShadowTypeをHardShadowsにする

	AStarアルゴリズム
	Nodeクラスの実装

★使用アセット
	YughuesFreeGroundMaterials
	YughuesFreeMetalMaterials
★外部素材
	効果音ラボ
	Digital-Architex <= 木目調タイル

	WWWWWWWWWW
	WOOOOOOOWW
	WOOOOWWOOW
	WOOOOWWWOW
	WWOWWWWWOW
	WWOWWWWWOW
	WOOOOOWWOW
	WOOOOOOOOW
	WOOOOOWWEW
	WWWWWWWWWW