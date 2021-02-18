using System;
using GXPEngine;
using TiledMapParser;

public class TileAcid : AnimationSprite
{
    public TileAcid(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        
    }
}
