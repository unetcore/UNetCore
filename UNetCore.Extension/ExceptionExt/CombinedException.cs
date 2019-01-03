using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 组合异常的拓展
/// </summary>
public class CombinedException : Exception
{
    /// <summary>
    /// 	Initializes a new instance of the <see cref = "CombinedException" /> class.
    /// </summary>
    /// <param name = "message">The message.</param>
    /// <param name = "innerExceptions">The inner exceptions.</param>
    public CombinedException(string message, Exception[] innerExceptions) : base(message)
    {
        InnerExceptions = innerExceptions;
    }

    /// <summary>
    /// 获取内部异常
    /// </summary>
    /// <value>The inner exceptions.</value>
    public Exception[] InnerExceptions { get; protected set; }

    /// <summary>
    /// 组合异常
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerExceptions"></param>
    /// <returns></returns>
    public static Exception Combine(string message, params Exception[] innerExceptions)
    {
        if (innerExceptions.Length == 1)
            return innerExceptions[0];

        return new CombinedException(message, innerExceptions);
    }
    /// <summary>
    /// 组合异常
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerExceptions">The inner exceptions.</param>
    /// <returns></returns>
    public static Exception Combine(string message, IEnumerable<Exception> innerExceptions)
    {
        return Combine(message, innerExceptions.ToArray());
    }
}