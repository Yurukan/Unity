using System.Text;

namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// 入力情報を扱うクラス
    /// </summary>
    public class PlayerInputData
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum InputState
        {
            Pressed  = 1,
            Released = 2,
            PressedAndReleased = 3,
            NotChanged = 4
        }

        int     _keyId;
        public InputState   inputState = 0;
        public float        pressTime = 0;
        public bool         useHoldThreshold = false;
        public bool         holding = false;
        public float        holdTime = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyId">キーID</param>
        public PlayerInputData(int keyId)
        {
            _keyId = keyId;
        }

        /// <summary>
        /// キーIDを取得する
        /// </summary>
        /// <returns></returns>
        public int KeyId 
        {
            get { return _keyId; }
        }

        /// <summary>
        /// 押下状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool IsPressed
        {
            get { return inputState == InputState.Pressed;}
        }

        /// <summary>
        /// 押中状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool IsHolding
        {
            get { return holding;}
        }

        /// <summary>
        /// 離上状態を取得する
        /// </summary>
        /// <returns></returns>
        public bool IsReleased
        {
            get { return inputState == InputState.Released;}
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<b>KeyId</b>: " + DataId.ToString(this._keyId));
            stringBuilder.AppendLine("<b>inputState</b>: " + this.inputState);
            stringBuilder.AppendLine("<b>pressTime</b>: " + this.pressTime);
            stringBuilder.AppendLine("<b>useHoldThreshold</b>: " + this.useHoldThreshold);
            stringBuilder.AppendLine("<b>holding</b>: " + this.holding);
            stringBuilder.AppendLine("<b>holdTime</b>: " + this.holdTime);
            return stringBuilder.ToString();
        }
    }
}