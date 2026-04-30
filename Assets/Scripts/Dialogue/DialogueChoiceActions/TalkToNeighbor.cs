using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoice - Talk To Neighbor", menuName = "Dialogue/Choice Actions/Talk to Neighbor")]
public class TalkToNeighbor : DialogueChoiceAction
{
    public override void Execute()
    {
        GameState.TalkedToNeighbor = true;
    }
}
