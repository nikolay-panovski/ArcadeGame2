using System;
using GXPEngine;

public class HUD : GameObject
{
    private EasyDraw text_elements;

    public HUD() : base()
    {
        /* shared lives - on main level screen? centered?
         * individual HP - position?
         * ammo
         * pickups (energy cells)
         * ability cooldowns
         */
        text_elements = new EasyDraw(game.width, game.height, false);
    }
}
