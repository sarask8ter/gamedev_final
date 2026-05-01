public class TalkToNeighbor : DialogueChoiceAction
{
    protected override void Execute()
    {
        GameState.TalkedToNeighbor = true;
    }
}
