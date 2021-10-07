using UnityEngine;
using UnityEditor;

namespace Utils.Editor
{
    [InitializeOnLoad]
    public class HierarchyLabel : MonoBehaviour
    {
        static HierarchyLabel()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject label = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (label != null)
            {
                if (label.name.StartsWith("---", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, ColorPalette.GREY);
                    EditorGUI.DropShadowLabel(selectionRect, label.name.Replace("-", "").ToString());
                } 
                else if (label.name.StartsWith("!!!", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, ColorPalette.RED);
                    EditorGUI.DropShadowLabel(selectionRect, label.name.Replace("!", "").ToString());
                } 
                else if (label.name.StartsWith("###", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, ColorPalette.BLUE);
                    EditorGUI.DropShadowLabel(selectionRect, label.name.Replace("#", "").ToString());
                }
                else if (label.name.StartsWith("???", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, ColorPalette.ORANGE);
                    EditorGUI.DropShadowLabel(selectionRect, label.name.Replace("?", "").ToString());
                }
                else if (label.name.StartsWith("===", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, ColorPalette.GREEN);
                    EditorGUI.DropShadowLabel(selectionRect, label.name.Replace("=", "").ToString());
                }
            }
        }
    }

    public static class ColorPalette
    {
        public static Color RED = new Color(0.74f, 0.31f, 0.27f);
        public static Color BLUE = new Color(0.18f, 0.36f, 0.59f);
        public static Color GREEN = new Color(0.48f, 0.57f, 0.14f);
        public static Color ORANGE = new Color(0.84f, 0.67f, 0.35f);
        public static Color GREY = new Color(0.42f, 0.44f, 0.45f);
    }
}