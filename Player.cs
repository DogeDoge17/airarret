using airarreT;
using airarreT.Collision;
using airarreT.Items;
using airarreT.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT
{
    public class Player : Loop
    {
        public Vector2 position = new Vector2();

        public int width = 18;
        public int height = 42;

        // public Vector2 velocity = new Vector2();
        public Vector2 center = new Vector2();
        public int whoAmI = 0;

        private int selectedTile = 0;
        public string selectedName = "Stone";

        int speed = 90;
        float jumpForce = 1;

        float slowDown = 5f;

        int walk = 90;
        int run = 200;

        float maxSpeed = 150;


        float gravity = 1f;

        SpriteEffects flipped = SpriteEffects.None;

        float switchTimer = 0f;

        private bool goku = false;

        int placeType = 0;

        public DynamicCollider collider;/*= new DynamicCollider();*/
        public TriggerCollider groundCheck;


        private bool pressedMovementKey = false;

        private bool slowingLeft, slowingRight;

        private float dir = 0;

        public bool isGrounded = false;

        public Player()
        {
            center = ToolsNStuff.Center(position, new Vector2(width, height));

            collider = new DynamicCollider(new DynamicRectangle(center, new Vector2(width, height)));
            groundCheck = new TriggerCollider(new Rectangle((int)position.X, (int)(position.Y + height), width, 5)); 
        }


        private Rectangle GetMouseGrid()
        {
            return new Rectangle(ToolsNStuff.FloorMultiple(Input.mouseRect.X, 16), ToolsNStuff.FloorMultiple(Input.mouseRect.Y, 16), 16, 16);
        }



        public override void Update()
        {
            pressedMovementKey = false;


            center = ToolsNStuff.Center(position, new Vector2(width, height));

            groundCheck.bounds = new Rectangle((int)position.X, (int)(position.Y + height), width, 5);

            //collider.dynamicRectangle.position = center;

            CollisionManagement.TriggerOnStatic(ref groundCheck);

            isGrounded = groundCheck.hits.stati.Count > 0;

            if (Input.GetKey(Keys.OemMinus))
            {
                Main.camera.Zoom = Math.Clamp(Main.camera.Zoom - 1 * Time.gameDelta, Main.camera.MinimumZoom, Main.camera.MaximumZoom);
            }
            if (Input.GetKey(Keys.OemPlus))
            {
                Main.camera.Zoom = Math.Clamp(Main.camera.Zoom + 1 * Time.gameDelta, Main.camera.MinimumZoom, Main.camera.MaximumZoom);
            }

            if (Input.GetKey(Keys.LeftShift))
            {
                speed = run;
            }
            else
            {
                speed = walk;
            }

            switchTimer -= 1 * Time.gameDelta;

            if (Input.GetKeyDown(Keys.G))
                goku = !goku;

            if (switchTimer <= 0)
            {
                if (Input.GetScrollWheel() == 1)
                {
                    if (selectedTile < 4)
                        selectedTile++;
                    else
                        selectedTile = 0;
                    switchTimer = 0.05f;
                }
                else if (Input.GetScrollWheel() == -1)
                {
                    if (selectedTile != 0)
                        selectedTile--;
                    else
                        selectedTile = 4;
                    switchTimer = 0.05f;
                }
            }



            if (Input.GetKeyDown(Keys.R))
            {
                position = Vector2.Zero;

            }

            if (Input.GetKeyDown(Keys.Space) && isGrounded)
            {
                collider.dynamicRectangle.velocity -= Vector2.UnitY * jumpForce * 15000 * Time.gameDelta;
                pressedMovementKey = true;
            }
            if (Input.GetKey(Keys.A))
            {
                // Debug.WriteLine(collider.dynamicRectangle.velocity.X);

                if (dir == 1 && !slowingRight)
                    collider.dynamicRectangle.velocity.X = -20;

                if (collider.dynamicRectangle.velocity.X > 0 || slowingRight)
                {
                    slowingRight = true;
                    //collider.dynamicRectangle.velocity.X = (collider.dynamicRectangle.velocity.X / 4) * -1;//-2 * speed * Time.gameDelta;
                    collider.dynamicRectangle.velocity.X = collider.dynamicRectangle.velocity.X.MoveTowards(0, slowDown * Time.gameDelta);

                }

                // Debug.WriteLine($"R {slowingRight}");

                if (collider.dynamicRectangle.velocity.X < 0)
                    slowingRight = false;
                if (!slowingRight)
                {
                    collider.dynamicRectangle.velocity -= Vector2.UnitX * speed * Time.gameDelta;
                    flipped = SpriteEffects.FlipHorizontally;
                }
                pressedMovementKey = true;
                slowingLeft = false;

                dir = 0;
            }
            if (Input.GetKey(Keys.S))
            {

                collider.dynamicRectangle.velocity += Vector2.UnitY * speed * Time.gameDelta;
                pressedMovementKey = true;
            }
            if (Input.GetKey(Keys.D))
            {
                //  Debug.WriteLine(collider.dynamicRectangle.velocity.X);

                if (dir == 0 && !slowingLeft)
                    collider.dynamicRectangle.velocity.X = 20;

                if (collider.dynamicRectangle.velocity.X < 0 || slowingLeft)
                {
                    slowingLeft = true;
                    //collider.dynamicRectangle.velocity.X = (collider.dynamicRectangle.velocity.X / 4) * -1;// 2 * speed * Time.gameDelta;
                    collider.dynamicRectangle.velocity.X = collider.dynamicRectangle.velocity.X.MoveTowards(0, 10 * slowDown * Time.gameDelta);

                }
                //Debug.WriteLine($"L: {slowingLeft}");

                if (collider.dynamicRectangle.velocity.X > 0)
                    slowingLeft = false;
                if (!slowingRight)
                {
                    collider.dynamicRectangle.velocity += Vector2.UnitX * speed * Time.gameDelta;
                    flipped = SpriteEffects.None;
                }

                dir = 1;

                pressedMovementKey = true;
                slowingRight = false;
            }

            collider.dynamicRectangle.velocity += Vector2.UnitY * gravity * 500 * Time.gameDelta;

            if (!pressedMovementKey)
                collider.dynamicRectangle.velocity.X += collider.dynamicRectangle.velocity.X.MoveTowards(0, slowDown * Time.gameDelta);

            collider.dynamicRectangle.velocity.X = Math.Clamp(collider.dynamicRectangle.velocity.X, maxSpeed * -1, maxSpeed);

            CollisionManagement.CheckCollision(collider);

            //Debug.WriteLine(position + " " + collider.dynamicRectangle.position);

            //position = ToolsNStuff.TopLeft(collider.dynamicRectangle.position, new Vector2(width, height));

            position = collider.dynamicRectangle.position;

            // position = new Vector2(collider.dynamicRectangle.position.X - 4, collider.dynamicRectangle.position.Y + 10);

            if (selectedTile == 0)
                selectedName = "Stone";

            else if (selectedTile == 1)
                selectedName = "Dirt";
            else if (selectedTile == 2)
                selectedName = "Grass";
            else if (selectedTile == 3)
                selectedName = "Plant";
            else if (selectedTile == 4)
                selectedName = "Wood";
            else
                selectedName = "Stone";

            if (Input.currentMouseState.RightButton == ButtonState.Pressed)
            {
                var rec = GetMouseGrid();
                if (!rec.Intersects(collider.dynamicRectangle.ToRectangle()))
                {
                    if (!Main.tiles.PositionArray().Contains(new Vector2(rec.X, rec.Y)))
                    {
                        if (selectedTile == 0)
                            new StoneTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 1)
                            new DirtTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 2)
                            new GrassTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 3)
                            new PlantTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 4)
                            new WoodTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else
                            new StoneTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                    }
                    else if (Main.tiles[Main.tiles.PositionArray().ToList().IndexOf(new Vector2(rec.X, rec.Y))].placeOver)
                    {
                        Main.RemoveTile(Main.tiles[Main.tiles.PositionArray().ToList().IndexOf(new Vector2(rec.X, rec.Y))]);

                        if (selectedTile == 0)
                            new StoneTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 1)
                            new DirtTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 2)
                            new GrassTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 3)
                            new PlantTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else if (selectedTile == 4)
                            new WoodTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                        else
                            new StoneTile() { position = new Vector2(rec.X, rec.Y), checkNeighbors = true, placeCheck = true };
                    }
                }
            }

            position = new Vector2(MathF.Round(position.X, 2), MathF.Round(position.Y, 2));
            center = ToolsNStuff.Center(position, new Vector2(width, height));
            base.Update();
        }

        public override void Draw()
        {
            if (!goku)
                for (int i = 0; i < 13; i++)
                {
                    Main.spriteBatch.Draw(Main.playerTextures["Player_0_" + i], new Vector2(position.X - 10, position.Y - 10), new Rectangle(0, 0, 40, 54), new Color(132, 55, 34), 0, new Vector2(), 1, flipped, 0);
                }
            else
                Main.spriteBatch.Draw(Main.randomTextures["Goku"], new(position.X - 16, position.Y - 55)/*ToolsNStuff.Center(position, new Vector2(50, 150))*/, new Rectangle(0, 0, 250, 509), Color.White, 0, new Vector2(), .2f, flipped, 0);

            //Main.spriteBatch.DrawEllipse(position, new(5), 20, Color.Red, 5);
            //Main.spriteBatch.DrawEllipse(center, new(5), 20, Color.Green, 5);

            base.Draw();
        }
    }


}