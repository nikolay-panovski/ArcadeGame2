using System;
using GXPEngine;

public class PickupHeart : AnimationSprite      // death killer drug or something
{
    private const int MIN_SPD = 3;
    private const int MAX_SPD = 7;
    private int speed = Utils.Random(MIN_SPD, MAX_SPD);
    public PickupHeart() : base("hearts.png", 2, 1)
    {
        SetOrigin(width / 2, height / 2);
        SetFrame(0);
    }

    private void Update()
    {
        Move(-speed, 0);
    }
}
