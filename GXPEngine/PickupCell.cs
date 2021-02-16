using System;
using GXPEngine;

public class PickupCell : AnimationSprite
{
    private const float MIN_SPD = 1.2f;
    private const float MAX_SPD = 3.6f;
    private float speed = Utils.Random(MIN_SPD, MAX_SPD);
    public PickupCell() : base("point_16x16.png", 1, 1)
    {
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        Move(-speed, 0);    // this can be removed from here and spawner and placed manually in Tiled
    }
}
