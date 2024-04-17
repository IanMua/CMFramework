#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMFramework
{
    public class GeneratorSceneObjectsMenu : EditorWindow
    {
        [MenuItem("CMFramework/GenerateKit/GenerateActiveSceneObjects")]
        public static void GenerateActiveScene()
        {
            new GeneratorSceneObjects().GenerateScript(SceneManager.GetActiveScene());
        }

        [MenuItem("CMFramework/GenerateKit/GenerateAllSceneObjects")]
        public static void GenerateAllSceneScript()
        {
            GeneratorSceneObjects generatorSceneObjects = new GeneratorSceneObjects();

            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                generatorSceneObjects.GenerateScript(SceneManager.GetSceneAt(i));
            }
        }
    }

    public class GeneratorSceneObjects
    {
        private StreamWriter _writer;
        private readonly Dictionary<string, int> _namedCount = new Dictionary<string, int>();

        public void GenerateScript(Scene scene)
        {
            string folderPath = Path.Combine(Application.dataPath, "Scripts/SceneObjects");
            string scriptPath = $"{folderPath}/{scene.name}SceneObjects.cs";

            // 检查文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 开始写入文件
            _writer = new StreamWriter(scriptPath);

            _writer.WriteLine("using CMFramework;");
            _writer.WriteLine("using UnityEngine;");
            _writer.WriteLine();
            _writer.WriteLine(
                $"public class {Filter(scene.name)}SceneObjects : Singleton<{Filter(scene.name)}SceneObjects>");
            _writer.WriteLine("{");

            // 获取所有根物体
            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                string fieldName = $"_{GetName(LetterToLower(rootObject.name))}";
                string propertyName = $"{GetName(rootObject.name)}";
                _writer.WriteLine($"    # region {propertyName}");
                // 字段
                _writer.WriteLine(
                    $"    private GameObject {fieldName};");
                // 属性
                _writer.WriteLine(
                    $"    public GameObject {propertyName}"
                );
                _writer.WriteLine("    {");
                _writer.WriteLine("        get");
                _writer.WriteLine("        {");
                _writer.WriteLine($"            if ({fieldName} != null) return {fieldName};");
                _writer.WriteLine($"            {fieldName} = GameObject.Find(\"{rootObject.name}\");");
                _writer.WriteLine($"            return {fieldName};");
                _writer.WriteLine("        }");
                _writer.WriteLine("    }");

                CheckForChildren(new List<Transform>() { rootObject.transform });
                _writer.WriteLine($"    #endregion");

                _writer.WriteLine();
            }

            _writer.WriteLine("}");
            _writer.Flush();
            _writer.Close();

            AssetDatabase.Refresh();
        }

        private void CheckForChildren(List<Transform> parentPath)
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

                _writer.WriteLine();
                // 字段
                string fieldName = $"_{GetName(LetterToLower(child.name))}";
                _writer.WriteLine(
                    $"    private GameObject {fieldName};");
                // 属性
                string propertyName = $"{GetName(child.name)}";
                _writer.WriteLine(
                    $"    public GameObject {propertyName}"
                );
                _writer.WriteLine("    {");
                _writer.WriteLine("        get");
                _writer.WriteLine("        {");
                _writer.WriteLine($"            if ({fieldName} != null) return {fieldName};");
                _writer.Write($"            {fieldName} = GameObject.Find(\"{selfParentPath[0].name}\").transform");

                // 然后再挨个遍历子物体
                for (int j = 1; j < selfParentPath.Count; j++)
                {
                    _writer.Write($".Find(\"{selfParentPath[j].name}\")");
                }

                _writer.WriteLine($".gameObject;");
                _writer.WriteLine($"            return {fieldName};");
                _writer.WriteLine("        }");
                _writer.WriteLine("    }");
                _writer.WriteLine();

                selfParentPath.Add(child);

                CheckForChildren(selfParentPath);
            }
        }

        private string Filter(string input)
        {
            // 匹配规则 只保留大小写英文字母和数字，并且不保留(数字)
            string pattern = @"\(\d+\)|[^a-zA-Z0-9]";
            // 不符合规则的要替换成
            string replacement = "";

            return Regex.Replace(input, pattern, replacement);
        }

        private string GetName(string input)
        {
            string filter = Filter(input);

            Debug.Log($"namedCount: {_namedCount}");
            Debug.Log($"filter: {filter}");

            if (_namedCount.ContainsKey(filter))
            {
                return $"{filter}{++_namedCount[filter]}";
            }
            else
            {
                _namedCount.Add(filter, 0);
                return filter;
            }
        }

        private string LetterToLower(string input)
        {
            if (!string.IsNullOrEmpty(input) && char.IsLetter(input[0]))
            {
                return char.ToLower(input[0]) + input.Substring(1);
            }
            else
            {
                return input;
            }
        }
    }
}

#endif