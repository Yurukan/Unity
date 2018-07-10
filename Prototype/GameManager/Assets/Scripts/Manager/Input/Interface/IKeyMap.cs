using UnityEngine;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// キー情報一覧のインターフェイス
    /// </summary>
    public interface IKeyMap
    {
        /// <summary>
        /// キーマップIDを取得する
        /// </summary>
        /// <returns></returns>
        int Id { get; }

        /// <summary>
        /// キー情報の数を取得する
        /// </summary>
        /// <returns></returns>
        int Count { get; }

        /// <summary>
        /// キーを使用するかを設定する
        /// </summary>
        /// <param name="keyId">設定するキーのID</param>
        /// <param name="isUse">使用フラグ</param>
        void SetKeyEnable(int keyId, bool isUse);

        /// <summary>
        /// 指定したIDのキー情報を取得する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="keyData">取得するキー情報</param>
        void GetKeyData(int keyId, out KeyData keyData);
 
        /// <summary>
        /// 指定したIDのキー情報を取得する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="keyData">取得するキー情報</param>
        /// <returns>true:キー情報が存在する。false:それ以外</returns>
        bool TryGetKeyData(int keyId, out KeyData keyData);

        /// <summary>
        /// 配列のインデックス指定されたキー情報を取得する
        /// </summary>
        /// <param name="index">キー情報配列のインデックス(>=1)</param>
        /// <param name="index">取得するキー情報</param>
        /// <returns>インデックスが有効であればtrue、それ以外はfalse</returns>
        bool TryGetKeyDataAtIndex(int index, out KeyData keyData);
    }

    /// <summary>
    /// 入力キーの情報
    /// </summary>
    public struct KeyData
    {
        public static readonly KeyData empty;

        static KeyData()
        {
            KeyData data;
            data.id = -1;
            data.code = KeyCode.None;
            data.defCode = KeyCode.None;
            data.use = false;

            empty = data;
        }
        
        /// <summary>
        /// 入力に関連付けられたID
        /// </summary>
        public int id;

        /// <summary>
        /// 入力に対応したキーコード
        /// </summary>
        public KeyCode code;

        /// <summary>
        /// デフォルトのキーコード
        /// </summary>
        public KeyCode defCode;

        /// <summary>
        /// キーを使用するかの有無
        /// </summary>
        public bool use;
    }
}