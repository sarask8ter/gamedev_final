using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCDialogue", menuName="NPC Dialogue")]

public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines; // Mark where dialogue ends
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;

    // public AudioClip voiceSound; // in case we want to add them
    // public float voicePitch = 1f;

    public DialogueChoice[] choices;
    
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; // Dialogue line where choices appear
    public string[] choices; // Player response options
    public int[] nextDialogueIndexes; //where choice leads

}
