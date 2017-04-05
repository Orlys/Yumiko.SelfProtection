/// Display Error In Console 
#define Display_Error

/// Generate Empty DLL
//#define Create_New

#pragma warning disable 0168
namespace Yumiko.SelfProtection.Strobarrieds.Core
{
#if !Create_New
    using System;

    public sealed partial class Strobarried
    {
        [Flags]
        public enum Evaluation
        {
            Error = 0,
            False = 1,
            True = 2,
            Restart = False | True
        }
    }
#endif
}

#pragma warning restore CS0168
