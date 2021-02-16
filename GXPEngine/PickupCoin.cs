using System;
using GXPEngine;

public class PickupCoin : AnimationSprite
{
    private const float MIN_SPD = 1.2f;
    private const float MAX_SPD = 3.6f;
    private float speed = Utils.Random(MIN_SPD, MAX_SPD);
    public PickupCoin() : base("point_16x16.png", 1, 1)
    {
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        //Move(-speed, 0);
    }
}
