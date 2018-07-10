using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// 入力キーの状態を保持するクラス
    /// </summary>
    class KeyState
    {
        Dictionary<int, KeyEventData> _data;
        EventSystem _eventSystem;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventDataCapacity">イベント情報を格納するコンテナの容量</param>
        /// <param name="eventSystem"></param>
        public KeyState(int eventDataCapacity, EventSystem eventSystem)
        {
            _eventSystem = eventSystem;
            _data = new Dictionary<int, KeyEventData>(eventDataCapacity);
        }

        /// <summary>
        /// 指定したキーIDのイベント情報を取得する
        /// </summary>
        /// <param name="keyId">キーID</param>
        /// <param name="eventData">取得したイベント情報</param>
        /// <returns>生成済のイベント情報を取得したかの有無</returns>
        public bool TryGetEventData(int keyId, out KeyEventData eventData)
        {
            if (_data.ContainsKey(keyId))
            {
                eventData = _data[keyId];
                return true;
            }

            eventData = new KeyEventData(_eventSystem, keyId);
            _data[keyId] = eventData;
            return false;
        }

        public bool AnyPressesThisFrame()
        {
            bool result = false;

            foreach (var pair in _data)
                if (pair.Value.PressedThisFrame())
                {
                    result = true;
                    break;
                }
            
            return result;
        }

        public bool AnyReleaseThisFrame()
        {
            bool result = false;

            foreach (var pair in _data)
                if (pair.Value.ReleasedThisFrame())
                {
                    result = true;
                    break;
                }
            
            return result;
        }
    }
}