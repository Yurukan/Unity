using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// キー設定へアクセスするインターフェイス
    /// </summary>
    public interface IKeyConfig
    {
        /// <summary>
        /// キーマップの数を取得する
        /// </summary>
        /// <returns></returns>
        int Count { get; }

        /// <summary>
        /// キー設定に割当てられたキーコードを変更する
        /// </summary>
        /// <param name="keyMapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="code">割当てるキーコード</param>
        void AlterKey(int keyMapId, int keyId, KeyCode code);

        /// <summary>
        /// キー情報を取得する
        /// </summary>
        /// <param name="keyMapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="keyData">取得するキー情報</param>
        void GetKeyData(int keyMapId, int keyId, out KeyData keyData);

        /// <summary>
        /// キー情報を取得する
        /// </summary>
        /// <param name="keyMapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="keyData">取得するキー情報</param>
        /// <returns>キー情報が存在するならtrue、それ以外はfalse</returns>
        bool TryGetKeyData(int keyMapId, int keyId, out KeyData keyData);

        /// <summary>
        /// ID指定されたキーマップを取得する
        /// </summary>
        /// <param name="keyMapId">キーマップID</param>
        /// <returns></returns>
        IKeyMap GetKeyMap(int keyMapId);

        /// <summary>
        /// ID指定されたキーマップを取得する
        /// </summary>
        /// <param name="keyMapId">キーマップID</param>
        /// <param name="keyMap">取得するキーマップ</param>
        /// <returns>キーマップが存在するならtrue、それ以外はfalse</returns>
        bool TryGetKeyMap(int keyMapId, out IKeyMap keyMap);

		/// <summary>
		/// キー設定を反映させる
		/// </summary>
		/// <param name="keys">キー設定</param>
        void ApplyKey(Dictionary<int, KeyCode> keys);

		/// <summary>
		/// デフォルトのキー設定を取得する
		/// </summary>
		/// <param name="keys">キー設定を格納するコンテナ</param>
        void CopyDefaultKey(Dictionary<int, KeyCode> keys);

		/// <summary>
		/// デフォルトのキー設定を反映させる
		/// </summary>
        void ApplyDefaultKey();
    }
}