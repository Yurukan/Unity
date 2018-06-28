using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace Assets.Script.Manager.Scenes.Editor
{
	static class SceneIdEditor
	{
		const string FilePath = "Assets/Script/Manager/Scenes/SceneId.cs";

		[MenuItem("MyAssets/Scene/Create SceneId")]
		static void CreateSceneId()
		{
			string fileName = Path.GetFileName(FilePath);

			try
			{
				OutputSceneId();

				// Unityエディタに反映
				AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

				EditorUtility.DisplayDialog("SceneIdEditor", 
					FilePath + "\nを作成しました", "OK");
			}
			catch (Exception ex)
			{
				EditorUtility.DisplayDialog(fileName,
					string.Format("{0}\n作成に失敗しました", ex.Message), "OK");
			}
		}

		static void OutputSceneId()
		{
			StringBuilder builder = new StringBuilder();
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

			builder.AppendLine("namespace Assets.Script.Manager.Scenes");
			builder.AppendLine("{");
			builder.AppendLine("\t/// <summary>");
			builder.AppendLine("\t/// シーンIDの列挙型");
			builder.AppendLine("\t/// </summary>");
			builder.AppendLine("\tpublic enum SceneId");
			builder.AppendLine("\t{");

			// ビルド設定からシーンを取得し、有効なものをシーンIDとする
			for (int i = 0; i < scenes.Length; i++)
			{
				if (!scenes[i].enabled)
					continue;

				builder.AppendFormat("\t\t{0} = {1},",
					Path.GetFileNameWithoutExtension(scenes[i].path), i).AppendLine();
			}

			builder.AppendLine("\t}");
			builder.AppendLine("}");

			// ファイルに出力
			File.WriteAllText(FilePath, builder.ToString(), Encoding.UTF8);
		}
	}
}
