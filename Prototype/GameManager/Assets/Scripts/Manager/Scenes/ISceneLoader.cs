using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager.Scenes
{
    /// <summary>
    /// シーンの読込みが完了したときのイベントハンドラ
    /// </summary>
    /// <param name="sceName">シーン名</param>
    /// <param name="mode">読込みモード</param>
    public delegate void SceneLoadedHandler(string sceName, LoadSceneMode mode);

    /// <summary>
    /// シーンの破棄が完了したときのイベントハンドラ
    /// </summary>
    /// <param name="sceName">シーン名</param>
    public delegate void SceneUnloadedHandler(string sceName);
    
    /// <summary>
    /// シーンの追加読込みと破棄を行うクラスのインターフェイス
    /// </summary>
    public interface ISceneLoader
    {
        event SceneLoadedHandler sceneLoaded;
        event SceneUnloadedHandler sceneUnloaded;

        /// <summary>
        /// 全てのシーンの読込みが完了したかの有無を取得
        /// </summary>
        /// <returns></returns>
        bool IsLoaded();

        /// <summary>
        /// シーンの追加読込みを行う
        /// </summary>
        /// <param name="sceId">シーンID</param>
        void LoadScene(SceneId sceId);

        /// <summary>
        /// シーンの追加読込みを行う
        /// </summary>
        /// <param name="sceName">シーン名</param>
        void LoadScene(string sceName);

        /// <summary>
        /// 読み込まれているシーンを破棄する
        /// </summary>
        /// <param name="sceId">シーンID</param>
        void UnloadScene(SceneId sceId);

        /// <summary>
        /// 読み込まれているシーンを破棄する
        /// </summary>
        /// <param name="sceName">シーン名</param>
        void UnloadScene(string sceName);
    }
}