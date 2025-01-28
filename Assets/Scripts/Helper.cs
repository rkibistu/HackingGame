using System.Collections.Generic;
using System;
using UnityEngine;

public class Helper {
    public static string GetCommonPrefix(string str1, string str2) {
        int minLength = Mathf.Min(str1.Length, str2.Length);
        int i = 0;

        while (i < minLength && str1[i] == str2[i]) {
            i++;
        }

        return str1.Substring(0, i);
    }
}
