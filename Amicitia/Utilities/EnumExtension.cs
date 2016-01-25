﻿namespace Amicitia.Utilities
{
    using System;

    internal static class EnumExtension
    {
        public static bool HasFlagFast(this Enum var, Enum val)
        {
            int varNum = Convert.ToInt32(var);
            int valNum = Convert.ToInt32(val);
            return (varNum & valNum) == valNum;
        }
    }
}
