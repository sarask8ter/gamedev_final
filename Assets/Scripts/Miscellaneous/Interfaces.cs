interface IInteractable
{
    public bool IsInteractable { get; }
    public void Interact(PlayerInteractor player);
}