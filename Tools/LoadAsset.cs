using UnityEditor;
using UnityEngine.InputSystem;

namespace CMUFramework_Embark.Tools
{
    public class LoadAsset
    {
        public static InputActionAsset LoadInputActions(string path)
        {
            return AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
        }
    }
}