using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;

public class ConfigurationManager {
    public static IConfiguration Configuration;

    /// <summary>
    /// 注入配置信息
    /// </summary>
    /// <param name="config"></param>
    public static void RegisterConfiguration (IConfiguration config) {
        Configuration = config;
    }

    public static IConfigurationSection GetSection (string key) { return Configuration.GetSection (key); }
    public static string GetValue (string key) {
        return Configuration.GetValue<string> (key);
    }

    public static T GetSection<T> (string key = null)
    where T : new () {
        if (Configuration == null) {
            throw new ArgumentNullException (nameof (Configuration));
        }

        if (key == null) {
            key = typeof (T).Name;
        }

        var section = new T ();
        Configuration.GetSection (key).Bind (section);
        return section;
    }
    public static T GetValue<T> (string key) {
        return Configuration.GetValue<T> (key);
    }

    private static Dictionary<string, object> _AppSettings;
    public static Dictionary<string, object> AppSettings {
        get {
            if (_AppSettings == null) {
                _AppSettings = GetSection<Dictionary<string, object>> ("AppSettings");
                if (_AppSettings == null) {
                    return new Dictionary<string, object> ();
                }
            }

            return _AppSettings;
        }
    }

}