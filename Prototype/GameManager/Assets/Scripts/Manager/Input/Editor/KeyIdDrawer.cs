using System;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Manager.Input.Editor
{
	[CustomPropertyDrawer(typeof(KeyIdAttribute))]
	public class KeyIdDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			KeyIdAttribute keyIdAttr = (KeyIdAttribute)attribute;

			if (property.propertyType != SerializedPropertyType.Integer)
			{
				Debug.LogError("キーIDにint型以外は使用できません");
				return;
			}

			if (keyIdAttr.KeyIdOffset == KeyIdOffset.P1)
			{
				if (property.intValue != 0 && 
					!Enum.IsDefined(typeof(EPKeyId), property.intValue))
					Debug.LogWarning("プロパティはPKeyId型のメンバーではありません");

				EPKeyId curId = (EPKeyId)property.intValue;
				EPKeyId newId = (EPKeyId)EditorGUI.EnumPopup(position, label, curId);

				property.intValue = (int)newId;
			}
			else if (keyIdAttr.KeyIdOffset == KeyIdOffset.UI)
			{
				if (property.intValue != 0 && 
					!Enum.IsDefined(typeof(EUIKeyId), property.intValue))
					Debug.LogWarning("プロパティはUIKeyId型のメンバーではありません");

				EUIKeyId curId = (EUIKeyId)property.intValue;
				EUIKeyId newId = (EUIKeyId)EditorGUI.EnumPopup(position, label, curId);

				property.intValue = (int)newId;
			}
		}
	}
}
