using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Common;

namespace Assets.Scripts.Manager.Scenes
{
	[RequireComponent(typeof(SceneLoader))]
	public class SceneControler<T> : 
		SingletonMonoBehaviour<T> where T : SceneControler<T>
	{
		ISceneLoader	_loader;

		/// <summary>
		/// シーンを開始した時のイベント
		/// </summary>
		public event Action initialized = ()=>{};

		/// <summary>
		/// シーンを終了した時のイベント。
		/// 終了処理をこのクラスよりも先に行いたい時に利用する
		/// </summary>
		public event Action finalized = ()=>{};

		/// <summary>
		/// シーンを読み込むインターフェイスを取得
		/// </summary>
		/// <returns></returns>
		public ISceneLoader Loader
		{
			get { return _loader; }
		}

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		protected override void Awake ()
		{
			base.Awake();

			_loader = GetComponent<ISceneLoader>();
		}
		
		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		/// </summary>
		protected virtual IEnumerator Start ()
		{
			yield return new WaitUntil(_loader.IsLoaded);

			initialized();
		}
		
		/// <summary>
		/// 終了処理
		/// </summary>
		protected override void OnDestroy ()
		{
			finalized();
	
			_loader = null;
			initialized = null;
			finalized = null;

			base.OnDestroy();
		}

		/// <summary>
		/// シーンを遷移する
		/// </summary>
		/// <param name="sceId">シーンID</param>
		public void TransitionScene(SceneId sceId)
		{
			TransitionScene(sceId.ToString());
		}

		/// <summary>
		/// シーンを遷移する
		/// </summary>
		/// <param name="sceName">シーン名</param>
		public void TransitionScene(string sceName)
		{
			Log.Debug("シーン({0})の遷移を指示", sceName);
			SceneManager.LoadSceneAsync(sceName);
		}
	}
}
