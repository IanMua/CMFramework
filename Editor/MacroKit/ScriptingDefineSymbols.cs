#if UNITY
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace CMFramework.Editor
{
    public static class ScriptingDefineSymbols
    {
        /// <summary>
        /// 获取当前构建平台
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static NamedBuildTarget GetBuildTarget(BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            if (targetGroup == BuildTargetGroup.Unknown)
            {
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            }

            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            if (targetGroup == BuildTargetGroup.Unknown)
            {
                throw new Exception("无法获取到当前构建平台");
            }

            return namedBuildTarget;
        }

        /// <summary>
        /// 获取构建平台所有的自定义宏
        /// </summary>
        /// <param name="targetGroup"></param>
        /// <returns></returns>
        private static List<string> GetScriptingDefineSymbols(BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            NamedBuildTarget namedBuildTarget = GetBuildTarget(targetGroup);

            // 获取宏列表
            string definesString = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget).Trim();
            return definesString.Split(';').ToList();
        }

        /// <summary>
        /// 覆写平台自定义宏
        /// </summary>
        /// <param name="macros"></param>
        /// <param name="targetGroup"></param>
        private static void OverrideScriptingDefineSymbols(List<string> macros,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            NamedBuildTarget namedBuildTarget = GetBuildTarget(targetGroup);

            // 更新宏定义
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(";", macros));
        }

        /// <summary>
        /// 删除自定义宏
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="targetGroup"></param>
        public static void RemoveCustomMacro(string macro, BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            if (macros.Contains(macro))
            {
                macros.Remove(macro);
                OverrideScriptingDefineSymbols(macros, targetGroup);
            }
        }

        /// <summary>
        /// 批量删除自定义宏
        /// </summary>
        /// <param name="macroList"></param>
        /// <param name="targetGroup"></param>
        public static void BatchRemoveCustomMacro(List<string> macroList,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            foreach (string macro in macroList)
            {
                if (macros.Contains(macro))
                {
                    macros.Remove(macro);
                }
            }

            OverrideScriptingDefineSymbols(macros, targetGroup);
        }

        /// <summary>
        /// 添加自定义宏
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="targetGroup"></param>
        public static void AddCustomMacro(string macro, BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            if (!macros.Contains(macro))
            {
                macros.Add(macro);
                OverrideScriptingDefineSymbols(macros, targetGroup);
            }
        }

        /// <summary>
        /// 批量添加自定义宏
        /// </summary>
        /// <param name="macroList"></param>
        /// <param name="targetGroup"></param>
        public static void BatchAddCustomMacro(List<string> macroList,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            foreach (string macro in macroList)
            {
                if (!macros.Contains(macro))
                {
                    macros.Add(macro);
                }
            }

            OverrideScriptingDefineSymbols(macros, targetGroup);
        }

        /// <summary>
        /// 替换自定义宏
        /// </summary>
        /// <param name="replacer">替换者-被替换者</param>
        /// <param name="targetGroup"></param>
        public static void ReplaceCustomMacro(KeyValuePair<string, string> replacer,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            if (macros.Contains(replacer.Value))
            {
                macros.Remove(replacer.Value);
                macros.Add(replacer.Key);
                OverrideScriptingDefineSymbols(macros, targetGroup);
            }
        }

        /// <summary>
        /// 批量替换自定义宏
        /// </summary>
        /// <param name="replaceList"></param>
        /// <param name="targetGroup"></param>
        public static void BatchReplaceCustomMacro(List<KeyValuePair<string, string>> replaceList,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);

            foreach (KeyValuePair<string, string> replacePair in replaceList)
            {
                if (macros.Contains(replacePair.Value))
                {
                    macros.Remove(replacePair.Value);
                    macros.Add(replacePair.Key);
                }
            }

            OverrideScriptingDefineSymbols(macros, targetGroup);
        }

        /// <summary>
        /// 是否存在自定义宏中
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="targetGroup"></param>
        /// <returns></returns>
        public static bool IsInMacro(string macro,
            BuildTargetGroup targetGroup = BuildTargetGroup.Unknown)
        {
            List<string> macros = GetScriptingDefineSymbols(targetGroup);
            return macros.Contains(macro);
        }
    }
}
#endif