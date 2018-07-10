using System;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Manager.Input.Editor
{
	[CustomPropertyDrawer(typeof(KeyIdOffsetAttribute))]
	public class KeyIdOffsetDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.Integer)
			{
				Debug.LogError("IDオフセットにint型以外は使用できません");
				return;
			}

            if (property.intValue != 0 && 
                !Enum.IsDefined(typeof(EKeyIdOffset), property.intValue))
                Debug.LogWarning("プロパティはKeyIdOffset型のメンバーではありません");

            EKeyIdOffset curId = (EKeyIdOffset)property.intValue;
            EKeyIdOffset newId = (EKeyIdOffset)EditorGUI.EnumPopup(position, label, curId);

            property.intValue = (int)newId;
		}
	}
}
