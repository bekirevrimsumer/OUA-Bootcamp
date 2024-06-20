using System.Collections.Generic;

public static class DictionaryExtension
{
    public static T KeyByValue<T, W>(this Dictionary<T, W> dictionary, W value)
    {
        foreach (KeyValuePair<T, W> pair in dictionary)
        {
            if (pair.Value.Equals(value))
            {
                return pair.Key;
            }
        }

        return default(T);
    }

    public static bool ContainsValue<T, W>(this Dictionary<T, W> dictionary, W value)
    {
        foreach (KeyValuePair<T, W> pair in dictionary)
        {
            if (pair.Value.Equals(value))
            {
                return true;
            }
        }

        return false;
    }

    public static bool ContainsKey<T, W>(this Dictionary<T, W> dictionary, T key)
    {
        foreach (KeyValuePair<T, W> pair in dictionary)
        {
            if (pair.Key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }
}
