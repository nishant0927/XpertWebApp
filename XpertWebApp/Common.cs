using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XpertWebApp
{
    public enum CompairStringResult
    {
        Equal = 0,
        Less = 1,
        Greater = 2
    }
    public class Common
    {
        public static CompairStringResult CompairString(string strFirst, string strSecond)
        {
            return CompairString(strFirst, strSecond, false);
        }

        public static CompairStringResult CompairString(string strFirst, string strSecond, bool isCaseSencitive)
        {
            int x = string.Compare(Convert.ToString(strFirst), Convert.ToString(strSecond), !isCaseSencitive);
            if (x == 0)
            {
                return CompairStringResult.Equal;
            }
            else if (x < 0)
            {
                return CompairStringResult.Less;
            }
            else
            {
                return CompairStringResult.Greater;
            }
        }
    }
}