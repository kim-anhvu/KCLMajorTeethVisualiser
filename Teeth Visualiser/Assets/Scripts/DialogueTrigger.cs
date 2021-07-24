using UnityEngine;


/**
 * Simple class used to trigger the Helper Dialogue
 */
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void Trigger() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
