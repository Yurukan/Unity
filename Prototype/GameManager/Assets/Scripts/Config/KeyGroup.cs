using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Manager.Input;

namespace Assets.Scripts.Config
{
    /// <summary>
    /// キー設定を変更するインターフェイス
    /// </summary>
    public interface IAlterKeyHandler
    {
        /// <summary>
        /// キー配置を変更する
        /// </summary>
        /// <param name="field">変更するフィールド</param>
        /// <param name="code">変更後のキーコード</param>
        void AlterKey(IKeyFieldAccesser field, KeyCode code);
    }

    /// <summary>
    /// キー変更を行うフィールドにキーコードを設定するインターフェイス
    /// </summary>
    public interface IKeyFieldAccesser
    {
        /// <summary>
        /// キーIDを取得する
        /// </summary>
        /// <returns></returns>
        int     KeyId { get; }

        /// <summary>
        /// キーコードを取得する
        /// </summary>
        /// <returns></returns>
        KeyCode Code { get; set; }
    }

    /// <summary>
    /// キー設定に対応するフィールドを持つクラス
    /// </summary>
    class KeyGroup
    {
        KeyAlterer[]    _alters;
        List<IKeyFieldAccesser>     _fields;
        Dictionary<int, KeyCode>    _keys;

        /// <summary>
        /// キー設定に含まれるキーフィールドを取得する
        /// </summary>
        /// <returns></returns>
        public List<IKeyFieldAccesser> Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// キー設定に含まれる設定されたキーを取得する
        /// </summary>
        /// <returns>キーIDとキーコードのコンテナ</returns>
        public Dictionary<int, KeyCode> Keys
        {
            get { return _keys; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyNum">キー設定に含むキーの数</param>
        /// <param name="keyConfig">キー設定のインターフェイス</param>
        public KeyGroup(int keyNum, IKeyConfig keyConfig)
        {
            _alters = new KeyAlterer[keyConfig.Count];
            _fields = new List<IKeyFieldAccesser>(keyNum);
            _keys = new Dictionary<int, KeyCode>(keyNum);

            IKeyMap map;
            for (int i = 1; i < _alters.Length; i++)
            {
                map = keyConfig.GetKeyMap(i);
                _alters[i] = new KeyAlterer(map.Count, _keys);
            }
        }

        /// <summary>
        /// キーフィールドを追加し、キーを変更するハンドラを取得する
        /// </summary>
        /// <param name="field"></param>
        /// <param name="handler"></param>
        public void AddKeyField(IKeyFieldAccesser field, out IAlterKeyHandler handler)
        {
            int index = DataId.GetData(field.KeyId);

            _fields.Add(field);
            _alters[index].AddKeyField(field);
            handler = _alters[index];
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Release()
        {
            _fields.Clear();
            _fields = null;

            for (int i = 1; i < _alters.Length; i++)
            {
                _alters[i].Release();
                _alters[i] = null;
            }
            _alters = null;
        }

        /// <summary>
        /// キーコードを一意に設定するクラス。対象はマップに含まれるキー全て
        /// </summary>
        class KeyAlterer : IAlterKeyHandler
        {
            Dictionary<int, KeyCode>	_svKeys;
            Dictionary<KeyCode, IKeyFieldAccesser> _uniKeys;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="keyNum">キー数</param>
            /// <param name="svKeys">キー配置を格納するコンテナ</param>
            public KeyAlterer(int keyNum, Dictionary<int, KeyCode> svKeys)
            {
                _svKeys = svKeys;
                _uniKeys = new Dictionary<KeyCode, IKeyFieldAccesser>(
                                keyNum, new InputUtility.KeyCodeEqualityComparer());
            }

            /// <summary>
            /// 終了処理
            /// </summary>
            public void Release()
            {
                _svKeys = null;
                _uniKeys.Clear();
                _uniKeys = null;
            }

            /// <summary>
            /// キー配置に対応するキーフィールドを追加
            /// </summary>
            /// <param name="field"></param>
            public void AddKeyField(IKeyFieldAccesser field)
            {
                if (!_svKeys.ContainsKey(field.KeyId))
                {
                    Log.Warning("フィールドIDがキー配置に対応していません（ID:{0:X8}）", field.KeyId);
                    return;
                }

                _uniKeys[_svKeys[field.KeyId]] = field;
            }

            /// <summary>
            /// IAlterKeyHandlerを参照
            /// </summary>
            /// <param name="field"></param>
            /// <param name="code"></param>
            public void AlterKey(IKeyFieldAccesser field, KeyCode code)
            {
                // 引数のキーコードが既に設定済み
                if (_uniKeys.ContainsKey(code))
                {
                    IKeyFieldAccesser duField = _uniKeys[code];
                    duField.Code = KeyCode.None;
                    _svKeys[duField.KeyId] = KeyCode.None;
                }
                
                field.Code = code;
                _uniKeys[code] = field;
                _svKeys[field.KeyId] = code;
            }
        }
    }
}