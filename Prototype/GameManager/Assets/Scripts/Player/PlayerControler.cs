using UnityEngine;
using Assets.Scripts.Manager.Input;

namespace Assets.Scripts.Player
{
	/// <summary>
	/// キャラクターのコントローラークラス
	/// </summary>
	public class PlayerControler : MonoBehaviour
	{
		IPlayerInput	_input;

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		void Awake ()
		{
			_input = GetComponent<IPlayerInput>();
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
			_input.RegistEvent(PKeyId.Attack, OnInputAttack);
			_input.RegistEvent(PKeyId.Jump, OnInputJump);
			_input.SelectKeyMap(PMapId.Normal);
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
		void OnDestroy ()
		{
			_input = null;
		}

		void OnInputAttack(PlayerInputData data)
		{
			Debug.Log("Call OnInputAttack.");
			Debug.Log(data.ToString());
		}

		void OnInputJump(PlayerInputData data)
		{
			Debug.Log("Call OnInputJump.");
			Debug.Log(data.ToString());
		}
	}
}
