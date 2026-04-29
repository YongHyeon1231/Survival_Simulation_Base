using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public static class Utils
{
    public static string Localization_text(String_Table table, string key)
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        return LocalizationSettings.StringDatabase.GetLocalizedString(table.ToString(), key, currentLanguage);
    }

    public static string Timer(float time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
        return timer;
    }

    public static T FindBase<T>(Transform parent, string key)
    {
        return parent.Find(key).GetComponent<T>();
    }

    public static void SetLayer(string layer, GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(layer);
    }
}
