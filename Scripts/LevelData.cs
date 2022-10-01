using Godot;

public class LevelData
{
    public string Name;

    public LevelData() { }
    public LevelData(string name)
    {
        Name = name;
    }

    public Node Load()
    {
        return ResourceLoader.Load<PackedScene>($"res://Scenes/Levels/{Name}.tscn").Instance();
    }
}