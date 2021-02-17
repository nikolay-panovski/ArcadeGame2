using System;
using GXPEngine;
using TiledMapParser;

public class PickupDrug : AnimationSprite      // death killer drug or something
{
    public int points_value { get; private set; }
    public PickupDrug(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        points_value = 100;
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        
    }
}
