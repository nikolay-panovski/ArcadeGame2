using System;
using GXPEngine;
using TiledMapParser;

public class TileTransmitter : AnimationSprite      // <--> target
{
    public bool is_charged { get; set; }
    public int charge_ID { get; private set; }
    public TileTransmitter(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        charge_ID = obj.GetIntProperty("charge_ID", 0);
    }

    private void Update()
    {

    }
}
