namespace ClientSide;

public class Sender
{
    public string? Name { get; set; }
    public string? RecepientName { get; set; }
    public string? Message { get; set; }

    public override string ToString()
    {
        return $"{Name} - den  Sene olan message: {Message}";
    }

}
