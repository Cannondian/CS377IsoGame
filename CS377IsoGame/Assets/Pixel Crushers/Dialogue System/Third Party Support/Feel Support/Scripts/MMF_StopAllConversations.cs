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
    /// This feedback will stop all conversations.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will stop all conversations.")]
    [FeedbackPath("Dialogue System/Stop All Conversations")]
    public class MMF_StopConversations : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.EventsColor; } }
#endif

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            DialogueManager.StopAllConversations();
        }

    }
}
