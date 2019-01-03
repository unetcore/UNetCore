
using System;
using System.Runtime.InteropServices;

public static class Guard
{
    public static void Argument(bool predicate, string paramName, string message = null)
    {
        if (!predicate)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = GetString("ArgumentInvalid", new object[0]);
            }
            throw new ArgumentException(message, paramName);
        }
    }

    public static void ArgumentNull(object obj, string paramName, string message = null)
    {
        if (obj == null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = GetString("ArgumentNull", new object[] { paramName });
            }
            throw new ArgumentNullException(message, paramName);
        }
    }

    public static void Assert(bool condition, Exception exception)
    {
        if (!condition && (exception != null))
        {
            throw exception;
        }
    }

    public static void NullReference(object obj, string message = null)
    {
        if (obj == null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = GetString("NullReference", new object[0]);
            }
            throw new ArgumentException(message);
        }
    }

    public static void OutOfRange(int range, int index, string message = null)
    {
        if ((index < 0) || ((range > -2147483648) && (index > (range - 1))))
        {
            if (string.IsNullOrEmpty(message))
            {
                message = GetString("ArgumentOutOfRange", new object[0]);
            }
            throw new ArgumentOutOfRangeException(message);
        }
    }


    public static string GetString(string kind, params object[] args)
    {
        string str = kind;
        string tmp = "";
        if (args != null)
        {
            foreach (var arg in args)
            {
                if (arg != null)
                {
                    tmp += args + ",";
                }
            }
        }
        if (!tmp.IsNullOrEmpty())
        {

        }

        return str.TrimEnd(',');
    }
}

