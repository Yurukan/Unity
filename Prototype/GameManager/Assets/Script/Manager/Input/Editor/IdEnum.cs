namespace Assets.Script.Manager.Input.Editor
{
    enum EKeyIdOffset
    {
        UI = 0x1010100,
        P1 = 0x1020100
    }

    enum EUIMapId
    {
        Menu = 1
    }

    enum EPMapId
    {
        Normal = 1
    }

    enum EUIKeyId
    {
        Submit = EKeyIdOffset.UI + 1,
        Cancel,
        Left,
        Right,
        Up,
        Down,
        // ↑ 予約語
    }

    enum EPKeyId
    {
        Attack = EKeyIdOffset.P1 + 1,
        Jump,
    }
}