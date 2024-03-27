using System;
using UnityEditor;
using UnityEngine;
using Workspace.EditorAttribute;

namespace Editor
{
    [CustomPropertyDrawer(typeof(RewriteNameAttribute))]
    public class RewriteNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is not RewriteNameAttribute rewriteName)
            {
                return;
            }

            var guiContent = new GUIContent(rewriteName.FieldName, rewriteName.Image, rewriteName.ToolTip);

            EditorGUI.PropertyField(position, property, guiContent);
        }
    }
    
    

    
}