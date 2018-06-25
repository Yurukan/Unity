namespace Assets.Script.Manager.Input
{
	/// <summary>
	/// キー情報のIDオフセットを定義する定数クラス
	/// </summary>
	public static class KeyIdOffset
	{
		public const int UI = 0x01010100;
		public const int P1 = 0x01020100;
		public const int Num = 2;
	}

	/// <summary>
	/// UI用のキーIDを定義する定数クラス
	/// </summary>
	public static class UIKeyId
	{
		public const int Submit = 0x01010101;
		public const int Cancel = 0x01010102;
		public const int Left = 0x01010103;
		public const int Right = 0x01010104;
		public const int Up = 0x01010105;
		public const int Down = 0x01010106;
		public const int Num = 6;
	}

	/// <summary>
	/// キャラクター用のキーIDを定義する定数クラス
	/// </summary>
	public static class PKeyId
	{
		public const int Attack = 0x01020101;
		public const int Jump = 0x01020102;
		public const int Num = 2;
	}
}
