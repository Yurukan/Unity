using System;
using UnityEngine;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// キー情報一覧を扱うクラス
    /// </summary>
    class KeyMap : IKeyMap
    {
        int _id;
        KeyData[] _data;

        /// <summary>
        /// キーマップIDを取得する
        /// </summary>
        /// <returns></returns>
        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        /// キー情報の数を取得する
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get { return (_data != null) ? _data.Length : 0; } 
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyMapId">キーマップのID</param>
        /// <param name="keyNum">使用するキー情報の数</param>
        /// <param name="keyIdOffset">キー情報のIDオフセット</param>
        public KeyMap(int keyMapId, int keyNum, int keyIdOffset)
        {
            _id = keyMapId;
            _data = new KeyData[keyNum + 1];

            _data[0] = KeyData.empty;
            _data[0].id = keyIdOffset;

            for (int i = 1; i < _data.Length; i++)
                _data[i] = KeyData.empty;
        }

        /// <summary>
        /// キーIDから配列インデックスに変換する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="index">変換したインデックス</param>
        /// <returns>処理結果</returns>
        bool TryConvertIndex(int keyId, out int index)
        {
            if (!DataId.EqualsUpper(keyId, _data[0].id))
            {
                Log.Error("IDの種類が異なります（KID:{0:X8}, Offset:{1:X8}）",
                    keyId, _data[0].id);
                index = -1;
                return false;
            }

            if (DataId.GetData(keyId) != DataId.GetIndex(Id))
            {
                Log.Error("マップIDが異なります（KID:{0:X8}, MID:{1:X8}）", keyId, Id);
                index = -1;
                return false;
            }

            index = DataId.GetIndex(keyId);
            if (index <= 0 || index >= _data.Length)
            {
                Log.Error("インデックスが不正（KID:{0:X8}, Num:{1}）",
                    keyId, _data.Length - 1);
                index = -1;
                return false;
            }

            return true;
        }

        /// <summary>
        /// キー情報の内容をチェックする
        /// </summary>
        /// <param name="keyData">キー情報</param>
        /// <returns></returns>
        bool ValidateKeyData(KeyData keyData)
        {
            if (keyData.id <= 0)
            {
                Log.Error("キーIDが設定されていません（ID:{0:X8}）", keyData.id);
                return false;
            }

            return true;
        }

        /// <summary>
        /// キーを使用するかを設定する
        /// </summary>
        /// <param name="keyId">設定するキーのID</param>
        /// <param name="isUse">使用フラグ</param>
        public void SetKeyEnable(int keyId, bool isUse)
        {
            int index;

            if (!TryConvertIndex(keyId, out index))
                return;

            KeyData keyData = _data[index];

            if (!ValidateKeyData(keyData))
                return;
            
            keyData.use = isUse;
            _data[index] = keyData;
        }

        /// <summary>
        /// 指定したキー情報をマップに設定する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="keyData">設定するキー情報</param>
        public void SetKeyData(int keyId, KeyData keyData)
        {
            int index;

            if (!TryConvertIndex(keyId, out index))
                return;

            if (!ValidateKeyData(keyData))
                return;
            
            _data[index] = keyData;
        }

        /// <summary>
        /// 指定したキーIDのキーコードを変更する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="code">変更するキーコード</param>
        public void AlterKeyCode(int keyId, KeyCode code)
        {
            int index;

            if (!TryConvertIndex(keyId, out index))
                return;

            KeyData keyData = _data[index];

            if (!ValidateKeyData(keyData))
                return;

            keyData.code = code;
            keyData.use  = (keyData.code != KeyCode.None);
            _data[index] = keyData;
        }

        /// <summary>
        /// デフォルトのキーコードに変更する
        /// </summary>
        /// <param name="index">キー情報のインデックス</param>
        public void AlterDefaultKeyAtIndex(int index)
        {
            if (index <= 0 || index >= _data.Length)
            {
                Log.Warning("キー情報のインデックス範囲外です（MID:{0:X8})", Id);
                return;
            }

            KeyData keyData = _data[index];
            keyData.code = keyData.defCode;
            keyData.use  = (keyData.code != KeyCode.None);
            _data[index] = keyData;
        }

        /// <summary>
        /// 指定したIDのキー情報を取得する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="keyData">取得するキー情報</param>
        public void GetKeyData(int keyId, out KeyData keyData)
        {
            int index;

            if (!TryConvertIndex(keyId, out index))
            {
                keyData = KeyData.empty;
                return;
            }

            keyData = _data[index];
        }
 
        /// <summary>
        /// 指定したIDのキー情報を取得する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="keyData">取得するキー情報</param>
        /// <returns>true:キー情報が存在する。false:それ以外</returns>
        public bool TryGetKeyData(int keyId, out KeyData keyData)
        {
            int index;

            if (!TryConvertIndex(keyId, out index))
            {
                keyData = KeyData.empty;
                return false;
            }
            
            keyData = _data[index];

            return true;
        }
  
        /// <summary>
        /// 配列のインデックス指定されたキー情報を取得する
        /// </summary>
        /// <param name="index">キー情報配列のインデックス(>=1)</param>
        /// <param name="keyData">取得するキー情報</param>
        /// <returns>true:キー情報が存在する。false:それ以外</returns>
        public bool TryGetKeyDataAtIndex(int index, out KeyData keyData)
        {
            if (index <= 0 || index >= _data.Length)
            {
                Log.Warning("キー情報のインデックス範囲外です（MID:{0:X8})", Id);
                keyData = KeyData.empty;
                return false;
            }

			keyData = _data[index];

            return true;
        }
    }
}