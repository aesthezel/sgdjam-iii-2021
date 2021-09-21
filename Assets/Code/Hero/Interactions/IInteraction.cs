namespace Code.Hero.Interactions
{
    public interface IInteraction
    { 
        string Type { get; set; }
        bool Performed();
    }
}