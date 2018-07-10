using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	/// <summary>
	/// テキスト表示領域に文字列を表示するインターフェイス
	/// </summary>
	public interface ITextShower
	{
        /// <summary>
        /// テキストの表示した時のイベント
        /// </summary>
		event Action TextShowed;

        /// <summary>
        /// 表示するテキストを設定する
        /// </summary>
        /// <param name="text">表示する文字列</param>
        void SetText(string text);

        /// <summary>
        /// テキストを表示する
        /// </summary>
		void ShowText();
	}
}