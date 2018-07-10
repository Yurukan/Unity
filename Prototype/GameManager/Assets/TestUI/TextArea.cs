using UnityEngine;
using Assets.Scripts.UI;

public class TextArea : MonoBehaviour
{
	ITextShower	_textShower;

	/// <summary>
	/// インスタンス生成直後に実行される処理
	/// </summary>
	void Awake ()
	{
		_textShower = GetComponent<ITextShower>();
	}
	
	/// <summary>
	/// 初回のみ実行される更新処理。
	/// 全てのAwake関数を実行した後に呼ばれる
	/// </summary>
	void Start ()
	{
		_textShower.ShowText();		
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
		
	}
}
