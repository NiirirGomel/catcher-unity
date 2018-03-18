using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameSaves {

    static string m_savePath = Application.persistentDataPath + "/save_data.db";
    public static string savePath { get { return m_savePath; } }

    public static string LoadValue(string name) {
        if (File.Exists(m_savePath)) {
            string[] data = File.ReadAllLines(m_savePath);
            for (int i = 0; i < data.Length; i++) {
                if (data[i].StartsWith(name)) {
                    return data[i].Substring(name.Length);
                }
            }
        }
        return "";
    }

    public static void SaveValue(string name, string value) {
        List<string> data = File.Exists(m_savePath) ? new List<string>(File.ReadAllLines(m_savePath)) : new List<string>();
        for (int i = 0; i < data.Count; i++) {
            if (data[i].StartsWith(name)) {
                data[i] = name + value;
                File.WriteAllLines(m_savePath, data.ToArray());
                return;
            }
        }
        data.Add(name + value);
        File.WriteAllLines(m_savePath, data.ToArray());
    }
}