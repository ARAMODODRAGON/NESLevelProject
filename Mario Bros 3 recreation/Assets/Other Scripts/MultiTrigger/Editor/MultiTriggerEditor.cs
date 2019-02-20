using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.IMGUI.Controls;
/*
 * created by Domara Shlimon
 * ver 4.0
 */

[CustomEditor(typeof(MultiTrigger))]
public class MultiColliderEditor : Editor {
    ReorderableList list;
    MultiTrigger trig;
    BoxBoundsHandle boxb;
    int curbox;
    //SerializedProperty prop;

    private void OnEnable() {
        trig = (MultiTrigger)target;
        curbox = -1;
        //prop = serializedObject.FindProperty("Triggers");
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("Triggers"), true, true, true, true);

        list.elementHeight = EditorGUIUtility.singleLineHeight * 5f + 5f;
        list.drawElementCallback = BuildGUI;
        list.drawHeaderCallback = BuildTitle;
        list.onSelectCallback = OnSelectItem;
    }
    
    public void BuildGUI(Rect rect, int index, bool isActive, bool isFocused) {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2f;

        Rect labelRect = rect;
        labelRect.width = 90f;
        labelRect.height = EditorGUIUtility.singleLineHeight;

        Rect propRect = rect;
        propRect.x += labelRect.width;
        propRect.width = EditorGUIUtility.currentViewWidth - 65f - labelRect.width;
        propRect.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.BeginChangeCheck();

        #region Name Field
        EditorGUI.LabelField(labelRect, "Name", GUIStyle.none);
        EditorGUI.PropertyField(propRect, element.FindPropertyRelative("name"), GUIContent.none);
        #endregion

        #region Layermask Field
        labelRect.y += EditorGUIUtility.singleLineHeight;
        propRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(labelRect, "LayerMask", GUIStyle.none);
        EditorGUI.PropertyField(propRect, element.FindPropertyRelative("layerMask"), GUIContent.none);
        #endregion

        #region Offset Field
        labelRect.y += EditorGUIUtility.singleLineHeight;
        propRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(labelRect, "Offset", GUIStyle.none);
        EditorGUI.PropertyField(propRect, element.FindPropertyRelative("offset"), GUIContent.none);
        #endregion

        #region Size Field
        labelRect.y += EditorGUIUtility.singleLineHeight;
        propRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(labelRect, "Size", GUIStyle.none);
        EditorGUI.PropertyField(propRect, element.FindPropertyRelative("size"), GUIContent.none);
        #endregion

        #region Draw Gizmos Field
        labelRect.y += EditorGUIUtility.singleLineHeight;
        propRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(labelRect, "Draw Gizmo", GUIStyle.none);
        EditorGUI.PropertyField(propRect, element.FindPropertyRelative("DrawGizmo"), GUIContent.none);
        #endregion
        
        if (EditorGUI.EndChangeCheck()) {
            boxb = new BoxBoundsHandle() {
                wireframeColor = Color.white,
                size = trig.Triggers[curbox].size,
                center = trig.Triggers[curbox].offset + (Vector2)trig.transform.position,
                axes = BoxBoundsHandle.Axes.X | BoxBoundsHandle.Axes.Y
            };
            boxb.DrawHandle();
        }
    }

    public void BuildTitle(Rect rect) {
        EditorGUI.LabelField(rect, "Triggers");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSelectItem(ReorderableList RList) {
        curbox = RList.index;
        boxb = new BoxBoundsHandle() {
            wireframeColor = Color.white,
            size = trig.Triggers[curbox].size,
            center = trig.Triggers[curbox].offset + (Vector2)trig.transform.position,
            axes = BoxBoundsHandle.Axes.X | BoxBoundsHandle.Axes.Y
        };
        SceneView.RepaintAll();
    }

    public void OnSceneGUI() {
        if (curbox == -1) return;
        EditorGUI.BeginChangeCheck();
        boxb.DrawHandle();
        if (EditorGUI.EndChangeCheck()) {
            // record the target object before setting new values so changes can be undone/redone
            Undo.RecordObject(trig, "Size and offset changed");

            // copy the handle's updated data back to the target object
            trig.Triggers[curbox].offset = boxb.center - trig.transform.position;
            trig.Triggers[curbox].size = boxb.size;
        }

    }//*/
}
