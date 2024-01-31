using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Checks the dialogue entry for a field named "Position" and sets
    /// the subtitle panel animator accordingly. You can use "Position"
    /// to show the bubble in a centered position instead of the default
    /// right-side position.
    /// </summary>
    public class SetBubblePosition : MonoBehaviour
    {

        private const string PositionFieldTitle = "Position";
        private const string PositionParameter = "Position";

        private Animator bubbleAnimator;

        private void Start()
        {
            var subtitlePanel = GetComponentInChildren<StandardUISubtitlePanel>();
            subtitlePanel.onOpen.AddListener(SetPositionOnOpen);
            bubbleAnimator = subtitlePanel.GetComponentInChildren<Animator>();
        }

        private void SetPositionOnOpen()
        {
            if (DialogueManager.currentConversationState == null) return;
            var entry = DialogueManager.currentConversationState.subtitle.dialogueEntry;
            if (Field.FieldExists(entry.fields, PositionFieldTitle))
            {
                var position = Field.LookupInt(entry.fields, PositionFieldTitle);
                bubbleAnimator.SetInteger(PositionParameter, position);
            }
        }

    }
}

