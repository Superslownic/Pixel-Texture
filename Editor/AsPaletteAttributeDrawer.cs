using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Pixel.Texture
{
    public sealed class AsPaletteAttributeDrawer : OdinAttributeDrawer<AsPaletteAttribute, Color[]>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            int spacing = 2;
            int count = (int)Mathf.Sqrt(ValueEntry.SmartValue.Length);
            Rect rect = EditorGUILayout.GetControlRect(false, Property.LastDrawnValueRect.width);
            float startPosition = rect.x;
            rect.height = rect.width;
            EditorGUILayout.GetControlRect(false, rect.width);
            
            var colorRect = new Rect(rect);
            colorRect.width -= (count - 1) * spacing;
            colorRect.width /= count;
            colorRect.height = colorRect.width;
            colorRect.y = rect.yMax;
            
            for (int i = 0; i < ValueEntry.SmartValue.Length; i++)
            {
                if (i % count == 0)
                {
                    colorRect.x = startPosition;
                    colorRect.y -= colorRect.height + spacing;
                }

                ValueEntry.SmartValue[i] = EditorGUI.ColorField(
                    colorRect,
                    GUIContent.none, 
                    ValueEntry.SmartValue[i], 
                    false, 
                    false, 
                    false);
                
                colorRect.x += colorRect.width + spacing;
            }
        }
    }
}