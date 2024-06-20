public static class ArrayExtension
{
    public static T[] Add<T>(this T[] array, T item)
    {
        if (array == null)
        {
            return new T[] { item };
        }

        T[] result = new T[array.Length + 1];
        array.CopyTo(result, 0);
        result[array.Length] = item;

        return result;
    }

    public static T[] Remove<T>(this T[] array, T item)
    {
        if (array == null)
        {
            return null;
        }

        if (!array.Contains(item))
        {
            return array;
        }

        T[] result = new T[array.Length - 1];
        int index = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (!array[i].Equals(item))
            {
                result[index] = array[i];
                index++;
            }
        }

        return result;
    }

    public static bool Contains<T>(this T[] array, T item)
    {
        if (array == null)
        {
            return false;
        }

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    public static T RandomValue<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            return default(T);
        }

        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}
