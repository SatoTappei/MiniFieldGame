★優先度高めタスク

★優先度低めタスク
	ItemManagerの46行目のGetThisItem関数
	MapManager、221行目、床のタイルのリストを複製しているが、乱数の方をリストにするやり方を模索する
		不具合が出たときのために現在はコードを残している。不具合が出たらMapクラスのfloorListを消す
	PlaySceneManager:スコアの計算式を作る
	フロアに障害物を配置する(移動しない敵) <- 実装見送り

★備考
	ステージは全部で3つ
	マップの大きさと出てくる敵とコインの量とターン制限が変わる(増える)
	コインを集めてゴールに行けばステージクリア
	3ステージのスコアを合算したのがリザルト
	区域分割法のアルゴリズムを使う場合は水路に対応させる、壁のみの判定だとバグる
	ゲームが重い時はURPAssetのShadow内のSoftShadowsのチェックを外して
		DirectionalLightのShadowsのShadowTypeをHardShadowsにする
	
	◎穴掘り法でスタートとゴールが生成されるかのデバッグ完了

★使用アセット
	Furniture_ges1
★外部素材
	効果音ラボ
	甘茶の音楽工房
	Digital-Architex <= 木目調タイル

OOOOOOOOOOOOO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OWWWWWWWWWWWO
OOOOOOOOOOOOO