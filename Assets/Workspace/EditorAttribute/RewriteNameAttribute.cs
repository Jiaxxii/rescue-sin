using System;
using UnityEngine;

namespace Workspace.EditorAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RewriteNameAttribute : PropertyAttribute
    {
        public string FieldName { get; }
        public string ToolTip { get; }
        public Texture2D Image { get; set; }

        public RewriteNameAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public RewriteNameAttribute(string fieldName, string tooltip)
        {
            FieldName = fieldName;
            ToolTip = tooltip;
        }
    }
}