using System.Globalization;
using System.Text;
/// <summary>
/// 拓展方法设置
/// </summary>
public static class ExtensionMethodSetting
{
    /// <summary>
    /// Initializes a static instance of the ExtensionMethodsSettings class
    /// </summary>
    static ExtensionMethodSetting()
    {
        DefaultEncoding = Encoding.UTF8;
        DefaultCulture = CultureInfo.CurrentCulture;
    }

    /// <summary>
    /// 获取或者设置默认编码,默认UTF8
    /// </summary>
    /// <remarks>
    /// The default value for this property is <see cref="Encoding.UTF8"/>
    /// </remarks>
    public static Encoding DefaultEncoding { get; set; }

    /// <summary>
    /// 获取或者设置默认语言信息，默认CultureInfo.CurrentUICulture
    /// </summary>
    /// <remarks>
    /// The default value for this property is <see cref="CultureInfo.CurrentUICulture"/>
    /// </remarks>
    public static CultureInfo DefaultCulture { get; set; }
}