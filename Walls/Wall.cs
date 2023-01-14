using airarreT.Tiles;
using airarreT.Walls;
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

namespace airarreT.Walls
{
    public class Wall : Loop
    {
        /// <summary>
        /// The main texture of the wall
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// The power of the tool the wall can withstand
        /// </summary>
        public static float strength = 0;

        /// <summary>
        /// The rectangle the texture will displayed in
        /// </summary>
        public Rectangle sourceRec = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// The position of the wall
        /// </summary>
        public Vector2 position = new Vector2(0, 0);

        /// <summary>
        /// The depth of the wall
        /// </summary>
        public int depth = 0;



        /// <summary>
        /// The id of the wall
        /// </summary>
        public int id = 0;

        /// <summary>
        /// The position of the wall
        /// </summary>
        public Rectangle wallRect = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Remove the the hitbox of the wall
        /// </summary>
        public bool passthrough = false;



        /// <summary>
        /// Blocks can be replace this wall
        /// </summary>
        public bool placeOver = false;

        /// <summary>
        /// Blocks can be replace this wall
        /// </summary>
        public bool ignoredNeighbor = false;

        private StringBuilder neighbors = new StringBuilder();
        private int wallPattern = 0;
        private bool mouseIntersect;

        public bool checkNeighbors;
        public bool placeCheck;

        public bool lN, rN, bN, tN;


        public Wall()
        {
            Main.walls.Add(this);


            SetDefaults();
        }

        public Wall(Vector2 vector2)
        {
            position = vector2;

            Main.walls.Add(this);



            SetDefaults();
        }

        public Wall(Vector2 vector2 = new Vector2(), bool checkNeighbors = false)
        {
            position = vector2;

            Main.walls.Add(this);

            SetDefaults();
        }

        public virtual void SetDefaults()
        {
            texture = Main.wallTextures[id];
        }

        public void ForceNeighborCheck()
        {
            if (Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y)) != -1)
            {
                Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y))].checkNeighbors = true;
                // neighbors.Append("Left");
            }
            //check for right
            if (Main.walls.PositionArray().Contains(new Vector2(position.X + 16, position.Y)))
            {
                Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X + 16, position.Y))].checkNeighbors = true;

                // neighbors.Append("Right");
            }
            //check for bottom
            if (Main.walls.PositionArray().Contains(new Vector2(position.X, position.Y - 16)))
            {
                Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y - 16))].checkNeighbors = true;
            }
            //check for top
            if (Main.walls.PositionArray().Contains(new Vector2(position.X, position.Y + 16)))
            {
                Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y + 16))].checkNeighbors = true;
            }
        }

        public override void Update()
        {
            wallRect = new Rectangle((int)position.X, (int)position.Y, 16, 16);

            mouseIntersect = Input.mouseRect.Intersects(wallRect);

            if (mouseIntersect)
            {
                if (Input.currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    Main.RemoveWall(this);
                    return;
                }
            }

            if (placeCheck)
            {
                ForceNeighborCheck();
                placeCheck = false;
            }

            if (Main.refreshblocks < 0 || checkNeighbors)
            {
                neighbors = new StringBuilder();
                lN = false;
                rN = false;
                bN = false;
                tN = false;

                //check for left
                if (Main.walls.PositionArray().Contains(new Vector2(position.X - 16, position.Y)))
                {
                    if (!Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y))].ignoredNeighbor)
                    { neighbors.Append("Left"); lN = true; }
                }
                //check for right
                if (Main.walls.PositionArray().Contains(new Vector2(position.X + 16, position.Y)))
                {
                    if (!Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X + 16, position.Y))].ignoredNeighbor)
                    { neighbors.Append("Right"); rN = true; }
                }
                //check for bottom
                if (Main.walls.PositionArray().Contains(new Vector2(position.X, position.Y + 16)))
                {
                    if (!Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y - 16))].ignoredNeighbor)
                    { neighbors.Append("Bottom"); tN = true; }
                }
                //check for top
                if (Main.walls.PositionArray().Contains(new Vector2(position.X, position.Y - 16)))
                {
                    if (!Main.walls[Main.walls.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y + 16))].ignoredNeighbor)
                    { neighbors.Append("Top"); bN = true; }
                }

                ChangeSource();

                checkNeighbors = false;
            }
        }

        public virtual void OnDestroy()
        {
            ForceNeighborCheck();
        }

        public override void Draw()
        {
            if (mouseIntersect)
                Main.spriteBatch.Draw(Main.randomTextures["Tiles_144"], position, new Rectangle(0, 0, 16, 16), Color.White);
        }

        public virtual void ChangeSource()
        {
            wallPattern = Main.rand.Next(0, 2);
            #region One Neighbor
            if (neighbors.ToString() == "")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(324, 108, 32, 32);
                        break;
                    case 1:
                        sourceRec = new Rectangle(360, 108, 32, 32);
                        break;
                    case 2:
                        sourceRec = new Rectangle(396, 108, 32, 32);
                        break;
                }
                return;
            }
            #endregion

            #region One Neighbor
            if (neighbors.ToString() == "Left")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(216, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(216, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(216, 36, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "Right")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(162, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(162, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(162, 36, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "Bottom")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(108, 54, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(126, 54, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(144, 54, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "Top")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(108, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(126, 0, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(144, 0, 16, 16);
                        break;
                }
                return;
            }
            #endregion

            #region Two Neighbors
            if (neighbors.ToString() == "LeftRight")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(108, 72, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(126, 72, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(144, 72, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "LeftTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(18, 54, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(54, 54, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(90, 54, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "LeftBottom")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(18, 72, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(54, 72, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(90, 72, 16, 16);
                        break;
                }

                return;
            }
            else if (neighbors.ToString() == "RightTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(0, 54, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(36, 54, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(72, 54, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "RightBottom")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(0, 72, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(36, 72, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(72, 72, 16, 16);
                        break;
                }

                return;
            }
            else if (neighbors.ToString() == "BottomTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(90, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(90, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(90, 36, 16, 16);
                        break;
                }
                return;
            }
            #endregion

            #region 3 Neighbors

            if (neighbors.ToString() == "RightBottomTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(0, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(0, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(0, 36, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "LeftBottomTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(72, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(72, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(72, 36, 16, 16);
                        break;
                }
                return;
            }
            else if (neighbors.ToString() == "LeftRightTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(18, 0, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(36, 0, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(54, 0, 16, 16);
                        break;
                }

                return;
            }
            else if (neighbors.ToString() == "LeftRightBottom")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(18, 36, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(36, 36, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(54, 36, 16, 16);
                        break;
                }
                return;
            }

            #endregion

            #region 4 Neighbors
            if (neighbors.ToString() == "LeftRightBottomTop")
            {
                switch (wallPattern)
                {
                    case 0:
                        sourceRec = new Rectangle(18, 18, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(36, 18, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(54, 18, 16, 16);
                        break;
                }
                return;
            }
            #endregion
        }
    }
}
public static class WallTools
{
    public static Vector2[] PositionArray(this List<Wall> walls)
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < walls.Count; i++)
        {
            positions.Add(walls[i].position);


        }

        return positions.ToArray();
    }
}