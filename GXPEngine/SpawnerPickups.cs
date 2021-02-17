using System;
using System.Collections.Generic;
using GXPEngine;

public class SpawnerPickups : GameObject
{
    /* deprecate - object positions will be hardcode level design'd
     */
    private float spawn_timer = 0;
    private const int MAX_RNG = 256;
    private List<AnimationSprite> object_list = new List<AnimationSprite>();
    public SpawnerPickups() : base()
    {
        SetXY(game.width + 200, 0);     // hide past the right side of the screen
    }

    private void spawnPickups()
    {
        /*int rng = Utils.Random(0, MAX_RNG);
        if (rng % 10 == 0)
        {
            PickupShield shield = new PickupShield();
            parent.AddChild(shield);
            shield.SetXY(this.x, Utils.Random(shield.height, game.height / 2 - MyGame.TILE_SIZE));
        }
        else if (rng % 10 != 0 && rng % 5 == 0)
        {
            PickupDrug drugs = new PickupDrug();
            parent.AddChild(drugs);
            drugs.SetXY(this.x, Utils.Random(drugs.height, game.height / 2 - MyGame.TILE_SIZE));
        }
        else if (rng % 10 != 0 && rng % 5 != 0 && rng % 3 == 0)
        {
            PickupCell coin = new PickupCell();
            parent.AddChild(coin);
            coin.SetXY(this.x, Utils.Random(coin.height, game.height / 2 - MyGame.TILE_SIZE));
        }
        else return;*/
    }

    private void Update()
    {
        /*spawn_timer += Time.deltaTime / 1000f;
        if (spawn_timer >= 1.2f)
        {
            spawnPickups();
            spawn_timer = 0;
        }*/
    }
}
