using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Assets.Script.Manager.Input
{
    /// <summary>
    /// マウスの入力イベントを処理するクラス
    /// </summary>
    public abstract class MouseInputModule : BaseInputModule
    {
        MouseState _mouseState;
        Vector2     _mousePosition;
        Vector2     _lastMousePosition;

        [SerializeField]
        bool _forceModuleActive;

        public class ButtonId
        {
            public const int Left = 0;
            public const int Right = 1;
            public const int Middle = 2;
        }

        public override bool IsModuleSupported()
        {
            return _forceModuleActive || base.input.mousePresent;
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
                flag |= ((this._mousePosition - this._lastMousePosition).sqrMagnitude > 0f);
                flag |= base.input.GetMouseButtonDown(0);
                result = flag;
            }
            return result;
        }

        public override void ActivateModule()
        {
            if (base.eventSystem.isFocused || !this.ShouldIgnoreEventsOnNoFocus())
            {
                base.ActivateModule();
                
                _mousePosition = base.input.mousePosition;
                _lastMousePosition = base.input.mousePosition;
                _mouseState = new MouseState(base.eventSystem);

                GameObject gameObject = base.eventSystem.currentSelectedGameObject;
                if (gameObject == null)
                {
                    gameObject = base.eventSystem.firstSelectedGameObject;
                }
                base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
            }
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            ClearSelection();
        }

        void ClearSelection()
        {
            for (int i = 0; i < _mouseState.Count; i++)
                base.HandlePointerExitAndEnter(_mouseState.GetEventData(i), null);
                
            base.eventSystem.SetSelectedGameObject(null, this.GetBaseEventData());
        }

        public override void UpdateModule()
        {
            if (base.eventSystem.isFocused || !this.ShouldIgnoreEventsOnNoFocus())
            {
                _lastMousePosition = _mousePosition;
                _mousePosition = base.input.mousePosition;
            }
        }

        protected bool ShouldIgnoreEventsOnNoFocus()
        {
            bool result;
            switch (SystemInfo.operatingSystemFamily)
            {
            case OperatingSystemFamily.MacOSX:
            case OperatingSystemFamily.Windows:
            case OperatingSystemFamily.Linux:
                result = !UnityEditor.EditorApplication.isRemoteConnected;
                break;
            default:
                result = false;
                break;
            }
            return result;
        }

        /// <summary>
        /// マウスのイベント処理
        /// </summary>
        protected void ProcessMouseEvent()
        {
            PointerInputModule.MouseButtonEventData buttonEventData;
            PointerInputModule.MouseButtonEventData buttonEventData1;

            UpdateMouseState();

            _mouseState.GetButtonEventData(ButtonId.Left, out buttonEventData);
            ProcessPress(buttonEventData);
            ProcessOver(buttonEventData.buttonData);
            ProcessDrag(buttonEventData.buttonData);
            _mouseState.GetButtonEventData(ButtonId.Right, out buttonEventData1);
            ProcessPress(buttonEventData1);
            ProcessDrag(buttonEventData1.buttonData);
            _mouseState.GetButtonEventData(ButtonId.Middle, out buttonEventData1);
            ProcessPress(buttonEventData1);
            ProcessDrag(buttonEventData1.buttonData);

            if (!Mathf.Approximately(buttonEventData.buttonData.scrollDelta.sqrMagnitude, 0f))
            {
                GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(
                    buttonEventData.buttonData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy<IScrollHandler>(
                    eventHandler, buttonEventData.buttonData, ExecuteEvents.scrollHandler);
            }
        }

        /// <summary>
        /// マウスの入力状態を更新する
        /// </summary>
        void UpdateMouseState()
        {
            // 左ボタンのイベント情報
            PointerEventData eventData1;
            bool created = _mouseState.GetEventData(ButtonId.Left, out eventData1);
            eventData1.Reset();

            Vector2 mousePosition = this.input.mousePosition;
            if (created)
                eventData1.position = this.input.mousePosition;

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                eventData1.delta = Vector2.zero;
                eventData1.position = new Vector2(-1f, -1f);
            }
            else
            {
                eventData1.delta = mousePosition - eventData1.position;
                eventData1.position = mousePosition;
            }

            eventData1.button = PointerEventData.InputButton.Left;
            eventData1.scrollDelta = base.input.mouseScrollDelta;
            base.eventSystem.RaycastAll(eventData1, base.m_RaycastResultCache);
            RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(base.m_RaycastResultCache);
            eventData1.pointerCurrentRaycast = pointerCurrentRaycast;
            base.m_RaycastResultCache.Clear();

            // 右ボタンのイベント情報
            PointerEventData eventData2;
            _mouseState.GetEventData(ButtonId.Right, out eventData2);
            CopyEventData(eventData1, eventData2);
            eventData2.button = PointerEventData.InputButton.Right;

            // 中ボタンのイベント情報
            PointerEventData eventData3;
            _mouseState.GetEventData(ButtonId.Middle, out eventData3);
            CopyEventData(eventData1, eventData3);
            eventData3.button = PointerEventData.InputButton.Middle;

            // ボタンの状態をチェックし、イベント情報に反映
            _mouseState.SetButtonState(ButtonId.Left, CheckMouseButtonState(0));
            _mouseState.SetButtonState(ButtonId.Right, CheckMouseButtonState(1));
            _mouseState.SetButtonState(ButtonId.Middle, CheckMouseButtonState(2));
        }

        /// <summary>
        /// イベント情報をコピーする
        /// </summary>
        /// <param name="from">コピー元</param>
        /// <param name="to">コピー先</param>
        void CopyEventData(PointerEventData from, PointerEventData to)
        {
            to.position = from.position;
            to.delta = from.delta;
            to.scrollDelta = from.scrollDelta;
            to.pointerCurrentRaycast = from.pointerCurrentRaycast;
            to.pointerEnter = from.pointerEnter;
        }

        /// <summary>
        /// 指定されたIDに対応するマウスボタンの状態を調べ、その状態を取得する
        /// </summary>
        /// <param name="inputId">Inputクラスで使用するマウスのボタンID</param>
        /// <returns>ボタンの状態</returns>
        PointerEventData.FramePressState CheckMouseButtonState(int inputId)
        {
            bool downState = base.input.GetMouseButtonDown(inputId);
            bool upState = base.input.GetMouseButtonUp(inputId);
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

        /// <summary>
        /// マウスの押下イベントを処理
        /// </summary>
        /// <param name="buttonEventData">マウスボタンのイベント情報</param>
        void ProcessPress(PointerInputModule.MouseButtonEventData buttonEventData)
        {
            PointerEventData eventData = buttonEventData.buttonData;
            GameObject rayTarget = eventData.pointerCurrentRaycast.gameObject;

            // ボタンを押下した時
            if (buttonEventData.PressedThisFrame())
            {
                GameObject eventTarget;

                eventData.eligibleForClick = true;
                eventData.delta = Vector2.zero;
                eventData.dragging = false;
                eventData.useDragThreshold = true;
                eventData.pressPosition = eventData.position;
                eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;

                DeselectIfSelectionChanged(rayTarget, eventData);

                // 押下イベントを発行。対象となるオブジェクトを取得
                eventTarget = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(
                                rayTarget, eventData, ExecuteEvents.pointerDownHandler);
                if (eventTarget == null)
                    eventTarget = ExecuteEvents.GetEventHandler<IPointerClickHandler>(rayTarget);

                // 選択したオブジェクトが直前のものと同じ場合、クリック回数をカウント
                float unscaledTime = Time.unscaledTime;
                if (eventTarget == eventData.lastPress)
                {
                    float deltaTime = unscaledTime - eventData.clickTime;

                    if (deltaTime < 0.3f)
                        eventData.clickCount++;
                    else
                        eventData.clickCount = 1;
                }
                else
                {
                    eventData.clickCount = 1;
                }

                eventData.pointerPress = eventTarget;
                eventData.rawPointerPress = rayTarget;
                eventData.clickTime = unscaledTime;

                // ドラッグ開始の直前イベント
                eventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(rayTarget);
                if (eventData.pointerDrag != null)
                {
                    ExecuteEvents.Execute<IInitializePotentialDragHandler>(
                        eventData.pointerDrag, eventData, ExecuteEvents.initializePotentialDrag);
                }
            }

            // ボタンを離上した時
            if (buttonEventData.ReleasedThisFrame())
            {
                // 離上イベントを発行
                ExecuteEvents.Execute<IPointerUpHandler>(
                    eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

                // クリックイベント
                GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(rayTarget);
                if (eventData.pointerPress == eventHandler && eventData.eligibleForClick)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(
                        eventData.pointerPress, eventData, ExecuteEvents.pointerClickHandler);
                }
                // ドロップイベント
                else if (eventData.pointerDrag != null && eventData.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy<IDropHandler>(
                        rayTarget, eventData, ExecuteEvents.dropHandler);
                }

                eventData.eligibleForClick = false;
                eventData.pointerPress = null;
                eventData.rawPointerPress = null;

                // ドラッグ終了
                if (eventData.pointerDrag != null && eventData.dragging)
                {
                    ExecuteEvents.Execute<IEndDragHandler>(
                        eventData.pointerDrag, eventData, ExecuteEvents.endDragHandler);
                }

                eventData.dragging = false;
                eventData.pointerDrag = null;

                // マウスオーバー更新
                if (rayTarget != eventData.pointerEnter)
                {
                    base.HandlePointerExitAndEnter(eventData, null);
                    base.HandlePointerExitAndEnter(eventData, rayTarget);
                }
            }
        }

        /// <summary>
        /// 選択するオブジェクトが選択中のものと異なれば非選択状態にする
        /// </summary>
        /// <param name="selection">選択するオブジェクト</param>
        /// <param name="eventData">イベント情報</param>
        protected void DeselectIfSelectionChanged(GameObject selection, BaseEventData eventData)
        {
            GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(selection);
            if (eventHandler != base.eventSystem.currentSelectedGameObject)
            {
                base.eventSystem.SetSelectedGameObject(null, eventData);
            }
        }

        /// <summary>
        /// マウスオーバー時のイベントを処理
        /// </summary>
        /// <param name="eventData">イベント情報</param>
        void ProcessOver(PointerEventData eventData)
        {
            GameObject newEnterTarget = (Cursor.lockState != CursorLockMode.Locked) 
                    ? eventData.pointerCurrentRaycast.gameObject : null;

            base.HandlePointerExitAndEnter(eventData, newEnterTarget);
        }

        /// <summary>
        /// マウスのドラッグイベントを処理
        /// </summary>
        /// <param name="eventData">イベント情報</param>
        void ProcessDrag(PointerEventData eventData)
        {
    	    if (!eventData.IsPointerMoving() 
                || Cursor.lockState == CursorLockMode.Locked 
                || eventData.pointerDrag == null)
                return;
            
            // ドラッグ開始
            if (!eventData.dragging 
                && ShouldStartDrag(eventData.pressPosition, eventData.position, 
                    (float)base.eventSystem.pixelDragThreshold, eventData.useDragThreshold))
            {
                ExecuteEvents.Execute<IBeginDragHandler>(
                    eventData.pointerDrag, eventData, ExecuteEvents.beginDragHandler);
                eventData.dragging = true;
            }

            // ドラッグ中
            if (eventData.dragging)
            {
                if (eventData.pointerPress != eventData.pointerDrag)
                {
                    ExecuteEvents.Execute<IPointerUpHandler>(
                        eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

                    eventData.eligibleForClick = false;
                    eventData.pointerPress = null;
                    eventData.rawPointerPress = null;
                }

                ExecuteEvents.Execute<IDragHandler>(
                    eventData.pointerDrag, eventData, ExecuteEvents.dragHandler);
            }
        }

        bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
        }
    }
}