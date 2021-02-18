using System;
using GXPEngine;
using TiledMapParser;

public class TileDoor : AnimationSprite         // <--> laser
{
    public bool is_open { get; set; }
    public bool open_locked { get; set; }       // hold that one open forever :)
    public int door_ID { get; private set; }
    public TileDoor(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        door_ID = obj.GetIntProperty("door_ID", 0);
    }

    public void openDoor()
    {
        if (is_open == false)
        {
            y -= 44 * MyGame.TILE_SIZE; // for some nice out of bounds
        }
    }

    public void closeDoor()
    {
        if (is_open == true)
        {
            y += 44 * MyGame.TILE_SIZE;
        }
    }

    private void Update()
    {
        
    }
}
