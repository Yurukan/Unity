using UnityEngine;
using Assets.Script.Manager.Input;

namespace Assets.Script.Player
{
	/// <summary>
	/// キャラクター用のキー設定を扱うクラス
	/// </summary>
	public class PlayerKeyConfig : KeyConfig
	{
		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		protected void Awake ()
		{
			base.Init(1);
			base.CreateKeyMap(PMapId.Normal, PKeyId.Num, KeyIdOffset.P1);
			base.SetKeyData(PMapId.Normal, PKeyId.Attack, KeyCode.Z);
			base.SetKeyData(PMapId.Normal, PKeyId.Jump, KeyCode.X);
		}
		
		/// <summary>
		/// 終了処理
		/// </summary>
		protected override void OnDestroy ()
		{
			base.OnDestroy();
		}
	}
}
