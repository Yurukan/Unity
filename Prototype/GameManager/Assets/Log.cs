using System.Diagnostics;
using UnityEngine;

namespace Assets
{
	/// <summary>
	/// ログを出力するクラス
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// デバッグログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		[Conditional("DEBUG")]
		public static void Debug(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		/// <summary>
		/// デバッグログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="arg">置換文字列</param>
		[Conditional("DEBUG")]
		public static void Debug(object message, params object[] arg)
		{
			UnityEngine.Debug.Log(string.Format(message.ToString(), arg));
		}

		/// <summary>
		/// デバッグログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		[Conditional("DEBUG")]
		public static void Debug(object message, Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		/// <summary>
		/// デバッグログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		/// <param name="arg">置換文字列</param>
		[Conditional("DEBUG")]
		public static void Debug(object message, Object context, params object[] arg)
		{
			UnityEngine.Debug.Log(string.Format(message.ToString(), arg), context);
		}

		/// <summary>
		/// 情報ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		public static void Info(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		/// <summary>
		/// 情報ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="arg">置換文字列</param>
		public static void Info(object message, params object[] arg)
		{
			UnityEngine.Debug.Log(string.Format(message.ToString(), arg));
		}

		/// <summary>
		/// 情報ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		public static void Info(object message, Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		/// <summary>
		/// 情報ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		/// <param name="arg">置換文字列</param>
		public static void Info(object message, Object context, params object[] arg)
		{
			UnityEngine.Debug.Log(string.Format(message.ToString(), arg), context);
		}

		/// <summary>
		/// 警告ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		[Conditional("DEBUG")]
		public static void Warning(object message)
		{
			UnityEngine.Debug.LogWarning(message);
		}

		/// <summary>
		/// 警告ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="arg">置換文字列</param>
		[Conditional("DEBUG")]
		public static void Warning(object message, params object[] arg)
		{
			UnityEngine.Debug.LogWarning(string.Format(message.ToString(), arg));
		}

		/// <summary>
		/// 警告ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		[Conditional("DEBUG")]
		public static void Warning(object message, Object context)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}

		/// <summary>
		/// 警告ログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		/// <param name="arg">置換文字列</param>
		[Conditional("DEBUG")]
		public static void Warning(object message, Object context, params object[] arg)
		{
			UnityEngine.Debug.LogWarning(string.Format(message.ToString(), arg), context);
		}

		/// <summary>
		/// エラーログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		public static void Error(object message)
		{
			UnityEngine.Debug.LogError(message);
		}

		/// <summary>
		/// エラーログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="arg">置換文字列</param>
		public static void Error(object message, params object[] arg)
		{
			UnityEngine.Debug.LogError(string.Format(message.ToString(), arg));
		}

		/// <summary>
		/// エラーログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		public static void Error(object message, Object context)
		{
			UnityEngine.Debug.LogError(message, context);
		}

		/// <summary>
		/// エラーログを出力する
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="context">ログを出力したオブジェクト</param>
		/// <param name="arg">置換文字列</param>
		public static void Error(object message, Object context, params object[] arg)
		{
			UnityEngine.Debug.LogError(string.Format(message.ToString(), arg), context);
		}
	}
}
