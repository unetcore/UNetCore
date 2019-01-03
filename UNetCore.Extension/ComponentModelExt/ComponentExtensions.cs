using System.ComponentModel;
/// <summary>
/// IComponent 拓展
/// </summary>
public static class ComponentExtensions
{
    /// <summary>
    /// 	Returns <c>true</c> if target component is in design mode.
    /// 	Othervise returns <c>false</c>.
    /// </summary>
    /// <param name = "target">Target component. Can not be null.</param>
    /// <remarks> 
    /// </remarks>
    public static bool IsInDesignMode(this IComponent target)
    {
        var site = target.Site;
        return ReferenceEquals(site, null) ? false : site.DesignMode;
    }

    /// <summary>
    /// 	Returns <c>true</c> if target component is NOT in design mode.
    /// 	Othervise returns <c>false</c>.
    /// </summary>
    /// <param name = "target">Target component.</param>
    /// <remarks> 
    /// </remarks>
    public static bool IsInRuntimeMode(this IComponent target)
    {
        return !IsInDesignMode(target);
    }
}