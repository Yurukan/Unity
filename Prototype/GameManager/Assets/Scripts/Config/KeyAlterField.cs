﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Manager.Input;

namespace Assets.Scripts.Config
{
    /// <summary>
    /// キー設定を変更するUI
    /// </summary>
	public class KeyAlterField : Selectable, IUpdateSelectedHandler,
		IKeyDownHandler, IKeyFieldAccesser
	{
		readonly Color normalColor = new Color(1f, 1f, 1f, 0f);

		[SerializeField]
		Color	_highlightColor;

		[SerializeField]
		Color	_activeColor;

		[SerializeField]
		int		_offset;

		[SerializeField]
		int		_keyId;

		KeyCode	_code;
		bool	_isAlter;
		Text 	_dispName;
		IAlterKeyHandler	_alterHandler;

		/// <summary>
		/// 変更するキーIDを取得する
		/// </summary>
		/// <returns></returns>
		public int KeyId
		{
            get { return _keyId; }
		}

		/// <summary>
		/// フィールドが持つキーコードを設定／取得する
		/// </summary>
		/// <returns></returns>
		public KeyCode Code
		{
			get { return _code; }
			set 
			{
				_code = value;
				_dispName.text = InputUtility.GetKeyName(_code);
			}
		}

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			_dispName = GetComponentInChildren<Text>();

			ConfigManager.Instance.initialized += OnInitialized;
		}

		/// <summary>
		/// 管理クラスの初期処理が完了したときのイベント
		/// </summary>
		void OnInitialized()
		{
			ConfigManager.Instance.AddKeyField(this, out _alterHandler);
		}

		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		protected override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// インスペクター上でリセットされたときの処理
		/// </summary>
		protected override void Reset()
		{
			base.Reset();

			transition = Transition.None;
			targetGraphic.color = normalColor;
		}

		/// <summary>
		/// キー変更を開始
		/// </summary>
		void BeginAlterKey()
		{
			_isAlter = true;
			targetGraphic.color = _activeColor;
		}

		/// <summary>
		/// キー変更を終了
		/// </summary>
		/// <param name="isDeselect">選択を解除したかの有無</param>
		void EndAlterKey(bool isDeselect)
		{
			_isAlter = false;

			if (isDeselect)
				targetGraphic.color = Color.clear;
			else
				targetGraphic.color = _highlightColor;
		}

		/// <summary>
		/// ボタンを押下したときの処理
		/// </summary>
		/// <param name="eventData">キー入力のイベント情報</param>
        void IKeyDownHandler.OnKeyDown(KeyEventData eventData)
        {
			if (_isAlter)
				return;

			if (eventData.keyId == UIKeyId.Submit)
				BeginAlterKey();
        }

		/// <summary>
		/// UIを選択したときの処理
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			targetGraphic.color = _highlightColor;
		}

		/// <summary>
		/// UIを選択しているときの処理
		/// </summary>
		/// <param name="eventData"></param>
        void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
        {
			if (!_isAlter || !UnityEngine.Input.anyKeyDown)
				return;
			
			KeyCode newCode;
			if (InputUtility.CheckKeyPressed(out newCode))
			{
				_alterHandler.AlterKey(this, newCode);
				EndAlterKey(false);
			}
        }

		/// <summary>
		/// UIが非選択になったときの処理
		/// </summary>
		/// <param name="eventData"></param>
        public override void OnDeselect(BaseEventData eventData)
        {
			base.OnDeselect(eventData);
			EndAlterKey(true);
        }
    }
}
