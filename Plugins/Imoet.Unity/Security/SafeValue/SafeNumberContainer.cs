using System;
using System.Collections.Generic;

namespace Imoet.Unity.Security.SafeValue.Internal
{
    internal static class SafeNumberManager {
        private static Dictionary<Type, List<int>> m_container;

        static SafeNumberManager() {
            m_container = new Dictionary<Type, List<int>>();
        }

        public static int GetSalt<T>() {
            var t = typeof(T);
            if (!m_container.ContainsKey(t))
                m_container.Add(t, new List<int>());
            var res = _getSaltNumber(m_container[t]);
            m_container[t].Add(res);
            return res;
        }
        public static bool RemoveSalt<T>(int number) {
            var t = typeof(T);
            if (m_container.ContainsKey(t)) {
                return m_container[t].Remove(number);
            }
            return false;
        }

        private static int _getSaltNumber(List<int> list) {
            var random = new Random();
            while (true) {
                var res = random.Next(int.MinValue, int.MaxValue);
                if(!list.Contains(res))
                {
                    list.Add(res);
                    return res;
                }
            }
        }
    }
}
