using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Manager;
using Assets.Scripts.Manager.Input;

namespace Assets.Scripts.Config
{
	public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
	{
		public event Action initialized = ()=>{};
		
		IKeyConfig	_uiConfig;
		IKeyConfig	_plConfig;
		KeyGroup	_uiGroup;
		KeyGroup	_plGroup;

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
		void Start ()
		{
			_uiConfig = InputManager.Instance.KeyConfig;
			_plConfig = Player.PlayerManager.Instance.KeyConfig;
			_uiGroup = new KeyGroup(UIKeyId.Num, _uiConfig);
			_plGroup = new KeyGroup(PKeyId.Num,  _plConfig);

			GetSaveData();

			// 初期処理が完了したことを通知
			initialized();

			SetKeyFromSaveData();
		}
		
		/// <summary>
		/// 終了処理
		/// </summary>
		protected override void OnDestroy ()
		{
			_uiGroup.Release();
			_uiGroup = null;
			_plGroup.Release();
			_plGroup = null;
			initialized = null;

			base.OnDestroy();
		}

		/// <summary>
		/// キーフィールドを追加し、キーコードを設定するハンドラを取得する
		/// </summary>
		/// <param name="field">キーフィールド</param>
		/// <param name="handler">キーコードを設定するハンドラ</param>
		public void AddKeyField(IKeyFieldAccesser field, out IAlterKeyHandler handler)
		{
			if (DataId.EqualsUpper(field.KeyId, KeyIdOffset.UI))
			{
				_uiGroup.AddKeyField(field, out handler);
			}
			else if (DataId.EqualsUpper(field.KeyId, KeyIdOffset.P1))
			{
				_plGroup.AddKeyField(field, out handler);
			}
			else
			{
				Log.Error("キーフィールドのIDが不正（ID:{0:X8}）", field.KeyId);
				handler = null;
			}
		}

		/// <summary>
		/// セーブデータからキー設定を取得する
		/// </summary>
		void GetSaveData()
		{
			Dictionary<int, KeyCode> svUIKeys = _uiGroup.Keys;
			svUIKeys[UIKeyId.Submit] = KeyCode.Z;
			svUIKeys[UIKeyId.Cancel] = KeyCode.X;
			svUIKeys[UIKeyId.Left] = KeyCode.LeftArrow;
			svUIKeys[UIKeyId.Right] = KeyCode.RightArrow;
			svUIKeys[UIKeyId.Up] = KeyCode.UpArrow;
			svUIKeys[UIKeyId.Down] = KeyCode.DownArrow;

			Dictionary<int, KeyCode> svPKeys  = _plGroup.Keys;
			svPKeys[PKeyId.Attack] = KeyCode.Z;
			svPKeys[PKeyId.Jump] = KeyCode.X;
		}

		/// <summary>
		/// デフォルトのキー設定をフィールドに設定する
		/// </summary>
		public void SetKeyFromDefault()
		{
			_uiConfig.CopyDefaultKey(_uiGroup.Keys);
			SetKeyToField(_uiGroup.Keys, _uiGroup.Fields);

			_plConfig.CopyDefaultKey(_plGroup.Keys);
			SetKeyToField(_plGroup.Keys, _plGroup.Fields);
		}

		/// <summary>
		/// セーブデータのキー設定をフィールドに設定する
		/// </summary>
		public void SetKeyFromSaveData()
		{
			SetKeyToField(_uiGroup.Keys, _uiGroup.Fields);
			SetKeyToField(_plGroup.Keys, _plGroup.Fields);
		}

		/// <summary>
		/// フィールドにキーコードを設定する
		/// </summary>
		/// <param name="svKeys"></param>
		/// <param name="fields"></param>
		void SetKeyToField(Dictionary<int, KeyCode> svKeys, 
				List<IKeyFieldAccesser> fields)
		{
			int keyId;

			for (int i = 0; i < fields.Count; i++)
			{
				keyId = fields[i].KeyId;

				if (!svKeys.ContainsKey(keyId))
				{
					Log.Warning("対応するキーコードが存在しません（ID:{0:X8}）", keyId);
					continue;
				}

				fields[i].Code = svKeys[keyId];
			}
		}

		/// <summary>
		/// 設定を保存する
		/// </summary>
		public void SaveConfig()
		{
			_uiConfig.ApplyKey(_uiGroup.Keys);
			_plConfig.ApplyKey(_plGroup.Keys);

			// セーブデータに反映
		}
	}
}
