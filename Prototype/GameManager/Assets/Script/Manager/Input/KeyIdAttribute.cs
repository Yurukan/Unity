using UnityEngine;

namespace Assets.Script.Manager.Input
{
    public class KeyIdAttribute : PropertyAttribute
    {
		int _offset;

        public KeyIdAttribute(int offset)
        {
            _offset = offset;
        }

        public int KeyIdOffset
        {
            get { return _offset; }
        }
    }
}