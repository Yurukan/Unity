using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace Assets.Script.Manager.Input.Editor
{
    static class KeyIdEditor
    {
        const string KeyFilePath = "Assets/Script/Manager/Input/KeyId.cs";
        const string MapFilePath = "Assets/Script/Manager/Input/KeyMapId.cs";
        const string Namespace = "Assets.Script.Manager.Input";

        static void OutputMember1(StringBuilder builder, string[] names, Array values)
        {
            int index = 0;

            foreach (int value in values)
            {
                builder.AppendFormat(
                    "\t\tpublic const int {0} = 0x{1};", 
                    names[index++], value.ToString("X8")).AppendLine();
            }
            builder.AppendFormat(
                "\t\tpublic const int Num = {0};", index).AppendLine();
        }
        
        static void OutputMember2(StringBuilder builder, string[] names, Array values)
        {
            int index = 0;

            foreach (int value in values)
            {
                builder.AppendFormat(
                    "\t\tpublic const int {0} = {1};", 
                    names[index++], value.ToString()).AppendLine();
            }
            builder.AppendFormat(
                "\t\tpublic const int Num = {0};", index).AppendLine();
        }
        
        [MenuItem("MyAssets/Input/Create KeyId")]
        static void CreateKeyId()
        {
			try
			{
                StringBuilder builder = new StringBuilder();
                string[] names;
                Array values;

				builder.AppendLine("namespace "+ Namespace);
				builder.AppendLine("{");

				names = Enum.GetNames(typeof(EKeyIdOffset));
				values = Enum.GetValues(typeof(EKeyIdOffset));
				builder.AppendLine("\t/// <summary>");
				builder.AppendLine("\t/// キー情報のIDオフセットを定義する定数クラス");
				builder.AppendLine("\t/// </summary>");
				builder.AppendLine("\tpublic static class KeyIdOffset");
				builder.AppendLine("\t{");
                OutputMember1(builder, names, values);
				builder.AppendLine("\t}");

				builder.AppendLine();

				names = Enum.GetNames(typeof(EUIKeyId));
				values = Enum.GetValues(typeof(EUIKeyId));
				builder.AppendLine("\t/// <summary>");
				builder.AppendLine("\t/// UI用のキーIDを定義する定数クラス");
				builder.AppendLine("\t/// </summary>");
				builder.AppendLine("\tpublic static class UIKeyId");
				builder.AppendLine("\t{");
                OutputMember1(builder, names, values);
				builder.AppendLine("\t}");

				builder.AppendLine();

                names = Enum.GetNames(typeof(EPKeyId));
                values = Enum.GetValues(typeof(EPKeyId));
				builder.AppendLine("\t/// <summary>");
				builder.AppendLine("\t/// キャラクター用のキーIDを定義する定数クラス");
				builder.AppendLine("\t/// </summary>");
				builder.AppendLine("\tpublic static class PKeyId");
				builder.AppendLine("\t{");
                OutputMember1(builder, names, values);
				builder.AppendLine("\t}");

                // 名前空間の終了
				builder.AppendLine("}");

				// ファイルに出力
				File.WriteAllText(KeyFilePath, builder.ToString(), Encoding.UTF8);

				// Unityエディタに反映
				AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

				EditorUtility.DisplayDialog("KeyIdEditor",
					KeyFilePath + "\nを作成しました", "OK");
			}
			catch (Exception ex)
			{
				EditorUtility.DisplayDialog("KeyIdEditor",
					string.Format("{0}\nキーIDの作成に失敗しました", ex.Message), "OK");
			}
        }
 
        [MenuItem("MyAssets/Input/Create MapId")]
        static void CreateMapId()
        {
			try
			{
                StringBuilder builder = new StringBuilder();
                string[] names;
                Array values;

				builder.AppendLine("namespace "+ Namespace);
				builder.AppendLine("{");

				names = Enum.GetNames(typeof(EUIMapId));
				values = Enum.GetValues(typeof(EUIMapId));
				builder.AppendLine("\t/// <summary>");
				builder.AppendLine("\t/// UI用のマップIDを定義する定数クラス");
				builder.AppendLine("\t/// </summary>");
				builder.AppendLine("\tpublic static class UIMapId");
				builder.AppendLine("\t{");
                OutputMember2(builder, names, values);
				builder.AppendLine("\t}");

				builder.AppendLine();

                names = Enum.GetNames(typeof(EPMapId));
                values = Enum.GetValues(typeof(EPMapId));
				builder.AppendLine("\t/// <summary>");
				builder.AppendLine("\t/// キャラクター用のマップIDを定義する定数クラス");
				builder.AppendLine("\t/// </summary>");
				builder.AppendLine("\tpublic static class PMapId");
				builder.AppendLine("\t{");
                OutputMember2(builder, names, values);
				builder.AppendLine("\t}");

                // 名前空間の終了
				builder.AppendLine("}");

				// ファイルに出力
				File.WriteAllText(MapFilePath, builder.ToString(), Encoding.UTF8);

				// Unityエディタに反映
				AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

				EditorUtility.DisplayDialog("KeyIdEditor",
					MapFilePath + "\nを作成しました", "OK");
			}
			catch (Exception ex)
			{
				EditorUtility.DisplayDialog("KeyIdEditor",
					string.Format("{0}\nマップIDの作成に失敗しました", ex.Message), "OK");
			}
        }
    }
}