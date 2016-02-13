using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Droid.Core.Helpers
{
    /// <summary>
    /// »спользуетс€ дл€ преобразовани€ типов Java к типам CLR
    /// </summary>
    public static class JavaCastToCLRHelper
    {
        public static T Cast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}