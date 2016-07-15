/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace uMVVMCS
{
    public static class CompareUtils
    {
        /// <summary>
        /// 比较两个不为空的 IList<object> 是否相等
        /// </summary>
        public static bool isSameValueIList(IList<object> a, IList<object> b)
        {
            if (a == null || b == null) { return false; }
            if (a.Count != b.Count) { return false; }

            int length = a.Count;
            for (int i = 0; i < length; i++)
            {
                if (!CompareUtils.isSameObject(a[i], b[i])) { return false; }
            }

            return true;
        }

        /// <summary>
        /// 比较两个不为空的 object 数组是否相等
        /// </summary>
        public static bool isSameValueArray(object[] a, object[] b)
        {
            if (a == null || b == null) { return false; }
            if (a.Length != b.Length) { return false; }

            int length = a.Length;
            for (int i = 0; i < length; i++)
            {
                if (!CompareUtils.isSameObject(a[i], b[i])) { return false; }
            }

            return true;
        }

        /// <summary>
        /// 比较两个 object 是否相等
        /// </summary>
        public static bool isSameObject(object a, object b)
        {
            if (a == null && b == null) { return true; }

            if ((a == null && b != null) || (a != null && b == null)) { return false; }

            return a.Equals(b);
        }

        /// <summary>
        /// 返回数组是否含有指定元素，也可于数组的 Find 方法中进行比较
        /// </summary>
        public static bool ContainsElement<T>(T[] array, T item)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if(array[i].Equals(item)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// 返回 list 中所有与指定类型相等的元素(T1：需要获取的类型；T2：list的类型)
        /// </summary>
        public static IList<T1> OfTheType<T1, T2>(this IList<T2> list)
        {
            Type t;
            IList<T1> matched = new List<T1>();

            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] is Type) { t = list[i] as Type; }
                else { t = list[i].GetType(); }

                if (t == typeof(T1))
                {
                    // 泛型的类型转换不能直接使用(T)或者 as 的方法，应该用 Convert.ChangeType
                    matched.Add((T1)Convert.ChangeType(list[i], typeof(T1)));
                }
            }

            return matched;
        }
    }
}