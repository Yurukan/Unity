using UnityEngine;
using Assets;
using Assets.Scripts.Manager.Scenes;

public class TestScript : MonoBehaviour
{
	/// <summary>
	/// インスタンス生成直後に実行される処理
	/// </summary>
	void Awake ()
	{
		MainControler.Instance.initialized += OnInitialized;
	}

	void OnInitialized()
	{
		Log.Debug("Call OnInitialized.");
		MainControler.Instance.initialized -= OnInitialized;
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
		if (Input.GetKeyUp(KeyCode.S))
			MainControler.Instance.TransitionScene(SceneId.SceMain2);
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
		
	}
}
