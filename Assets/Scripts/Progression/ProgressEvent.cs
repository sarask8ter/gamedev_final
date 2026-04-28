public enum ProgressEvent
{
    None,
    GameStart,
    MoveInMonologue,
    SearchMat,
    KeyNotFoundMonologue,
    FindKeyInTrash,
    KeyFoundMonologue,
    MoveInBoxes,
    DoorKnock,
    GoToNeighborsHouse,
    NeighborWelcome,
    ExploreNeighborsHouse,
    TeaReadyDialogue,
    TeaReady,
    StayTheNightDialogue,
    StayTheNightDecision,
    // Events that only happen if investigating blood/clues.
    FindToolToOpenDoor,
    OpenBloodiedDoor,
}