using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager.Input
{
    /// <summary>
    /// UIに対する入力を監視とイベントを発行するクラス
    /// </summary>
    [RequireComponent(typeof(KeyConfig))]
    public class UIInputModule : MouseInputModule, IUIInputModule
    {
        IKeyMap _keyMap;
        Vector2 _lastMoveVector;
        float   _prevActionTime = 0;
        int     _consecutiveMoveCount = 0;
        KeyState    _keyState;
        bool    _isUseSubmit = true;
        bool    _isUseMove = true;
        bool    _isUseMouse = true;
        bool    _isUseKey = true;

        [SerializeField]
        int     _keyStateCapacity = 6;

        [SerializeField]
        float   _repeatDelay = 0.5f;

        [SerializeField]
        float   _inputActionsPerSecond = 10f;

        [SerializeField]
        float   _holdDelay = 0.3f;

        public bool IsUseMoveEvent
        {
            set { _isUseMove = value; }
            get { return _isUseMove; }
        }

        public bool IsUseSubmitEvent
        {
            set { _isUseSubmit = value; }
            get { return _isUseSubmit; }
        }

        public bool IsUseKeyEvent
        {
            set { _isUseKey = value; }
            get { return _isUseKey; }
        }

        public bool IsUseMouseEvent
        {
            set { _isUseMouse = value; }
            get { return _isUseMouse; }
        }

        /// <summary>
        /// UI入力に使用するキーマップを設定する
        /// </summary>
        /// <param name="keyMap">UI用のキーマップ</param>
        public void SetKeyMap(IKeyMap keyMap)
        {
            _keyMap = keyMap;
        }

        /// <summary>
        /// キーの使用可能を設定する
        /// </summary>
        /// <param name="keyId">使用中マップ内のキーID</param>
        /// <param name="enable">使用の有無</param>
        public void SetEnableKey(int keyId, bool enable)
        {
            if (_keyMap == null)
                return;

            _keyMap.SetKeyEnable(keyId, enable);
        }

        public override bool ShouldActivateModule()
        {
            bool result;
            if (!base.ShouldActivateModule())
            {
                result = false;
            }
            else
            {
                bool flag = false;

                if (_keyMap != null)
                {
                    KeyData keyData;
                    _keyMap.GetKeyData(UIKeyId.Submit, out keyData);
                    flag |= UnityEngine.Input.GetKeyDown(keyData.code);
                    _keyMap.GetKeyData(UIKeyId.Cancel, out keyData);
                    flag |= UnityEngine.Input.GetKeyDown(keyData.code);
                    flag |= CheckMovePress();
                }
                else
                    flag = true;

                result = flag;
            }
            return result;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();

            if (_keyState == null)
                _keyState = new KeyState(_keyStateCapacity, base.eventSystem);
        }

        /// <summary>
        /// UIのイベント処理
        /// </summary>
        public override void Process()
        {
            if (base.eventSystem.isFocused || !this.ShouldIgnoreEventsOnNoFocus())
            {
                if (_keyMap != null)
                {
                    bool flag = this.SendUpdateEventToSelectedObject();

                    if (base.eventSystem.sendNavigationEvents)
                    {
                        if (_isUseMove && !flag)
                            flag |= this.SendMoveEventToSelectedObject();

                        if (_isUseSubmit && !flag)
                            this.SendSubmitEventToSelectedObject();
                    }

                    if (_isUseKey)
                        ProcessKeyEvent();
                }

                if (_isUseMouse && base.input.mousePresent)
                    base.ProcessMouseEvent();
            }
        }

        /// <summary>
        /// 選択しているUIの更新イベントを通知
        /// </summary>
        /// <returns>イベントを利用したかの有無</returns>
        bool SendUpdateEventToSelectedObject()
        {
            bool result = false;

            if (base.eventSystem.currentSelectedGameObject != null)
            {
                BaseEventData eventData = this.GetBaseEventData();
                ExecuteEvents.Execute<IUpdateSelectedHandler>(
                    base.eventSystem.currentSelectedGameObject, eventData, 
                    ExecuteEvents.updateSelectedHandler);
                result = eventData.used;
            }

            return result;
        }

        /// <summary>
        /// 選択しているUI上のカーソルの移動イベントを通知
        /// </summary>
        /// <returns>イベントを利用したかの有無</returns>
        bool SendMoveEventToSelectedObject()
        {
            Vector2 move = GetMoveVector();

            // 移動なし
            if (move.x == 0f && move.y == 0f)
                return false;

            bool result = false;
            float unscaledTime = Time.unscaledTime;
            bool isMoveStart = CheckMovePress();
            bool isRepeat = false;
            bool isSameDir = Vector2.Dot(move, _lastMoveVector) > 0f;

            if (!isMoveStart)
            {
                // 同じ方向への移動中、移動イベントの繰り返しを判定
                if (isSameDir && _consecutiveMoveCount == 1)
                {   // 初回
                    isRepeat = (unscaledTime > _prevActionTime + _repeatDelay);
                }
                else
                {   // ２回目以降
                    isRepeat = (unscaledTime > _prevActionTime + 1f / _inputActionsPerSecond);
                }
            }

            if (isMoveStart || isRepeat)
            {
                AxisEventData axisEventData = this.GetAxisEventData(move.x, move.y, 0.6f);
                if (axisEventData.moveDir != MoveDirection.None)
                {
                    ExecuteEvents.Execute<IMoveHandler>(
                        base.eventSystem.currentSelectedGameObject, axisEventData,
                         ExecuteEvents.moveHandler);

                    if (!isSameDir)
                        _consecutiveMoveCount = 0;
                        
                    _consecutiveMoveCount++;
                    _prevActionTime = unscaledTime;
                    _lastMoveVector = move;
                }
                else
                {
                    _consecutiveMoveCount = 0;
                }
                result = axisEventData.used;
            }
            
            return result;
        }

        /// <summary>
        /// キー入力により移動方向を取得
        /// </summary>
        /// <returns>移動方向</returns>
        Vector2 GetMoveVector()
        {
            Vector2 move = Vector2.zero;
            KeyData keyData;

            _keyMap.GetKeyData(UIKeyId.Left, out keyData);
            if (UnityEngine.Input.GetKey(keyData.code))
            {
                move.x = -1f;
            }
            else
            {
                _keyMap.GetKeyData(UIKeyId.Right, out keyData);
                if (UnityEngine.Input.GetKey(keyData.code))
                {
                    move.x = 1f;
                }
            }

            _keyMap.GetKeyData(UIKeyId.Up, out keyData);
            if (UnityEngine.Input.GetKey(keyData.code))
            {
                move.y = 1f;
            }
            else
            {
                _keyMap.GetKeyData(UIKeyId.Down, out keyData);
                if (UnityEngine.Input.GetKey(keyData.code))
                {
                    move.y = -1f;
                }
            }

            return move;
        }

        /// <summary>
        /// いずれかの方向に押下されたかをチェック
        /// </summary>
        /// <returns>true:押下あり。false:押下中または入力なし</returns>
        bool CheckMovePress()
        {
            KeyData keyData;

            _keyMap.GetKeyData(UIKeyId.Left, out keyData);
            if (UnityEngine.Input.GetKeyDown(keyData.code))
                return true;

            _keyMap.GetKeyData(UIKeyId.Right, out keyData);
            if (UnityEngine.Input.GetKeyDown(keyData.code))
                return true;

            _keyMap.GetKeyData(UIKeyId.Up, out keyData);
            if (UnityEngine.Input.GetKeyDown(keyData.code))
                return true;

            _keyMap.GetKeyData(UIKeyId.Down, out keyData);
            if (UnityEngine.Input.GetKeyDown(keyData.code))
                return true;

            return false;
        }

        /// <summary>
        /// キー情報を取得
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        KeyEventData GetKeyEventData(int keyId)
        {
            KeyEventData eventData;

            _keyState.TryGetEventData(keyId, out eventData);
            eventData.Reset();
            return eventData;
        }

        /// <summary>
        /// 選択しているUIのボタンイベントを通知
        /// </summary>
        /// <returns>イベントを利用したかの有無</returns>
        bool SendSubmitEventToSelectedObject()
        {
            bool result;
            
            if (base.eventSystem.currentSelectedGameObject == null)
            {
                result = false;
            }
            else
            {
                KeyData keyData;
                BaseEventData baseEventData = this.GetBaseEventData();

                _keyMap.GetKeyData(UIKeyId.Submit, out keyData);
                if (UnityEngine.Input.GetKeyDown(keyData.code))
                {
                    ExecuteEvents.Execute<ISubmitHandler>(
                        base.eventSystem.currentSelectedGameObject, baseEventData,
                        ExecuteEvents.submitHandler);
                }

                _keyMap.GetKeyData(UIKeyId.Cancel, out keyData);
                if (UnityEngine.Input.GetKeyDown(keyData.code))
                {
                    ExecuteEvents.Execute<ICancelHandler>(
                        base.eventSystem.currentSelectedGameObject, baseEventData,
                        ExecuteEvents.cancelHandler);
                }

                result = baseEventData.used;
            }
            return result;
        }

        /// <summary>
        /// キー設定に基づくイベント処理
        /// </summary>
        void ProcessKeyEvent()
        {
            KeyEventData eventData;
            KeyData keyData;
            float unscaledTime = Time.unscaledTime;

            for (int i = 1; i < _keyMap.Count; i++)
            {
                if (!_keyMap.TryGetKeyDataAtIndex(i, out keyData))
                    continue;

                if (!keyData.use)
                    continue;
                    
                _keyState.TryGetEventData(keyData.id, out eventData);
                eventData.inputState = CheckKeyState(keyData.code);

                // 押下
                if (eventData.PressedThisFrame())
                {
                    eventData.holding = false;
                    eventData.pressTime = unscaledTime;

                    ExecuteEvents.Execute<IKeyDownHandler>(
                        base.eventSystem.currentSelectedGameObject, eventData, 
                        ExecuteFunctions.KeyDownHandler);

                    eventData.useHoldThreshold = true;
                }

                // 離上
                if (eventData.ReleasedThisFrame())
                {
                    if (eventData.holding)
                    {
                        ExecuteEvents.Execute<IKeyEndHoldHandler>(
                            base.eventSystem.currentSelectedGameObject, eventData, 
                            ExecuteFunctions.KeyEndHoldHandler);

                        eventData.holding = false;
                        eventData.holdTime = 0;
                    }
                    
                    eventData.useHoldThreshold = false;

                    ExecuteEvents.Execute<IKeyUpHandler>(
                        base.eventSystem.currentSelectedGameObject, eventData, 
                        ExecuteFunctions.KeyUpHandler);
                }

                // 押下中
                if (eventData.holding)
                {
                    eventData.holdTime = unscaledTime - eventData.pressTime;

                    // 押下中イベント発行（移行後）
                    ExecuteEvents.Execute<IKeyHoldHandler>(
                        base.eventSystem.currentSelectedGameObject, eventData, 
                        ExecuteFunctions.KeyHoldHandler);
                }
                // 押下中に移行する前
                else if (eventData.useHoldThreshold)
                {
                    float delta = unscaledTime - eventData.pressTime;
                    if (delta > _holdDelay)
                    {
                        // 押下中イベント発行（移行する直前）
                        ExecuteEvents.Execute<IKeyBeginHoldHandler>(
                            base.eventSystem.currentSelectedGameObject, eventData, 
                            ExecuteFunctions.KeyBeginHoldHandler);

                        eventData.holding = true;
                        eventData.useHoldThreshold = false;
                        eventData.holdTime = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 指定されたIDに対応するボタンの状態を調べ、その状態を取得する
        /// </summary>
        /// <param name="keyCode">Inputクラスで使用するキーコード</param>
        /// <returns>ボタンの状態</returns>
        PointerEventData.FramePressState CheckKeyState(KeyCode keyCode)
        {
            bool downState = UnityEngine.Input.GetKeyDown(keyCode);
            bool upState = UnityEngine.Input.GetKeyUp(keyCode);
            PointerEventData.FramePressState result;

            if (downState && upState)
                result = PointerEventData.FramePressState.PressedAndReleased;
            else if (downState)
                result = PointerEventData.FramePressState.Pressed;
            else if (upState)
                result = PointerEventData.FramePressState.Released;
            else
                result = PointerEventData.FramePressState.NotChanged;

            return result;
        }
    }
}