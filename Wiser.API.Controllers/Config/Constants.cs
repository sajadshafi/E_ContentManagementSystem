using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.BL.Config
{
    class Constants
    {
        public static readonly Guid DEFAULT_GUID = new Guid("{00000000-0000-0000-0000-000000000000}");
        public const string E_FILE_PATH = "EFiles/";
    }

    public enum Units
    {
        UNIT1=1,
        UNIT2=2,
        UNIT3=3,
        UNIT4=4,
        UNIT5=5,
        GENERAL=0
    }
}
