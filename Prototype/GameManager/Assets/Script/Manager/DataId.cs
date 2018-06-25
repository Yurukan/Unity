using System;

namespace Assets.Script.Manager
{
    public struct DataId : IEquatable<DataId>, IComparable<DataId>
    {
        public const int MaskAll    = 0x7fffffff;
        public const int MaskRole   = 0x7f000000;
        public const int MaskUtil   = 0x00ff0000;
        public const int MaskData   = 0x0000ff00;
        public const int MaskIndex  = 0x000000ff;
        public const int MaskUpper  = 0x7fff0000;

        const int DistRole  = 24;
        const int DistUtil  = 16;
        const int DistData  = 8;

        byte _role;
        byte _util;
        byte _data;
        byte _index;

        public DataId(int id)
        {
            _role = (byte)((id & MaskRole) >> DistRole);
            _util = (byte)((id & MaskUtil) >> DistUtil);
            _data = (byte)((id & MaskData) >> DistData);
            _index = (byte)(id & MaskIndex);
        }

        public DataId(int offset, byte index)
        {
            _role = (byte)((offset & MaskRole) >> DistRole);
            _util = (byte)((offset & MaskUtil) >> DistUtil);
            _data = (byte)((offset & MaskData) >> DistData);
            _index = index;
        }

        public int Id
        {
            get { return (_role << DistRole) | (_util << DistUtil) | (_data << DistData) | _index; }
        }

        public int Data
        {
            get { return _data; }
        }
        public static int GetData(int id)
        {
            return (int)(id & MaskData) >> DistData;
        }

        public int Index
        {
            get { return _index; }
        }
        public static int GetIndex(int id)
        {
            return (int)(id & MaskIndex);
        }

        public override string ToString()
        {
            return Id.ToString("X8");
        }
        public static string ToString(int id)
        {
            return id.ToString("X8");
        }

        public override int GetHashCode()
        {
            return _util ^ _index;
        }

        public int CompareTo(DataId other)
        {
            if (Id < other.Id)
                return -1;
            else if (Id > other.Id)
                return 1;

            return 0;
        }

        public bool Equals(DataId other)
        {
            if (_index == other._index && _data == other._data &&
                _util  == other._util  && _role == other._role)
                return true;
            
            return false;
        }

        public bool EqualsUpper(int id)
        {
            return ((_role << DistRole) | (_util << DistUtil)) == (id & MaskUpper);
        }
        public static bool EqualsUpper(int id1, int id2)
        {
            return (id1 & MaskUpper) == (id2 & MaskUpper);
        }

        public bool EqualsData(int id)
        {
            return (_data << DistData) == (id & MaskData);
        }
        public static bool EqualsData(int id1, int id2)
        {
            return (id1 & MaskData) == (id2 & MaskData);
        }
    }
}