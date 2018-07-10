using UnityEngine;

namespace Assets.Scripts.Common
{
	/// <summary>
	/// シングルトンパターンの機能を持った抽象クラス
	/// </summary>
	/// <typeparam name="T">シングルトンなクラス</typeparam>
	public abstract class SingletonMonoBehaviour<T> : 
							MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		static T _instance;

		/// <summary>
		/// シングルトンの対象となるインスタンスを取得する。
		/// インスタンスを所持していないときシーン上から取得する。
		/// </summary>
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));
					if (_instance == null)
					{
						Log.Warning(typeof(T) + " をアタッチしているGameObjectはありません");
					}
				}
				return _instance;
			}
		}

		/// <summary>
		/// インスタンス生成直後に実行される処理。
		/// DontDestroyOnLoadは無効
		/// </summary>
		protected virtual void Awake()
		{
			CheckInstance();
		}

		/// <summary>
		/// インスタンスの所有者かをチェック
		/// </summary>
		protected bool CheckInstance()
		{
			if (_instance == null)
			{
				_instance = this as T;
				return true;
			}
			else if (_instance == this)
			{
				return true;
			}

			Destroy(this);
			return false;
		}

		/// <summary>
		/// 終了処理
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (_instance == this)
				_instance = null;
		}
	}
}
