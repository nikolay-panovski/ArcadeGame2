using System;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

// used for freeroam debug movement (without gravity),
// and testing Collider/BoxCollider in case they allow me to scrap MoveUntilCollision
public class DebugPlayer2 : AnimationSprite
{
    //private Collision coll_info;
    //public BoxCollider coll_handle { get; set; }
    //private bool is_colliding;
    //public HumanPlayer player1_ref { get; set; }

    public DebugPlayer2(string filename, int columns, int rows, TiledObject obj) : base(filename, columns, rows)
    {
        //player_id = obj.GetIntProperty("player_id");
        //coll_handle = new BoxCollider(this);
    }

    /*private void movementHandle()       // this somehow works better than the other one for this class
    {
        if (Input.GetKey(Key.LEFT)) coll_info = MoveUntilCollision(-1, 0);
        if (Input.GetKey(Key.RIGHT)) coll_info = MoveUntilCollision(1, 0);
        if (Input.GetKey(Key.UP)) coll_info = MoveUntilCollision(0, -1);
        if (Input.GetKey(Key.DOWN)) coll_info = MoveUntilCollision(0, 1);
    }*/
    /*private void movementHandle()
    {
        //if (is_colliding == false)
        //{
            if (Input.GetKey(Key.LEFT)) Move(-1, 0);
            if (Input.GetKey(Key.RIGHT)) Move(1, 0);
            if (Input.GetKey(Key.UP)) Move(0, -1);
            if (Input.GetKey(Key.DOWN)) Move(0, 1);
       // }
    }*/

    /*private void OnCollision(GameObject other)
    {
        //Console.WriteLine("wow collision!");
        if (Input.GetKey(Key.LEFT)) Move(1, 0);     // wow hack works
        if (Input.GetKey(Key.RIGHT)) Move(-1, 0);
        if (Input.GetKey(Key.UP)) Move(0, 1);
        if (Input.GetKey(Key.DOWN)) Move(0, -1);
    }*/

    private void Update()
    {
        //movementHandle();
        //if (coll_info != null) Console.WriteLine("wow collision!");
    }
}

