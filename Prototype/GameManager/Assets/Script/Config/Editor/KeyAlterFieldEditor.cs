using System;
using UnityEngine;
using UnityEditor;
using Assets.Script.Manager;
using Assets.Script.Manager.Input;
using Assets.Script.Manager.Input.Editor;

namespace Assets.Script.Config.Editor
{
	[CustomEditor(typeof(KeyAlterField))]
	public class KeyAlterFieldEditor : UnityEditor.Editor
	{
        SerializedProperty _spHighColor;
        SerializedProperty _spActColor;
        SerializedProperty _spOffset;
        SerializedProperty _spKeyId;

        public override void OnInspectorGUI()
        {
			// オブジェクトを最新に更新
            serializedObject.Update();

            _spHighColor = serializedObject.FindProperty("_highlightColor");
            _spActColor = serializedObject.FindProperty("_activeColor");
            _spOffset = serializedObject.FindProperty("_offset");
            _spKeyId = serializedObject.FindProperty("_keyId");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_spHighColor);
            EditorGUILayout.PropertyField(_spActColor);
            ShowMapId();
            ShowKeyId();

			// 変更されていれば適用
            if (EditorGUI.EndChangeCheck())
                Save();
        }

        void ShowMapId()
        {
            if (_spOffset.intValue != 0 && 
                !Enum.IsDefined(typeof(EKeyIdOffset), _spOffset.intValue))
                _spOffset.intValue = 0;

            EKeyIdOffset offset = (EKeyIdOffset)_spOffset.intValue;
            offset = (EKeyIdOffset)EditorGUILayout.EnumPopup(_spOffset.displayName, offset);

            _spOffset.intValue = (int)offset;
        }

        void ShowKeyId()
        {
            int offset = _spOffset.intValue;
            int keyId = 0;

            if (DataId.EqualsUpper(offset, KeyIdOffset.UI))
            {
                EUIKeyId keyIdEnum = (EUIKeyId)_spKeyId.intValue;
                keyIdEnum = (EUIKeyId)EditorGUILayout.EnumPopup(
                    _spKeyId.displayName, keyIdEnum);
                keyId = (int)keyIdEnum;
            }
            else if (DataId.EqualsUpper(offset, KeyIdOffset.P1))
            {
                EPKeyId keyIdEnum = (EPKeyId)_spKeyId.intValue;
                keyIdEnum = (EPKeyId)EditorGUILayout.EnumPopup(
                    _spKeyId.displayName, keyIdEnum);
                keyId = (int)keyIdEnum;
            }

            _spKeyId.intValue = keyId;
        }

        void Save()
        {
			serializedObject.ApplyModifiedProperties();
        }
	}
}
