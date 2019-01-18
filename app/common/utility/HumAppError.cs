using System;
using System.Runtime.Serialization;

namespace Hum.Common.Utility
{
    public enum HumAppErrorType
    {
        Validation,
        RestrictedAccess,
        Unauthorized,       
        Unexpected
    }

    public class HumAppError : Exception
    {
        public HumAppErrorType Type {get;set;}

        public HumAppError(HumAppErrorType type) : base("")
        {
            this.Type = type;
        }
        public HumAppError(HumAppErrorType type, string message) : base(message)
        {
            this.Type = type;
        }
    }
}