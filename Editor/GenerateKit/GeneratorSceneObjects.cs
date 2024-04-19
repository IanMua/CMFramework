#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMFramework.Editor
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

    public class GeneratorCount
    {
        public List<int> used;
        public int maxIndex;

        public GeneratorCount(List<int> used, int maxIndex)
        {
            this.used = used;
            this.maxIndex = maxIndex;
        }
    }

    public class GeneratorSceneObjects
    {
        private StreamWriter _writer;
        private readonly Dictionary<string, GeneratorCount> _namedCount = new Dictionary<string, GeneratorCount>();

        public void GenerateScript(UnityEngine.SceneManagement.Scene scene)
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

                CheckForChildren(selfParentPath);
            }
        }

        private string Filter(string input)
        {
            // 匹配规则 只保留大小写英文字母和数字，并且不保留(数字)
            string pattern = @"[^a-zA-Z0-9]";
            // 不符合规则的要替换成
            string replacement = "";

            input = Regex.Replace(input, pattern, replacement);

            // 如果第一位不符合C#命名规则，就在前面加上Filter
            if (!Regex.IsMatch(input[0].ToString(), @"[a-zA-Z_@]"))
            {
                return "Filter" + input;
            }

            return input;
        }

        private string GetName(string input)
        {
            string filter = Filter(input);

            string pattern = @"\d+$";
            // 先获取到结尾的连续数字
            int num = 0;
            Match match = Regex.Match(filter, pattern);
            // 判断结尾是否有数字
            if (match.Success)
            {
                // 如果有数字，执行有数字的逻辑

                // 把字符串转换成数字
                num = int.Parse(match.Value);

                // 判断是否在字典中，判断没有结尾连续数字后的是否在字典中
                string tempFilter = filter.Replace(num.ToString(), "");
                if (_namedCount.ContainsKey(tempFilter))
                {
                    GeneratorCount count = _namedCount[tempFilter];

                    // 如果在字典中，判断数字是否超过最大值
                    bool numFlag = num > count.maxIndex;
                    if (numFlag)
                    {
                        // 把最大值设置为当前数字
                        count.maxIndex = num;
                        // 把数字添加到记录中
                        count.used.Add(num);
                        return $"{filter}";
                    }
                    else
                    {
                        // 如果没有超过最大值，查找记录中是否存在
                        for (int i = 0; i < count.used.Count; i++)
                        {
                            // 如果记录中存在相同的值，就设置为最大值+1
                            if (count.used[i] == num)
                            {
                                // 把最大值+1，放到使用记录中
                                count.maxIndex += 1;
                                count.used.Add(count.maxIndex);
                                return $"{tempFilter}{count.maxIndex}";
                            }
                        }

                        // 如果记录中不存在，就把值添加到记录中
                        count.used.Add(num);
                        return $"{filter}";
                    }
                }
                // 不在字典中
                else
                {
                    _namedCount.Add(tempFilter, new GeneratorCount(new List<int>(), num));
                    // 添加使用记录
                    _namedCount[tempFilter].used.Add(num);
                    return filter;
                }
            }
            // 如果没有数字
            else
            {
                // 如果字典中存在
                if (_namedCount.ContainsKey(filter))
                {
                    // 最大值+1，存入使用记录
                    _namedCount[filter].maxIndex += 1;
                    _namedCount[filter].used.Add(_namedCount[filter].maxIndex);
                    return $"{filter}{_namedCount[filter].maxIndex}";
                }
                // 字典中不存在
                else
                {
                    _namedCount.Add(filter, new GeneratorCount(new List<int>(), 0));
                    return filter;
                }
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