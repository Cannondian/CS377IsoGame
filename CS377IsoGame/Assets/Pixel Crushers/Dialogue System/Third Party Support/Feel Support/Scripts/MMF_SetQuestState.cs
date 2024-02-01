using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using PixelCrushers.DialogueSystem;
using Random = UnityEngine.Random;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will set a quest and/or quest entry state.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will set a quest state and/or a quest entry state.")]
    [FeedbackPath("Dialogue System/Set Quest State")]
    public class MMF_SetQuestState : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.EventsColor; } }
        public override bool EvaluateRequiresSetup() { return string.IsNullOrEmpty(Quest); }
        public override string RequiredTargetText { get { return !string.IsNullOrEmpty(Quest) ? Quest : ""; } }
        public override string RequiresSetupText { get { return "This feedback requires that a Quest be selected. You can set one below."; } }
#endif

        [MMFInspectorGroup("Quest State Settings", true, 76, true)]
        [QuestPopup(true)]
        public string Quest;
        public bool SetQuestState;
        public QuestState NewQuestState;
        public bool SetQuestEntryState;
        [QuestEntryPopup]
        public int QuestEntry;
        public QuestState NewQuestEntryState;

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (string.IsNullOrEmpty(Quest))
            {
                return;
            }
            if (SetQuestState)
            {
                QuestLog.SetQuestState(Quest, NewQuestState);
            }
            if (SetQuestEntryState)
            {
                QuestLog.SetQuestEntryState(Quest, QuestEntry, NewQuestEntryState);
            }
        }

    }
}
