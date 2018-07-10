using System.Collections;
using UnityEngine;
using Assets.Scripts.Manager.Scenes;

public class MainControler : SceneControler<MainControler>
{
	/// <summary>
	/// インスタンス生成直後に実行される処理
	/// </summary>
	protected override void Awake ()
	{
		base.Awake();
	}
	
	/// <summary>
	/// 初回のみ実行される更新処理。
	/// 全てのAwake関数を実行した後に呼ばれる
	/// </summary>
	protected override IEnumerator Start ()
	{
		yield return base.Start();
	}
	
	/// <summary>
	/// 毎フレーム実行される更新処理
	/// </summary>
	void Update ()
	{
		
	}
	
	/// <summary>
	/// 終了処理
	/// </summary>
	protected override void OnDestroy ()
	{
		base.OnDestroy();
	}
}
