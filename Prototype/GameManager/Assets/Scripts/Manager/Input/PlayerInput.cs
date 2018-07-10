using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager.Input
{
	/// <summary>
	/// プレイヤーの入力を監視するクラス
	/// </summary>
	[RequireComponent(typeof(KeyConfig))]
	public class PlayerInput : MonoBehaviour, IPlayerInput
	{
		IKeyConfig	_keyConfig;
		IKeyMap		_keyMap;
		Dictionary<int, EventSender> _events;

		[SerializeField]
		float _delayHold = 0.2f;

		[SerializeField]
		int	_inputActionCount = 10;

		/// <summary>
		/// 入力イベントを送信するクラス
		/// </summary>
        class EventSender
		{
			public event PlayerInputAction inputEvent;
			PlayerInputData	_inputData;

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="keyId">入力に使用するキーID</param>
			public EventSender(int keyId)
			{
				inputEvent = (e) => {};
				_inputData = new PlayerInputData(keyId);
			}

			/// <summary>
			/// 入力データを取得する
			/// </summary>
			/// <returns></returns>
			public PlayerInputData InputData
			{
				get { return _inputData; }
			}

			/// <summary>
			/// イベントを送信する
			/// </summary>
			public void Send()
			{
				inputEvent(_inputData);
			}

			/// <summary>
			/// 内部データをクリアする
			/// </summary>
			public void Clear()
			{
				inputEvent = null;
				_inputData = null;
			}
		}

		/// <summary>
		/// 入力許可の有無を設定／取得する
		/// </summary>
		/// <returns></returns>
        public bool InputEnable 
		{ 
			get { return base.enabled; }
			set { base.enabled = value; }
		}

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		void Awake ()
		{
			_keyConfig = GetComponent<IKeyConfig>();
			_events = new Dictionary<int, EventSender>(_inputActionCount);
		}
		
		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		/// </summary>
		void Start ()
		{
		}
		
		/// <summary>
		/// オブジェクトが有効/アクティブになったときの処理。
		/// Awake関数の直後に呼ばれる
		/// </summary>
		void OnEnable ()
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
		void OnDestroy ()
		{
			foreach (var val in _events.Values)
				val.Clear();

			_events.Clear();
			_events = null;
		}

		/// <summary>
		/// 毎フレーム実行される更新処理
		/// </summary>
		void Update ()
		{
			if (_keyMap == null)
				return;

            KeyData keyData;
			PlayerInputData	inputData;
            float unscaledTime = Time.unscaledTime;

			foreach (var pair in _events)
			{
				inputData = pair.Value.InputData;

				if (!_keyMap.TryGetKeyData(inputData.KeyId, out keyData))
					continue;
				
				if (!keyData.use)
					continue;

				inputData.inputState = GetInputState(keyData.code);

				// 押下
				if (inputData.IsPressed)
				{
					inputData.holding = false;
					inputData.pressTime = unscaledTime;

					pair.Value.Send();

					inputData.useHoldThreshold = true;
				}

				// 離上
				if (inputData.IsReleased)
				{
					inputData.holding = false;
					inputData.holdTime = 0;
					inputData.useHoldThreshold = false;

					pair.Value.Send();
				}

				// 押下中
				if (inputData.holding)
				{
					inputData.holdTime = unscaledTime - inputData.pressTime;
					pair.Value.Send();
				}
				// 押下中に移行する前
				else if (inputData.useHoldThreshold)
				{
					float delta = unscaledTime - inputData.pressTime;
					if (delta > _delayHold)
					{
						pair.Value.Send();

						inputData.holding = true;
						inputData.holdTime = 0;
						inputData.useHoldThreshold = false;
					}
				}
			}
		}

		/// <summary>
		/// 入力状態を取得する
		/// </summary>
		/// <param name="keyCode">キーコード</param>
		/// <returns></returns>
		PlayerInputData.InputState GetInputState(KeyCode keyCode)
		{
			bool downState = UnityEngine.Input.GetKeyDown(keyCode);
			bool upState = UnityEngine.Input.GetKeyUp(keyCode);
			PlayerInputData.InputState result;

            if (downState && upState)
                result = PlayerInputData.InputState.PressedAndReleased;
            else if (downState)
                result = PlayerInputData.InputState.Pressed;
            else if (upState)
                result = PlayerInputData.InputState.Released;
            else
                result = PlayerInputData.InputState.NotChanged;

            return result;
		}
		
		/// <summary>
		/// 指定したキーIDにイベントを登録
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="action">キー入力のイベント</param>
        public void RegistEvent(int keyId, PlayerInputAction action)
        {
			EventSender sender;

			if (_events.ContainsKey(keyId))
				sender = _events[keyId];
			else
			{
				sender = new EventSender(keyId);
				_events[keyId] = sender;
			}

			sender.inputEvent -= action;
			sender.inputEvent += action;
        }

		/// <summary>
		/// 指定したキー入力のイベントを削除する
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="action">削除するイベント</param>
        public void RemoveEvent(int keyId, PlayerInputAction action)
        {
			if (!_events.ContainsKey(keyId))
				return;

			_events[keyId].inputEvent -= action;
        }

		/// <summary>
		/// 指定したキーIDの入力許可の有無を設定する
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="enable">入力許可</param>
        public void SetKeyEnable(int keyId, bool enable)
        {
			if (_keyMap == null)
			{
				Log.Warning("キーマップが選択されていません（ID:{0}）", keyId);
				return;
			}
				
			_keyMap.SetKeyEnable(keyId, enable);
        }

		/// <summary>
		/// 使用するキーマップを選択
		/// </summary>
		/// <param name="keyMapId">使用するキーマップのID</param>
        public void SelectKeyMap(int keyMapId)
        {
			_keyMap = _keyConfig.GetKeyMap(keyMapId);
        }
    }
}
