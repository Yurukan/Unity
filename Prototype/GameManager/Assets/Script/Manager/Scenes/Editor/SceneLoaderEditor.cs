using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace Assets.Script.Manager.Scenes.Editor
{
	[CustomEditor(typeof(SceneLoader))]
	public class SceneLoaderEditor : UnityEditor.Editor
    {
		SceneId 			_selectSceId;
		SerializedProperty	_spSceList;
		HashSet<string>   	_sceNames = new HashSet<string>();
		Dictionary<string, EditorBuildSettingsScene> _scenes;

		/// <summary>
		/// オブジェクトがロードされたときの処理
		/// </summary>
		void OnEnable()
		{
			_spSceList = serializedObject.FindProperty("_fstLoading");
			_scenes = new Dictionary<string, EditorBuildSettingsScene>(
						EditorBuildSettings.scenes.Length);

			// ビルド設定から有効なシーンを取得
			foreach (var obj in EditorBuildSettings.scenes)
				if (obj.enabled)
					_scenes.Add(Path.GetFileNameWithoutExtension(obj.path), obj);

			SerializedProperty spElem;
			EditorBuildSettingsScene sceBuild;

			for (int i = 0; i < _spSceList.arraySize; i++)
			{
				spElem = _spSceList.GetArrayElementAtIndex(i);

				// シーンがビルド設定に存在しないか無効の場合
				if (!_scenes.ContainsKey(spElem.stringValue))
				{
					Log.Warning("シーン({0})がビルド設定に存在しないか無効です", spElem.stringValue);
					continue;
				}

				sceBuild = _scenes[spElem.stringValue];
				_sceNames.Add(spElem.stringValue);

				// 開いているシーンを取得し、存在しなければロードする
				Scene sceLoaded = EditorSceneManager.GetSceneByName(spElem.stringValue);
				if (!sceLoaded.isLoaded)
					EditorSceneManager.OpenScene(sceBuild.path, OpenSceneMode.Additive);
			}
		}

		/// <summary>
		/// インスペクターにGUIを表示する
		/// </summary>
		public override void OnInspectorGUI()
        {
			// オブジェクトを最新に更新
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			// ロードするアセットバンドル名を追加するGUI
			using (new EditorGUILayout.VerticalScope())
			{
				// ヘッダーを表示
				ShowHeader();

				GUILayout.Space(5);

				// シーンリストを表示
				ShowSceneList();
			}

			// オブジェクトに変更を適用
			if (EditorGUI.EndChangeCheck())
				Save();
        }

		/// <summary>
		/// シーンIDを追加するGUIを表示する
		/// </summary>
        void ShowHeader()
        {
			using (new EditorGUILayout.HorizontalScope())
			{
				// GUI:シーンIDの列挙体
				_selectSceId = (SceneId)EditorGUILayout.EnumPopup(
						"Add scene", _selectSceId);
				string sceName = _selectSceId.ToString();

				// GUI:追加ボタン
				if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(20)))
				{
					_selectSceId = 0;

					// ビルド設定を再取得
					_scenes.Clear();
					foreach (var obj in EditorBuildSettings.scenes)
						if (obj.enabled)
							_scenes.Add(Path.GetFileNameWithoutExtension(obj.path), obj);

					// シーンがビルド設定に存在しないか無効の場合
					if (!_scenes.ContainsKey(sceName))
					{
						Log.Warning("シーン({0})がビルド設定に存在しないか無効です", sceName);
						return;
					}

					EditorBuildSettingsScene scene = _scenes[sceName];

					// シーンが読み込まれていない場合
					Scene loaded = EditorSceneManager.GetSceneByName(sceName);
					if (loaded.isLoaded)
						return;

					// 追加モードでシーンをロードする
					UnityEditor.SceneManagement.EditorSceneManager
						.OpenScene(scene.path, OpenSceneMode.Additive);
					_sceNames.Add(sceName);
				}
			}
        }

		/// <summary>
		/// 追加されているシーンの表示や削除を行うGUIを表示する
		/// </summary>
		void ShowSceneList()
		{
			string remSceName = null;

			foreach (var name in _sceNames)
			{
				EditorGUILayout.BeginHorizontal();

				// GUI:シーン名
				EditorGUILayout.LabelField(name, EditorStyles.textField);

				// GUI:削除ボタン
				if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20)))
				{
					remSceName = name;

					// ヒエラルキー上のシーンを取得
					UnityEngine.SceneManagement.Scene loaded =
						EditorSceneManager.GetSceneByName(remSceName);

					// 追加シーンを削除する
					if (loaded.isLoaded)
						EditorSceneManager.CloseScene(loaded, true);
				}

				EditorGUILayout.EndHorizontal();
			}

			if (remSceName != null)
				_sceNames.Remove(remSceName);
		}

		/// <summary>
		/// データを保存する
		/// </summary>
		void Save()
		{
			int i = 0;
			SerializedProperty spElem;

			// シーンリストを更新
			_spSceList.arraySize = _sceNames.Count;
			foreach (var v in _sceNames)
			{
				spElem = _spSceList.GetArrayElementAtIndex(i++);
				spElem.stringValue = v;
			}

			serializedObject.ApplyModifiedProperties();
		}
    }
}