using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BO
{
    /// <summary>
    /// Class that converts the fields in the one class to another class
    /// </summary>
    public static class DeepCopy
    {
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                {
                    continue;
                }
                object value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                {
                    propTo.SetValue(to, value);
                }
            }
        }

        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to)
            where T : new()
        {
            foreach (S s in from)
            {
                T t = new T();
                s.CopyPropertiesTo(t);
                to.Add(t);
            }
        }
    }
}