public interface IInteractable
{
    public bool canInteract { get; set; }

    public void Interaction();
    public void ResetInteraction();
}