using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMFramework
{
    public class SceneObjectsGenerator : EditorWindow
    {
        private static StreamWriter _writer;
        private static readonly Dictionary<string, int> NamedCount = new Dictionary<string, int>();

        [MenuItem("CMFramework/GenerateKit/GenerateSceneObjects")]
        public static void GenerateScript()
        {
            string folderPath = Path.Combine(Application.dataPath, "Scripts/SceneObjects");
            string scriptPath = $"{folderPath}/{SceneManager.GetActiveScene().name}SceneObjects.cs";

            // 检查文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 开始写入文件
            _writer = new StreamWriter(scriptPath);

            _writer.WriteLine("using UnityEngine;");
            _writer.WriteLine();
            _writer.WriteLine($"public class {SceneManager.GetActiveScene().name}SceneObjects");
            _writer.WriteLine("{");

            // 获取所有根物体
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                _writer.WriteLine(
                    $"    public GameObject {GetName(rootObject.name)} => GameObject.Find(\"{rootObject.name}\");");
                CheckForChildren(new List<Transform>() { rootObject.transform });
            }

            _writer.WriteLine("}");
            _writer.Flush();
            _writer.Close();

            AssetDatabase.Refresh();
        }

        private static void CheckForChildren(List<Transform> parentPath)
        {
            if (parentPath.Count == 0)
            {
                return;
            }

            // 获取 parentPath 最后一个，最后一个是父物体
            for (int i = 0; i < parentPath[^1].childCount; i++)
            {
                // 获取父物体下的子物体
                Transform child = parentPath[^1].GetChild(i);
                
                
                List<Transform> selfParentPath = new List<Transform>();
                selfParentPath.AddRange(parentPath);

                selfParentPath.Add(child);

                // 先把跟物体写出来
                _writer.Write(
                    $"    public GameObject {GetName(child.name)} => GameObject.Find(\"{selfParentPath[0].name}\").transform");

                // 然后再挨个遍历子物体
                for (int j = 1; j < selfParentPath.Count; j++)
                {
                    _writer.Write($".Find(\"{selfParentPath[j].name}\")");
                }

                _writer.Write(".gameObject;");
                _writer.WriteLine();

                selfParentPath.Add(child);

                CheckForChildren(selfParentPath);
            }
        }

        private static string Filter(string input)
        {
            // 匹配规则 只保留大小写英文字母和数字，并且不保留(数字)
            string pattern = @"\(\d+\)|[^a-zA-Z0-9]";
            // 不符合规则的要替换成
            string replacement = "";

            return Regex.Replace(input, pattern, replacement);
        }

        private static string GetName(string input)
        {
            string filter = Filter(input);

            Debug.Log($"namedCount: {NamedCount}");
            Debug.Log($"filter: {filter}");

            if (NamedCount.ContainsKey(filter))
            {
                return $"{filter}{++NamedCount[filter]}";
            }
            else
            {
                NamedCount.Add(filter, 0);
                return filter;
            }
        }
    }
}