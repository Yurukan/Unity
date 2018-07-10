using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// キー入力に使用するイベン情報を扱うクラス
    /// </summary>
    public class KeyEventData : BaseEventData
    {
        int _keyId;

        public bool holding;
        public bool useHoldThreshold;
        public float pressTime;
        public float holdTime;
        public PointerEventData.FramePressState inputState;

        /// <summary>
        /// キーIDを取得する
        /// </summary>
        /// <returns></returns>
        public int keyId
        {
            get { return _keyId; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventSystem"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public KeyEventData(EventSystem eventSystem, int keyId) : base(eventSystem)
        {
            _keyId = keyId;
        }

        /// <summary>
        /// 押下状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool PressedThisFrame()
        {
            return inputState == PointerEventData.FramePressState.Pressed
                || inputState == PointerEventData.FramePressState.PressedAndReleased;
        }
 
        /// <summary>
        /// 離上状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool ReleasedThisFrame()
        {
            return inputState == PointerEventData.FramePressState.Released
                || inputState == PointerEventData.FramePressState.PressedAndReleased;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<b>KeyId</b>: " + this._keyId);
            stringBuilder.AppendLine("<b>inputState</b>: " + this.inputState);
            stringBuilder.AppendLine("<b>pressTime</b>: " + this.pressTime);
            stringBuilder.AppendLine("<b>useHoldThreshold</b>: " + this.useHoldThreshold);
            stringBuilder.AppendLine("<b>holding</b>: " + this.holding);
            stringBuilder.AppendLine("<b>holdTime</b>: " + this.holdTime);
            stringBuilder.AppendLine("<b>used</b>: " + this.used);
            stringBuilder.AppendLine("<b>selectedObject:</b>");
            stringBuilder.AppendLine(this.selectedObject.ToString());
            return stringBuilder.ToString();
        }
    }
}