namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// 入力モジュールのインターフェイス
    /// </summary>
    public interface IUIInputModule
    {
        bool IsUseMoveEvent { set; get; }
        bool IsUseSubmitEvent { set; get; }
        bool IsUseKeyEvent { set; get; }
        bool IsUseMouseEvent { set; get; }
        
        /// <summary>
        /// キーの使用可能を設定する
        /// </summary>
        /// <param name="keyId">使用中マップ内のキーID</param>
        /// <param name="enable">使用の有無</param>
        void SetEnableKey(int keyId, bool enable);

        /// <summary>
        /// UI入力に使用するキーマップを設定する
        /// </summary>
        /// <param name="keyMap">使用するキーマップ</param>
        void SetKeyMap(IKeyMap keyMap);
    }
}