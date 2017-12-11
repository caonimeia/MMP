using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

public static class MFTabFileReader {
    private const string REMARK_SIGN         = "#";                 // 以 # 开头为注释 
    private const char   SPLITTER_SIGN       = '\t';                // tab文件以'\t'作为分隔符
    private const string PRE_PATH            = "Assets/Config/";    // tab文件前置路径

    public static List<T> LoadTabFile<T>(string path, string primaryKey = null) {
        path = string.Format("{0}/{1}", PRE_PATH, path);
        if (!File.Exists(path)) {
            MFLog.LogError("File {0} Not Found" + path);
            return null;
        }

        List<string> fileDataList = LoadFileData(path);
        int index = 0;
        string[] fields = ReadFileField(ref index, fileDataList);
        if (!CheckFileField<T>(fields)) {
            MFLog.LogError("File {0} Header Struct Error" + path);
            return null;
        }

        return ReadFileContent<T>(index, fields, fileDataList);
    }

    private static List<string> LoadFileData(string path) {
        List<string> fileDataList = new List<string>();
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            using (StreamReader sr = new StreamReader(fs, Encoding.Default)) {
                while (sr.Peek() > -1) {
                    string line = sr.ReadLine();
                    fileDataList.Add(line);
                }
            }
        }

        return fileDataList;
    }

    private static string[] ReadFileField(ref int index, List<string> fileDataList) {
        int fileDataLen = fileDataList.Count;
        while (index < fileDataLen) {
            string line = fileDataList[index++];
            // 第一行非注释为表头
            if (line.IndexOf(REMARK_SIGN) == 0) {
                continue;
            }

            return line.Split(SPLITTER_SIGN);
        }

        return null;
    }

    private static List<T> ReadFileContent<T>(int index, string[] fields, List<string> fileDataList) {
        List<T> objList = new List<T>();
        int fileDataLen = fileDataList.Count;
        while (index < fileDataLen) {
            string line = fileDataList[index++];
            if (line.IndexOf(REMARK_SIGN) == 0) {
                continue;
            }

            string[] datas = line.Split(SPLITTER_SIGN);
            T inst = Activator.CreateInstance<T>();
            Type t = inst.GetType();
            for (int i = 0; i < fields.Length; i++) {
                FieldInfo fi = t.GetField(fields[i]);
                if (fi == null) {
                    continue;
                }

                fi.SetValue(inst, MFTypeUtil.ChangeType(datas[i], fi.FieldType));
            }

            objList.Add(inst);
        }

        return objList;
    }

    private static bool CheckFileField<T>(string[] fields) {
        if (fields == null || fields.Length == 0) {
            return false;
        }

        HashSet<string> fieldSet = new HashSet<string>();
        foreach(string field in fields) {
            fieldSet.Add(field);
        }

        Type t = typeof(T);
        foreach (FieldInfo pi in t.GetFields()) {
            if (!fieldSet.Contains(pi.Name)) {
                return false;
            }
        }

        return true;
    }
}
