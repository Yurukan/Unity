using UnityEngine;

namespace Assets.Scripts.Manager.Input
{
	/// <summary>
	/// UI用のキー設定を行うクラス
	/// </summary>
	public class UIKeyConfig : KeyConfig
	{
        /// <summary>
        /// インスタンス生成直後に実行される処理
        /// </summary>
        protected void Awake()
        {
			base.Init(1);
			base.CreateKeyMap(UIMapId.Menu, UIKeyId.Num, KeyIdOffset.UI);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Submit, KeyCode.Z);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Cancel, KeyCode.X);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Left, KeyCode.LeftArrow);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Right, KeyCode.RightArrow);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Up, KeyCode.UpArrow);
			base.SetKeyData(UIMapId.Menu, UIKeyId.Down, KeyCode.DownArrow);
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
