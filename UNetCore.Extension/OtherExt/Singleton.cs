using System;

/// <summary>
/// 单例模式抽象类
/// </summary>
/// <typeparam name="T">Class type of the singleton</typeparam>
public abstract class Singleton<T> where T : class
{
    #region Members

    /// <summary>
    /// 延迟构造获取实例对象
    /// </summary>
    private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());

    #endregion

    #region Properties

    /// <summary>
    /// 获取实例对象
    /// </summary>
    public static T Instance { get { return sInstance.Value; } }

    #endregion

    #region Methods

    /// <summary>
    /// 创建实例对象
    /// </summary>
    /// <returns></returns>
    private static T CreateInstanceOfT()
    {
        return Activator.CreateInstance(typeof(T), true) as T;
    }

    #endregion
}
