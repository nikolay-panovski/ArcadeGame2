using System;
using GXPEngine;

public class PickupShield : AnimationSprite      // death killer drug or something
{
    private const int MIN_SPD = 5;
    private const int MAX_SPD = 10;
    private int speed = Utils.Random(MIN_SPD, MAX_SPD);
    public PickupShield() : base("circle.png", 1, 1)
    {
        SetOrigin(width / 2, height / 2);
    }

    private void Update()
    {
        Move(-speed, 0);
    }
}
