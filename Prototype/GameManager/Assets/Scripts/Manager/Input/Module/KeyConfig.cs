using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// キー設定を行う抽象クラス
    /// </summary>
	public class KeyConfig : MonoBehaviour, IKeyConfig
	{
		KeyMap[] _keyMaps;

        [SerializeField, KeyIdOffset()]
        int     _idOffset;

        /// <summary>
        /// キーIDオフセットを取得する
        /// </summary>
        /// <returns></returns>
        protected int IdOffset
		{
			get { return _idOffset; }
		}

        /// <summary>
        /// キーマップの数を取得する
        /// </summary>
        /// <returns></returns>
        public int Count 
        {
            get { return _keyMaps.Length; }
        }

		/// <summary>
		/// 終了処理
		/// </summary>
		protected virtual void OnDestroy()
        {
            _keyMaps = null;
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="mapNum">扱うキーマップの数</param>
        protected void Init(int mapNum)
        {
            _keyMaps = new KeyMap[mapNum + 1];
        }

        /// <summary>
        /// キーマップを生成する
        /// </summary>
        /// <param name="mapId">キーマップのID</param>
        /// <param name="keyNum">使用するキー情報の数</param>
        /// <param name="keyIdOffset">キー情報のIDオフセット</param>
        protected void CreateKeyMap(int mapId, int keyNum, int keyIdOffset)
        {
            if (mapId <= 0 || mapId >= _keyMaps.Length)
            {
                Log.Error("インデックスが不正（MID:{0}, Num:{1}）", 
                    mapId, _keyMaps.Length - 1);
                return;
            }

            _keyMaps[mapId] = new KeyMap(mapId, keyNum, keyIdOffset);
        }

        /// <summary>
        /// 指定したキー情報をマップに設定する
        /// </summary>
        /// <param name="mapId">キーマップのID</param>
        /// <param name="keyId">キーID</param>
        /// <param name="code">キーコード</param>
        protected void SetKeyData(int mapId, int keyId, KeyCode code)
        {
            if (mapId <= 0 || mapId >= _keyMaps.Length)
            {
                Log.Error("インデックスが不正（MID:{0}, Num:{1}）", 
                    mapId, _keyMaps.Length - 1);
                return;
            }
            
            if (_keyMaps[mapId] == null)
            {
                Log.Warning("キーマップが存在しません（MID:{0}）", mapId);
                return;
            }

            KeyData data;
            data = KeyData.empty;
            data.id = keyId;
            data.code = code;
            data.defCode = code;
            data.use = true;

            _keyMaps[mapId].SetKeyData(keyId, data);
        }

        /// <summary>
        /// キー設定に割当てられたキーコードを変更する
        /// </summary>
        /// <param name="mapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="code">割当てるキーコード</param>
        public void AlterKey(int mapId, int keyId, KeyCode code)
        {
            if (mapId <= 0 || mapId >= _keyMaps.Length)
            {
                Log.Error("インデックスが不正（MID:{0}, Num:{1}）", 
                    mapId, _keyMaps.Length - 1);
                return;
            }

            if (_keyMaps[mapId] == null)
            {
                Log.Warning("キーマップが存在しません（MID:{0}）", mapId);
                return;
            }

			KeyMap keyMap = _keyMaps[mapId];
            keyMap.AlterKeyCode(keyId, code);
        }

        /// <summary>
        /// キー情報を取得する
        /// </summary>
        /// <param name="mapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="keyData">取得するキー情報</param>
        public void GetKeyData(int mapId, int keyId, out KeyData keyData)
        {
            IKeyMap keyMap;

            if (!TryGetKeyMap(mapId, out keyMap))
            {
                keyData = KeyData.empty;
                return;
            }
            
            keyMap.GetKeyData(keyId, out keyData);
        }

        /// <summary>
        /// キー情報を取得する
        /// </summary>
        /// <param name="mapId">キーマップの番号</param>
        /// <param name="keyId">マップ内のキーID</param>
        /// <param name="keyData">取得するキー情報</param>
        /// <returns>キー情報が存在するならtrue、それ以外はfalse</returns>
        public bool TryGetKeyData(int mapId, int keyId, out KeyData keyData)
        {
            IKeyMap keyMap;

            if (!TryGetKeyMap(mapId, out keyMap))
            {
                keyData = KeyData.empty;
                return false;
            }
            
            return keyMap.TryGetKeyData(keyId, out keyData);
        }

        /// <summary>
        /// ID指定されたキーマップを取得する
        /// </summary>
        /// <param name="mapId">キーマップID</param>
        /// <returns></returns>
        public IKeyMap GetKeyMap(int mapId)
        {
            if (mapId <= 0 || mapId >= _keyMaps.Length)
            {
                Log.Error("インデックスが不正（MID:{0}, Num:{1}）", 
                    mapId, _keyMaps.Length - 1);
                return null;
            }
            
			return _keyMaps[mapId];
        }

        /// <summary>
        /// 配列のインデックス指定されたキーマップを取得する
        /// </summary>
        /// <param name="mapId">キーマップID</param>
        /// <param name="keyMap">取得するキーマップ</param>
        /// <returns>インデックスが有効であればtrue、それ以外はfalse</returns>
        public bool TryGetKeyMap(int mapId, out IKeyMap keyMap)
        {
            if (mapId <= 0 || mapId >= _keyMaps.Length)
            {
                Log.Error("インデックスが不正（MID:{0}, Num:{1}）", 
                    mapId, _keyMaps.Length - 1);
                keyMap = null;
                return false;
            }

            if (_keyMaps[mapId] == null)
            {
                Log.Warning("キーマップが存在しません（MID:{0}）", mapId);
                keyMap = null;
                return false;
            }

			keyMap = _keyMaps[mapId];
            return true;
        }

		/// <summary>
		/// キー設定を反映させる
		/// </summary>
		/// <param name="keys">キー設定</param>
        public void ApplyKey(Dictionary<int, KeyCode> keys)
        {
			int index;

			foreach (var pair in keys)
			{
                if (!DataId.EqualsUpper(pair.Key, _idOffset))
                {
                    Log.Error("キーIDの種類が異なります（ID:{0:X8}）", pair.Key);
                    return;
                }

				index = DataId.GetData(pair.Key);
                if (index <= 0)
                {
                    Log.Warning("キーIDのインデックスが不正（ID:{0:X8}）", pair.Key);
                    continue;
                }

                _keyMaps[index].AlterKeyCode(pair.Key, pair.Value);
			}
        }

		/// <summary>
		/// デフォルトのキー設定を取得する
		/// </summary>
		/// <param name="keys">キー設定を格納するコンテナ</param>
        public void CopyDefaultKey(Dictionary<int, KeyCode> keys)
        {
			KeyData keyData;

			keys.Clear();

			for (int i = 1; i < Count; i++)
			{
				if (_keyMaps[i] == null)
					continue;

				for (int j = 0; j < _keyMaps[i].Count; j++)
				{
					if (!_keyMaps[i].TryGetKeyDataAtIndex(j, out keyData))
						continue;
					
					keys[keyData.id] = keyData.defCode;
				}
			}
        }

		/// <summary>
		/// デフォルトのキー設定を反映させる
		/// </summary>
        public void ApplyDefaultKey()
        {
			for (int i = 1; i < Count; i++)
			{
				if (_keyMaps[i] == null)
					continue;

				for (int j = 0; j < _keyMaps[i].Count; j++)
                    _keyMaps[i].AlterDefaultKeyAtIndex(j);
			}
        }
    }
}
