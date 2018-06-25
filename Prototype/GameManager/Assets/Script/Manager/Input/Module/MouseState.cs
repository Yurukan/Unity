using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// マウスのイベント情報を保持するクラス
    /// </summary>
    class MouseState
    {
        PointerInputModule.MouseButtonEventData[] _mouseData;

        /// <summary>
        /// イベント情報の数を取得
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get { return _mouseData.Length; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MouseState(EventSystem eventSystem)
        {
            _mouseData = new PointerInputModule.MouseButtonEventData[3];
            _mouseData[0] = new PointerInputModule.MouseButtonEventData();
            _mouseData[0].buttonData = new PointerEventData(eventSystem);
            _mouseData[1] = new PointerInputModule.MouseButtonEventData();
            _mouseData[1].buttonData = new PointerEventData(eventSystem);
            _mouseData[2] = new PointerInputModule.MouseButtonEventData();
            _mouseData[2].buttonData = new PointerEventData(eventSystem);
        }

        /// <summary>
        /// 指定されたボタンのイベント情報を取得
        /// </summary>
        /// <param name="button">マウスのボタン。左、右、中ボタンの３種類</param>
        /// <returns>マウスボタンのイベント情報</returns>
        public PointerEventData GetEventData(PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
                return _mouseData[0].buttonData;
            else if (button == PointerEventData.InputButton.Right)
                return _mouseData[1].buttonData;
            else if (button == PointerEventData.InputButton.Middle)
                return _mouseData[2].buttonData;
            else
                throw new InvalidOperationException("対応していないマウスボタンが指定されました");
        }

        /// <summary>
        /// 指定されたボタンのイベント情報を取得
        /// </summary>
        /// <param name="buttonId">マウスのボタンId。左:0、右:1、中:2</param>
        /// <returns>マウスボタンのイベント情報</returns>
        public PointerEventData GetEventData(int buttonId)
        {
            if (buttonId < 0 || buttonId >= _mouseData.Length )
                throw new InvalidOperationException("対応していないマウスボタンが指定されました");

            return _mouseData[0].buttonData;
        }
        /// <summary>
        /// 指定されたボタンのイベント情報を取得
        /// </summary>
        /// <param name="buttonId">マウスのボタンId。左:0、右:1、中:2</param>
        /// <param name="eventData">イベント情報</param>
        /// <returns>イベント情報を生成したかの有無</returns>
        public bool GetEventData(int buttonId, out PointerEventData eventData)
        {
            PointerInputModule.MouseButtonEventData mouseEventData;
            bool result = GetButtonEventData(buttonId, out mouseEventData);

            eventData = mouseEventData.buttonData;
            return result;
        }

        /// <summary>
        /// 指定されたボタンのイベント情報を取得
        /// </summary>
        /// <param name="buttonId">マウスのボタンId。左:0、右:1、中:2</param>
        /// <param name="mouseEventData">イベント情報</param>
        /// <returns>イベント情報を生成したかの有無</returns>
        public bool GetButtonEventData(int buttonId, out PointerInputModule.MouseButtonEventData mouseEventData)
        {
            bool result = false;

            if (buttonId < 0 || buttonId >= _mouseData.Length )
                throw new InvalidOperationException("対応していないマウスボタンが指定されました");

            if (_mouseData[buttonId] == null)
            {
                result = true;
                _mouseData[buttonId] = new PointerInputModule.MouseButtonEventData();
            }

            mouseEventData = _mouseData[buttonId];
            return result;
        }

        /// <summary>
        /// 指定されたボタンの状態を設定する
        /// </summary>
        /// <param name="buttonId">マウスのボタンId。左:0、右:1、中:2</param>
        /// <param name="state">ボタンの入力状態</param>
        public void SetButtonState(int buttonId, PointerEventData.FramePressState state)
        {
            PointerInputModule.MouseButtonEventData mouseEventData;
            GetButtonEventData(buttonId, out mouseEventData);
            mouseEventData.buttonState = state;
        }

        /// <summary>
        /// いずれかのボタンの押下状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool AnyPressesThisFrame()
        {
            bool result = false;

            for (int i = 0; i < _mouseData.Length; i++)
                if (_mouseData[i].PressedThisFrame())
                {
                    result = true;
                    break;
                }
            
            return result;
        }

        /// <summary>
        /// いずれかのボタンの離上状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool AnyReleaseThisFrame()
        {
            bool result = false;

            for (int i = 0; i < _mouseData.Length; i++)
                if (_mouseData[i].ReleasedThisFrame())
                {
                    result = true;
                    break;
                }
            
            return result;
        }
    }
}