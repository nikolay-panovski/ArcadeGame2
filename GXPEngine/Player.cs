using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player : AnimationSprite
{
    protected Collision coll_info = null;
    protected Vector2 direction = new Vector2(0, 0);
    public Pivot bullet_handler { get; set; }
    protected bool is_colliding;
    public int HP { get; set; }
    public int active_ID { get; set; }
    protected float shield_timer;
    protected int i_frames;
    protected Sprite sprite_shielded = new Sprite("circle.png", false, false);

    protected float x_speed = 0.8f;

    protected float velocity_y = 0.0f;

    public Player(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        sprite_shielded.SetOrigin(sprite_shielded.width / 2, sprite_shielded.height / 2);
    }

    protected bool MoveAndCollide(float deltaX, float deltaY)   // returns true  if move is possible, false if landed on floor (can't move)
    {
        x += deltaX;
        y += deltaY;

        bool isColliding = false;
        foreach (Sprite other in GetCollisions())           // works with everything added as child to the Level
        {
                resolveCollision(this, other, deltaX, deltaY);
                isColliding = true;
        }

        return !isColliding;

    }

    private void resolveCollision(Sprite subject, Sprite collider, float deltaX, float deltaY)
    {
        // maybe useless check but not sure /shrug
        if (collider is RevolverBullet || collider is PickupAmmo || collider is PickupCell || collider is PickupDrug ||
            collider is PickupShield || collider is TileButton || collider is TileTransmitter || collider is TileLaser ||
            collider is TileWater || collider is TileAcid)
        {
            if (collider is RevolverBullet)
            {
                collider.LateDestroy();
                if (shield_timer <= 0) HP--;
                if (this is HumanPlayer) (parent as Level).game_hud.removeSpriteFromHuman();
                if (this is AlienPlayer) (parent as Level).game_hud.removeSpriteFromAlien();
            }
            if (collider is PickupAmmo)
            {
                (parent as Level).pickups_score += (collider as PickupAmmo).points_value * (parent as Level).dist_multiplier;
                (parent as Level).player1_ref.ammo += 12;
                collider.LateDestroy();
            }
            if (collider is PickupCell)
            {
                (parent as Level).pickups_score += (collider as PickupCell).points_value * (parent as Level).dist_multiplier;
                (parent as Level).number_cells++;
                collider.LateDestroy();
            }
            if (collider is PickupDrug)
            {
                (parent as Level).pickups_score += (collider as PickupDrug).points_value * (parent as Level).dist_multiplier;
                (parent.parent as MyGame).total_lives++;
                collider.LateDestroy();
            }
            if (collider is PickupShield)
            {
                (parent as Level).pickups_score += (collider as PickupShield).points_value * (parent as Level).dist_multiplier;
                shield_timer = 8;
                AddChild(sprite_shielded);
                collider.LateDestroy();
            }
            if (collider is TileButton)
            {
                active_ID = (collider as TileButton).button_ID;
                // as it turns out, a really cool bug (feature!) of quicksand collision is present here
            }
            if (collider is TileTransmitter)
            {
                if ((parent as Level).number_cells > 0 && (collider as TileTransmitter).is_charged == false)
                {
                    (parent as Level).number_cells--;
                    (parent as Level).pickups_score += 100;
                    (collider as TileTransmitter).is_charged = true;
                }
            }
            if (collider is TileLaser)      // add cooldown
            {
                if ((collider as TileLaser).is_deact == false) {
                    if (shield_timer <= 0 && i_frames == 0)
                    {
                        HP--;
                        if (this is HumanPlayer) (parent as Level).game_hud.removeSpriteFromHuman();
                        if (this is AlienPlayer) (parent as Level).game_hud.removeSpriteFromAlien();
                    }
                    i_frames = 15;
                }
            }
            if (collider is TileWater)
            {
                if (this is AlienPlayer)
                {
                    velocity_y = -3f;
                    if (i_frames == 0)
                    {
                        HP--;
                        (parent as Level).game_hud.removeSpriteFromAlien();
                    }
                        i_frames = 8;
                }
            }
            if (collider is TileAcid)
            {
                if (this is HumanPlayer)
                {
                    velocity_y = -3f;
                    if (i_frames == 0)
                    {
                        HP--;
                        (parent as Level).game_hud.removeSpriteFromHuman();
                    }
                    i_frames = 8;
                }
            }
        }

        //else  // ELSE is not any object we want special collision with -> treat as walls etc.
                // seems to run for everything anyway
                // quicksand gone, otherwise water and acid won't work
        //{
            if (deltaX < 0) subject.x = (collider.x + collider.width / 2 + subject.width / 2);  // refactored for origin at middle
            if (deltaX > 0) subject.x = (collider.x - collider.width / 2 - subject.width / 2);
            if (deltaY < 0) subject.y = (collider.y + collider.height / 2 + subject.height / 2) + 1;
            if (deltaY > 0) subject.y = (collider.y - collider.height / 2 - subject.height / 2) - 1;
        //}

        if (collider is TileButton == false) active_ID = -1;    // takes collision with something else to reset, but shh
    }

    protected void handleCollisions()   // do I really have to do this?
    {
        if (coll_info != null)
        {
            if (coll_info.other is RevolverBullet)
            {
                if (shield_timer <= 0) HP--;
                (parent as Level).game_hud.removeSpriteFromHuman();
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupAmmo)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupAmmo).points_value * (parent as Level).dist_multiplier;
                (parent as Level).player1_ref.ammo += 12;
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupCell)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupCell).points_value * (parent as Level).dist_multiplier;
                (parent as Level).number_cells++;
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupDrug)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupDrug).points_value * (parent as Level).dist_multiplier;
                (parent.parent as MyGame).total_lives++;
                coll_info.other.Destroy();
            }
            if (coll_info.other is PickupShield)
            {
                (parent as Level).pickups_score += (coll_info.other as PickupShield).points_value * (parent as Level).dist_multiplier;
                shield_timer = 8;
                AddChild(sprite_shielded);
                coll_info.other.Destroy();
            }
            if (coll_info.other is TileLaser)   // does not work? ok, still solid, so... cool with me / i-frames if necessary
            {
                if (shield_timer <= 0) HP--;
                if (this is HumanPlayer) (parent as Level).game_hud.removeSpriteFromHuman();
                if (this is AlienPlayer) (parent as Level).game_hud.removeSpriteFromAlien();
            }
            // don't access water and acid from the side, ever
        }
    }

    /*protected void Update()
    {
        
    }*/
}