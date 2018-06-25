using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Manager.Input
{
    public static class InputUtility
    {
        static Dictionary<KeyCode, string> _keyNames;

        public static readonly KeyCode[] charKeys = new KeyCode[]
        {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
            KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
            KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
            KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
            KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
            KeyCode.Z,
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
            KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Comma, KeyCode.Period,
            KeyCode.Slash, KeyCode.Backslash,
            KeyCode.Semicolon, KeyCode.Colon,
            KeyCode.LeftShift, KeyCode.RightShift,
            KeyCode.LeftControl, KeyCode.RightControl,
            KeyCode.LeftBracket, KeyCode.RightBracket,
            KeyCode.At, KeyCode.Minus, KeyCode.Caret,
        };

		public static readonly KeyCode[] joy1Keys = new KeyCode[]
		{
			KeyCode.Joystick1Button0,
			KeyCode.Joystick1Button1,
			KeyCode.Joystick1Button2,
			KeyCode.Joystick1Button3,
			KeyCode.Joystick1Button4,
			KeyCode.Joystick1Button5,
			KeyCode.Joystick1Button6,
			KeyCode.Joystick1Button7,
			KeyCode.Joystick1Button8,
			KeyCode.Joystick1Button9,
			KeyCode.Joystick1Button10,
			KeyCode.Joystick1Button11,
			KeyCode.Joystick1Button12,
			KeyCode.Joystick1Button13,
			KeyCode.Joystick1Button14,
			KeyCode.Joystick1Button15,
			KeyCode.Joystick1Button16,
			KeyCode.Joystick1Button17,
			KeyCode.Joystick1Button18,
			KeyCode.Joystick1Button19
		};

        static InputUtility()
        {
            _keyNames = new Dictionary<KeyCode, string>()
            {
                {KeyCode.Alpha0, "0"},      {KeyCode.Alpha1, "1"},
                {KeyCode.Alpha2, "2"},      {KeyCode.Alpha3, "3"},
                {KeyCode.Alpha4, "4"},      {KeyCode.Alpha5, "5"},
                {KeyCode.Alpha6, "6"},      {KeyCode.Alpha7, "7"},
                {KeyCode.Alpha8, "8"},      {KeyCode.Alpha9, "9"},
                {KeyCode.Comma,  ","},      {KeyCode.Period, "."},
                {KeyCode.Slash,  "/"},      {KeyCode.Backslash, "\\"},
                {KeyCode.Semicolon, ";"},   {KeyCode.Colon, ":"},
                {KeyCode.LeftShift, "LShift"},  {KeyCode.RightShift, "RShift"},
                {KeyCode.LeftControl, "LCtrl"}, {KeyCode.RightControl, "RCtrl"},
                {KeyCode.LeftBracket, "["},     {KeyCode.RightBracket, "]"},
                {KeyCode.At, "@"},              {KeyCode.Minus, "-"}, 
                {KeyCode.Caret, "^"}
            };
        }

        public static string GetKeyName(KeyCode code)
        {
            if (code >= KeyCode.A && code <= KeyCode.Z)
                return code.ToString();
            
            if (code == KeyCode.None)
                return "-";
            
            if (!_keyNames.ContainsKey(code))
                return "No name";
            
            return _keyNames[code];
        }

        public static bool CheckKeyPressed(out KeyCode keyPressed)
        {
            for (int i = 0; i < charKeys.Length; i++)
                if (UnityEngine.Input.GetKeyDown(charKeys[i]))
                {
                    keyPressed = charKeys[i];
                    return true;
                }

            // for (int i = 0; i < joy1Keys.Length; i++)
            //     if (UnityEngine.Input.GetKeyDown(joy1Keys[i]))
            //     {
            //         keyPressed = joy1Keys[i];
            //         return true;
            //     }
            
            keyPressed = 0;
            return false;
        }

        public class KeyCodeEqualityComparer : IEqualityComparer<KeyCode>
        {
            public bool Equals(KeyCode x, KeyCode y)
            {
                return (x == y);
            }

            public int GetHashCode(KeyCode obj)
            {
                return (int)obj;
            }
        }
    }
}