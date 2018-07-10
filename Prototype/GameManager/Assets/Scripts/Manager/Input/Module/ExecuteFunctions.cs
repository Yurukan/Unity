using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager.Input
{
    public static class ExecuteFunctions
    {
        static readonly ExecuteEvents.EventFunction<IKeyDownHandler> s_keyDownHandler;
        static readonly ExecuteEvents.EventFunction<IKeyHoldHandler> s_keyHoldHandler;
        static readonly ExecuteEvents.EventFunction<IKeyBeginHoldHandler> s_keyBeginHoldHandler;
        static readonly ExecuteEvents.EventFunction<IKeyEndHoldHandler> s_keyEndHoldHandler;
        static readonly ExecuteEvents.EventFunction<IKeyUpHandler> s_keyUpHandler;

        public static ExecuteEvents.EventFunction<IKeyDownHandler> KeyDownHandler
        {
            get { return s_keyDownHandler; }
        }

        public static ExecuteEvents.EventFunction<IKeyHoldHandler> KeyHoldHandler
        {
            get { return s_keyHoldHandler; }
        }

        public static ExecuteEvents.EventFunction<IKeyBeginHoldHandler> KeyBeginHoldHandler
        {
            get { return s_keyBeginHoldHandler; }
        }

        public static ExecuteEvents.EventFunction<IKeyEndHoldHandler> KeyEndHoldHandler
        {
            get { return s_keyEndHoldHandler; }
        }

        public static ExecuteEvents.EventFunction<IKeyUpHandler> KeyUpHandler
        {
            get { return s_keyUpHandler; }
        }

        static ExecuteFunctions()
        {
            s_keyDownHandler = Execute;
            s_keyHoldHandler = Execute;
            s_keyBeginHoldHandler = Execute;
            s_keyEndHoldHandler = Execute;
            s_keyUpHandler = Execute;
        }

		static void Execute(IKeyDownHandler handler, BaseEventData eventData)
		{
			handler.OnKeyDown(ExecuteEvents.ValidateEventData<KeyEventData>(eventData));
		}

		static void Execute(IKeyHoldHandler handler, BaseEventData eventData)
		{
			handler.OnKeyHold(ExecuteEvents.ValidateEventData<KeyEventData>(eventData));
		}

		static void Execute(IKeyBeginHoldHandler handler, BaseEventData eventData)
		{
			handler.OnKeyBeginHold(ExecuteEvents.ValidateEventData<KeyEventData>(eventData));
		}

		static void Execute(IKeyEndHoldHandler handler, BaseEventData eventData)
		{
			handler.OnKeyEndHold(ExecuteEvents.ValidateEventData<KeyEventData>(eventData));
		}

		static void Execute(IKeyUpHandler handler, BaseEventData eventData)
		{
			handler.OnKeyUp(ExecuteEvents.ValidateEventData<KeyEventData>(eventData));
		}
    }
}