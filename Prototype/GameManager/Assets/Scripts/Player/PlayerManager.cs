using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Manager;
using Assets.Scripts.Manager.Input;
using System.Collections.Generic;

namespace Assets.Scripts.Player
{
	[RequireComponent(typeof(PlayerKeyConfig))]
	[RequireComponent(typeof(PlayerControler))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
	{
		IKeyConfig	_keyConfig;

		/// <summary>
		/// キー設定を取得
		/// </summary>
		/// <returns></returns>
		public IKeyConfig KeyConfig
		{
			get { return _keyConfig; }
		}

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		protected override void Awake ()
		{
			base.Awake();

			_keyConfig = GetComponent<PlayerKeyConfig>();
		}
		
		/// <summary>
		/// オブジェクトが有効/アクティブになったときの処理。
		/// Awake関数の直後に呼ばれる
		/// </summary>
		void OnEnable ()
		{
			
		}
		
		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		/// </summary>
		void Start ()
		{
			
		}
		
		/// <summary>
		/// 毎フレーム実行される更新処理
		/// </summary>
		void Update ()
		{
			
		}
		
		/// <summary>
		/// オブジェクトが無効/非アクティブになったときの処理。
		/// </summary>
		void OnDisable ()
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
}
