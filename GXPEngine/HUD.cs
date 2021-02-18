using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;

public class HUD : EasyDraw
{
    private const int FONT_SIZE = 22;
    private EasyDraw text_elements;
    private Sprite static_elements = new Sprite("HUD_sprite_fit.png", false, false);
    PrivateFontCollection custom_font = new PrivateFontCollection();
    FontFamily[] family;
    private string font_name = "";

    public HumanPlayer player1_ref { get; set; }
    public AlienPlayer player2_ref { get; set; }

    // LISTS FOR STACKABLE ICONS
    public List<Sprite> red_hearts = new List<Sprite>();
    public List<Sprite> blue_hearts = new List<Sprite>();
    public List<Sprite> cells = new List<Sprite>();
    private Sprite cooldown_bullet = new Sprite("HUD_cooldownbullet.png", false, false);

    public HUD() : base(MyGame.GAME_WIDTH * 2, MyGame.GAME_HEIGHT * 2, false)
    {
        text_elements = new EasyDraw(this.width, this.height, false);
        scale = 0.5f;
        AddChild(static_elements);
        AddChild(text_elements);
        
        custom_font.AddFontFile("spaceranger.ttf");
        family = custom_font.Families;
        if (family.Length > 0) font_name = family[0].Name;
        text_elements.TextFont(font_name, FONT_SIZE);
    }

    public void SetInitSprites()
    {
        for (int initCount = 0; initCount < player1_ref.HP; initCount++)
        {
            Sprite red_heart = new Sprite("HUD_heart_red.png", false, false);
            red_hearts.Add(red_heart);
            red_heart.x = game.width * 0.15f + (red_hearts.Count - 1) * MyGame.TILE_SIZE * 3;
            red_heart.y = game.height * 0.88f;
            AddChild(red_heart);
        }
        for (int initCount = 0; initCount < player2_ref.HP; initCount++)
        {
            Sprite blue_heart = new Sprite("HUD_heart_blue.png", false, false);
            blue_hearts.Add(blue_heart);
            blue_heart.x = game.width * 0.71f + (blue_hearts.Count - 1) * MyGame.TILE_SIZE * 3;
            blue_heart.y = game.height * 0.88f;
            AddChild(blue_heart);
        }
        cooldown_bullet.SetXY(game.width * 0.74f, game.height * 0.94f);     // please don't actually resize the game ever, thanks :)
        AddChild(cooldown_bullet);
    }

    public void RemoveSpriteFromHuman()
    {
        if (red_hearts.Count > 0)
        {
            RemoveChild(red_hearts[red_hearts.Count - 1]);
            red_hearts.RemoveAt(red_hearts.Count - 1);
        }
    }

    public void RemoveSpriteFromAlien()
    {
        if (blue_hearts.Count > 0)
        {
            RemoveChild(blue_hearts[blue_hearts.Count - 1]);
            blue_hearts.RemoveAt(blue_hearts.Count - 1);
        }
    }

    public void AddCellSprite()
    {
        Sprite newCell = new Sprite("HUD_cell.png", false, false);
        cells.Add(newCell);
        newCell.x = game.width - FONT_SIZE * 4 - (cells.Count - 1) * MyGame.TILE_SIZE * 3;
        newCell.y = FONT_SIZE * 2;
        AddChild(newCell);
    }

    public void RemoveCellSprite()
    {
        if (cells.Count > 0)
        {
            RemoveChild(cells[cells.Count - 1]);
            cells.RemoveAt(cells.Count - 1);
        }
    }

    private void Update()
    {
        text_elements.Clear(Color.FromArgb(0, 0, 0, 0));

        if (parent != null)
        {
            TextSize(FONT_SIZE);
            text_elements.Text("SCORE: " + (parent as Level).level_score /*+ "M"*/, FONT_SIZE, FONT_SIZE * 4);
            text_elements.Text(player1_ref.ammo.ToString(), game.width * 0.18f, game.height * 0.98f);
            Stroke(Color.Black);    // does not care about size or stroke??
            if (parent.parent != null)
            {
                text_elements.Text((parent.parent as MyGame).total_lives.ToString(),
                                   game.width * 0.48f + FONT_SIZE / 4,
                                   game.height * 0.9f + FONT_SIZE / 4);
            }
        }

        if (player2_ref.bullet_cooldown == 0) cooldown_bullet.alpha = 1;
        else cooldown_bullet.alpha = 0;

        //if ((parent as Level).viewport.x < (parent as Level).level_width - (game as MyGame).width / 4) x += 0.5f;  // increment by the same as viewport
    }
}
