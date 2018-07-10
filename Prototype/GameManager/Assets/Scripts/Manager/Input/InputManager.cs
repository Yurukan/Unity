using UnityEngine;
using Assets.Scripts.Common;
using System.Collections.Generic;

namespace Assets.Scripts.Manager.Input
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		IUIInputModule	_inputModule;
		IKeyConfig		_keyConfig;

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

			_inputModule = GetComponent<IUIInputModule>();
			_keyConfig = GetComponent<IKeyConfig>();
		}
		
		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		/// </summary>
		void Start ()
		{
			IKeyMap map = _keyConfig.GetKeyMap(UIMapId.Menu);
			_inputModule.SetKeyMap(map);
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

		/// <summary>
		/// 入力許可を設定
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="enable">入力許可の有無</param>
		public void SetKeyEnable(int keyId, bool enable)
		{
			if (!DataId.EqualsUpper(keyId, KeyIdOffset.UI))
			{
				Log.Error("キーIDの種類が異なります（ID:{0:X8}）", keyId);
				return;
			}
			
			int index = DataId.GetIndex(keyId);
			if (index <= 0)
			{
				Log.Error("キーIDのインデックスが不正（ID:{0:X8}）", keyId);
				return;
			}
			
			IKeyMap keyMap;
			if (!_keyConfig.TryGetKeyMap(index, out keyMap))
				return;
			
			keyMap.SetKeyEnable(keyId, enable);
		}
    }
}
