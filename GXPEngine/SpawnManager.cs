using System;
using System.Collections.Generic;
using GXPEngine;

public class SpawnManager : GameObject
{
    /* spawns: what - coins/hearts/shields/whatever (pickups);
     *         when - define drop rates;
     *         where - on random positions on the screen, probably limit to above floor+characters
     *         what2 - structures with platforms
     */
    private float spawn_timer = 0;
    private const int MAX_RNG = 256;
    private List<AnimationSprite> object_list = new List<AnimationSprite>();
    public SpawnManager() : base()
    {
        SetXY(game.width + 200, 0);     // hide past the right side of the screen
    }

    private void spawnPickups()
    {
        int rng = Utils.Random(0, MAX_RNG);
        /*if (rng % 10 == 0)
        {
            PickupShield shield = new PickupShield();
            parent.AddChild(shield);
            shield.SetXY(this.x, Utils.Random(shield.height, game.height - MyGame.TILE_SIZE));
        }
        else if (rng % 10 != 0 && rng % 5 == 0)
        {*/
            PickupHeart drugs = new PickupHeart();
            parent.AddChild(drugs);
            drugs.SetXY(this.x, game.height / 2 - MyGame.TILE_SIZE - 32);
        /*}
        else
        {
            AnimationSprite coin = new PickupCoin();
            parent.AddChild(coin);
            coin.SetXY(this.x, Utils.Random(game.height - MyGame.TILE_SIZE - 32, game.height - MyGame.TILE_SIZE - 32));
        }*/
    }

    private void Update()
    {
        spawn_timer += Time.deltaTime / 1000f;
        if (spawn_timer >= 1.2f)
        {
            spawnPickups();
            spawn_timer = 0;
        }
    }
}
