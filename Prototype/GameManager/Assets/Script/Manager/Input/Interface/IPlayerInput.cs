using System;

namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// 入力を検知したことを通知するイベント
    /// </summary>
    /// <param name="eventData">イベント情報</param>
    public delegate void PlayerInputAction(PlayerInputData eventData);

    /// <summary>
    /// キャラクターに対する入力を監視するクラスのインターフェイス
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        ///  プレイヤー入力の有無を設定／取得する
        /// </summary>
        /// <returns></returns>
        bool InputEnable { set; get; }

		/// <summary>
		/// 指定したキーIDにイベントを登録
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="action">キー入力のイベント</param>
        void RegistEvent(int keyId, PlayerInputAction action);

		/// <summary>
		/// 指定したキー入力のイベントを削除する
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="action">削除するイベント</param>
        void RemoveEvent(int keyId, PlayerInputAction action);

		/// <summary>
		/// 指定したキーIDの入力許可の有無を設定する
		/// </summary>
		/// <param name="keyId">キーID</param>
		/// <param name="enable">入力許可</param>
        void SetKeyEnable(int keyId, bool enable);

		/// <summary>
		/// 使用するキーマップを選択
		/// </summary>
		/// <param name="keyMapId">使用するキーマップのID</param>
        void SelectKeyMap(int keyMapId);
    }
}