using System;
using GXPEngine;
using TiledMapParser;

public class PickupAmmo : AnimationSprite
{
    public int points_value { get; private set; }
    public PickupAmmo(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        points_value = 50;
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        
    }
}
