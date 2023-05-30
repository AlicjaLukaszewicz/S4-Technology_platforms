using System;
using System.Collections.Generic;

namespace PT_Lab_1
{
    [Serializable]
    public class StringComparer : IComparer<String>
    {
        public int Compare(String a, String b)
        {
            if (a.Length > b.Length) return 1;
            else if (a.Length < b.Length) return -1;
            else return a.CompareTo(b);
        }
    }
}
