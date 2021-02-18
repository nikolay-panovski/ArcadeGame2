using System;
using GXPEngine;
using TiledMapParser;

public class TileLaser : AnimationSprite        // <--> door
{
    public bool is_deact { get; set; }
    public bool deact_locked { get; set; }       // hold that one open forever :)
    public int laser_ID { get; private set; }
    public TileLaser(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        laser_ID = obj.GetIntProperty("laser_ID", 0);
    }

    public void deactivateLaser()
    {
        if (is_deact == false)
        {
            y -= 44 * MyGame.TILE_SIZE;
        }
    }

    public void activateLaser()
    {
        if (is_deact == true)
        {
            y += 44 * MyGame.TILE_SIZE;
        }
    }

    private void Update()
    {
        
    }
}
