using GameSystem.Utils;
using GameSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace GameSystem.Editors
{
    [CustomEditor(typeof(CardCommandView))]
    public class CardCommandViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            var cardCommandNameSp = serializedObject.FindProperty("_cardCommandName");
            var cardcommandName = cardCommandNameSp.stringValue;

            var typeNames = CardCommandProviderTypeHelper.FindCardCommandProviderTypes();

            var selectedIndex = Array.IndexOf(typeNames, cardcommandName);
            var newSelectedIdx = EditorGUILayout.Popup("Command", selectedIndex, typeNames);

            if (selectedIndex != newSelectedIdx)
            {
                cardCommandNameSp.stringValue = typeNames[newSelectedIdx];
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
