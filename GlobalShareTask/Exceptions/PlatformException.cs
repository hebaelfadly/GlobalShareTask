using System;

namespace GlobalShareTask
{
    [Serializable]
    public class PlatformException : Exception
    {
        public PlatformException(string message) : base(message)
        {
        }
    }
}