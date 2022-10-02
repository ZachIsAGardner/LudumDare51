using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MiniGames : Node
{
    float duration = 10;
    float time;

    int hp = 3;

    Node container;
    Label timerLabel;
    Label titleLabel;
    Label hpLabel;

    string lastLevel = "";

    List<LevelData> datas = new List<LevelData>()
    {
        new LevelData("Test"),
        new LevelData("Press"),
    };

    public override void _Ready()
    {
        base._Ready();

        time = duration;

        timerLabel = this.GetChild("Timer") as Label;
        titleLabel = this.GetChild("Title") as Label;
        hpLabel = this.GetChild("Hp") as Label;
        container = this.GetChild("Container");

        Load();
    }

    public override void _Process(float delta)
    {
        UpdateTimer(delta);
    }

    public void Load()
    {
        time = duration;
        container.RemoveChildren();

        LevelData data = datas.Where(d => d.Name != lastLevel).Random();
        lastLevel = data.Name;

        Level level = data.Load() as Level;
        level.OnComplete += () =>
        {
            Load();
        };

        container.AddChild(level);
        Info info = level.GetChild<Info>();
        titleLabel.Text = info?.Title ?? "";
    }

    public void UpdateTimer(float delta)
    {
        time -= delta;

        titleLabel.Visible = time >= 7.5f;

        if (time <= 0) time = 0;

        string content = $"{time.RoundTo(3)}";
        while (content.Length < 5)
        {
            if (!content.Contains(".")) content += ".";
            content += "0";
        }

        timerLabel.Text = $"{content}s";

        if (time <= 0)
        {
            time = duration;

            hp--;
            hpLabel.Text = $"HP: {hp}";
            if (hp <= 0)
            {
                Game.Load("GameOver");
            }
            else
            {
                Load();
            }
        }
    }
}
