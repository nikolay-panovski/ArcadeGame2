using System;
using GXPEngine;
using TiledMapParser;

public class TileButton : AnimationSprite
{
    public bool is_active { get; set; }
    public bool sound_played { get; set; }
    public int button_ID { get; private set; }
    public TileButton(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        button_ID = obj.GetIntProperty("button_ID", 0);
    }

    private void Update()
    {
        
    }
}
