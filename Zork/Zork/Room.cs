namespace Zork
{
    public class Room
    {
        // Name of the room.
        public string Name { get; }

        // Description of the room.
        public string Description { get; set; }

        // Constructor that takes in the name and an optional description.
        public Room(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public override string ToString() => Name;
    }
}