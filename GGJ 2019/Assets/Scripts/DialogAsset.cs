using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SubjectNerd.Utilities;

[CreateAssetMenu(menuName = "Dialog")]
public class DialogAsset : ScriptableObject
{
    [Reorderable]
    public DialogEntry[] DialogEntries;

    [Serializable]
    public class DialogEntry
    {
        [SerializeField]
        [TextArea]
        public string EntryText;
        [SerializeField]
        [Tooltip("Text speed factor (0.0 = full speed)")]
        [Range(-3.0f, 3.0f)]
        public float TextSpeed = 0.0f;
        [SerializeField]
        public event_object ExecuteOnEnter;
        [SerializeField]
        public event_object ExecuteOnExit;
    }
}
