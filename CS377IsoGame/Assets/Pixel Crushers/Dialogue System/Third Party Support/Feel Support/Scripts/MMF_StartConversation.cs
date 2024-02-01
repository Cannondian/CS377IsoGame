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
    /// This feedback will start a conversation.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will start a conversation.")]
    [FeedbackPath("Dialogue System/Start Conversation")]
    public class MMF_StartConversation : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.EventsColor; } }
        public override bool EvaluateRequiresSetup() { return string.IsNullOrEmpty(ConversationTitle); }
        public override string RequiredTargetText { get { return !string.IsNullOrEmpty(ConversationTitle) ? ConversationTitle : ""; } }
        public override string RequiresSetupText { get { return "This feedback requires that a Conversation Title be selected. You can set it below."; } }
#endif

        [MMFInspectorGroup("Conversation Settings", true, 76, true)]
        [ConversationPopup(true)]
        public string ConversationTitle;
        [Tooltip("The conversation actor (optional)")]
        public Transform BoundConversationActor;
        [Tooltip("The conversation conversant (optional)")]
        public Transform BoundConversationConversant;
        [Tooltip("Stop any active conversations when starting this conversation")]
        public bool ReplaceOtherActiveConversations;
        [Tooltip("Do not start conversation if another conversation is already active")]
        public bool SkipIfAnotherIsActive;

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (!string.IsNullOrEmpty(ConversationTitle))
            {
                if (SkipIfAnotherIsActive && DialogueManager.isConversationActive)
                {
                    return;
                }
                if (ReplaceOtherActiveConversations)
                {
                    DialogueManager.StopAllConversations();
                }
                DialogueManager.StartConversation(ConversationTitle, BoundConversationActor, BoundConversationConversant);
            }
        }

    }
}
