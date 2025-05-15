using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class Helper {
    public static string GetCommonPrefix(string str1, string str2) {
        int minLength = Mathf.Min(str1.Length, str2.Length);
        int i = 0;

        while (i < minLength && str1[i] == str2[i]) {
            i++;
        }

        return str1.Substring(0, i);
    }

    public static bool IsMatching(string str1, string str2) {
        string temp1 = Regex.Replace(str1.Trim(), @"\s+", " ");
        string temp2 = Regex.Replace(str2.Trim(), @"\s+", " ");

        return temp1 == temp2;
    }

    public static string GetCommonPrefixBeforeFirstSpace(string str1, string str2) {
        int minLength = Mathf.Min(str1.Length, str2.Length);
        int i = 0;

        while (i < minLength && str1[i] == str2[i]) {
            if (str1[i] == ' ' || str2[i] == ' ')
                break;
            i++;
        }

        return str1.Substring(0, i);
    }

    public static string GetFirstWord(string input) {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        string[] words = input.Trim().Split(' ');

        return words.Length > 0 ? words[0] : "";
    }

    public static bool HasOnlyOneWord(string input) {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Split by whitespace and remove empty entries
        string[] words = input.Trim().Split(new[] { ' ', '\t', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        return words.Length == 1;
    }
}
