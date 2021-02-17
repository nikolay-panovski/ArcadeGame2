using System;
using GXPEngine;
using TiledMapParser;

public class TileTarget : AnimationSprite      // <--> transmitter
{
    public bool is_shot { get; set; }
    public int shoot_ID { get; private set; }
    public TileTarget(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        shoot_ID = obj.GetIntProperty("shoot_ID", 0);
    }

    private void Update()
    {
        
    }
}
