using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager.Scenes
{
    /// <summary>
    /// シーンの追加読込みと破棄を行うクラス
    /// </summary>
	public class SceneLoader : MonoBehaviour, ISceneLoader
	{
		[SerializeField]
		List<string>	_fstLoading = new List<string>(5);

		HashSet<string>	_loadingNames = new HashSet<string>();

        public event SceneLoadedHandler sceneLoaded = (s,m)=>{};
        public event SceneUnloadedHandler sceneUnloaded = (s)=>{};

        /// <summary>
        /// インスタンス生成直後に実行される処理
        /// </summary>
        void Awake ()
		{
			Scene scene;

			// エディタ上で開かれているシーンをチェック
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				scene = SceneManager.GetSceneAt(i);
				_loadingNames.Add(scene.name);
			}

			// 初回読込み分をチェック
			for (int i = 0; i < _fstLoading.Count; i++)
			{
				// 開かれていないシーンを読み込む
				if (!_loadingNames.Contains(_fstLoading[i]))
					LoadScene(_fstLoading[i]);
			}

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			_fstLoading.Clear();
			_fstLoading = null;
		}
		
		/// <summary>
		/// 初回のみ実行される更新処理。
		/// 全てのAwake関数を実行した後に呼ばれる
		/// </summary>
		void Start ()
		{
		}
		
		/// <summary>
		/// 終了処理
		/// </summary>
		void OnDestroy ()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			sceneLoaded = null;
			sceneUnloaded = null;
			_loadingNames.Clear();
			_loadingNames = null;
		}
		
        /// <summary>
		/// ISceneLoaderを参照
        /// </summary>
        /// <returns></returns>
        public bool IsLoaded()
		{
			return (_loadingNames.Count == 0);
		}

		/// <summary>
		/// シーンの読込みが完了したときの処理
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="mode"></param>
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Log.Debug("シーン({0})の読込みが完了", scene.name);
			_loadingNames.Remove(scene.name);
			sceneLoaded(scene.name, mode);
		}

		/// <summary>
		/// シーンの破棄が完了したときの処理
		/// </summary>
		/// <param name="scene"></param>
		void OnSceneUnloaded(Scene scene)
		{
			Log.Debug("シーン({0})の破棄が完了", scene.name);
			sceneUnloaded(scene.name);
		}

		/// <summary>
		/// ISceneLoaderを参照
		/// </summary>
		/// <param name="sceId"></param>
        public void LoadScene(SceneId sceId)
        {
			LoadScene(sceId.ToString());
        }

		/// <summary>
		/// ISceneLoaderを参照
		/// </summary>
		/// <param name="sceName"></param>
        public void LoadScene(string sceName)
        {
			Log.Debug("シーン({0})の読込みを指示", sceName);
			Scene scene = SceneManager.GetSceneByName(sceName);

			if (scene.isLoaded)
			{
				Log.Warning("シーン({0})は既に存在します", sceName);
				return;
			}

			_loadingNames.Add(sceName);
			SceneManager.LoadSceneAsync(sceName, LoadSceneMode.Additive);
        }

		/// <summary>
		/// ISceneLoaderを参照
		/// </summary>
		/// /// <param name="sceId"></param>
        public void UnloadScene(SceneId sceId)
        {
			UnloadScene(sceId.ToString());
        }

		/// <summary>
		/// ISceneLoaderを参照
		/// </summary>
		/// <param name="sceName"></param>
        public void UnloadScene(string sceName)
        {
			Log.Debug("シーン({0})の破棄を指示", sceName);
			Scene scene = SceneManager.GetSceneByName(sceName);

			if (!scene.isLoaded)
			{
				Log.Warning("シーン({0})は存在しません", sceName);
				return;
			}

			SceneManager.UnloadSceneAsync(sceName);
        }
    }
}
