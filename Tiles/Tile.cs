using airarreT.Collision;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Tiles
{
    public abstract class Tile : Loop
    {
        /// <summary>
        /// The main texture of the tile
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// The power of the tool the tile can withstand
        /// </summary>
        public static float strength = 0;

        /// <summary>
        /// The rectangle the texture will displayed in
        /// </summary>
        public Rectangle sourceRec = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// The position of the tile
        /// </summary>
        public Vector2 position = new Vector2(0, 0);

        /// <summary>
        /// The position of the tile
        /// </summary>
        public int depth = 0;



        /// <summary>
        /// The id of the tile
        /// </summary>
        public int id = 0;

        /// <summary>
        /// The position of the tile
        /// </summary>
        public Rectangle tileRect = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Remove the the hitbox of the tile
        /// </summary>
        public bool passthrough = false;

        /// <summary>
        /// Breaks easily
        /// </summary>
        public bool weakTile = false;

        /// <summary>
        /// Blocks can be replace this tile
        /// </summary>
        public bool placeOver = false;

        /// <summary>
        /// Blocks can be replace this tile
        /// </summary>
        public bool ignoredNeighbor = false;

        private StringBuilder neighbors = new StringBuilder();
        private int tilePattern = 0;
        private bool mouseIntersect;

        public bool checkNeighbors;
        public bool placeCheck;

        public bool lN, rN, bN, tN;

        public Collider collider;//=> new Collider(new Rectangle((int)position.X, (int)position.Y,16,16));

        public Tile()
        {
            Main.tiles.Add(this);

            //collider = new Collider(new Rectangle((int)position.X, (int)position.Y, 16, 16));

            SetDefaults();
        }

        public Tile(Vector2 vector2)
        {
            position = vector2;

            Main.tiles.Add(this);

            //collider = new Collider(new Rectangle((int)position.X, (int)position.Y, 16, 16));


            SetDefaults();
        }

        public Tile(Vector2 vector2 = new Vector2(), bool checkNeighbors = false)
        {
            position = vector2;

            Main.tiles.Add(this);

            //collider = new Collider(new Rectangle((int)position.X, (int)position.Y, 16, 16));


            SetDefaults();
        }

        public virtual void SetDefaults()
        {
            texture = Main.tileTextures[id];
        }

        public void ForceNeighborCheck()
        {
            if (Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y)) != -1)
            {
                Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y))].checkNeighbors = true;
                // neighbors.Append("Left");
            }
            //check for right
            if (Main.tiles.PositionArray().Contains(new Vector2(position.X + 16, position.Y)))
            {
                Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X + 16, position.Y))].checkNeighbors = true;

                // neighbors.Append("Right");
            }
            //check for bottom
            if (Main.tiles.PositionArray().Contains(new Vector2(position.X, position.Y - 16)))
            {
                Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y - 16))].checkNeighbors = true;
            }
            //check for top
            if (Main.tiles.PositionArray().Contains(new Vector2(position.X, position.Y + 16)))
            {
                Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y + 16))].checkNeighbors = true;
            }
        }

        bool didTheHitBox = false;

        public override void Update()
        {
            if (!didTheHitBox)
            {
                CollisionManagement.RemoveStatic(collider);
                if (!passthrough)
                    collider = new Collider(new Rectangle((int)position.X, (int)position.Y, 16, 16));
                didTheHitBox = true;
            }




            tileRect = new Rectangle((int)position.X, (int)position.Y, 16, 16);

            mouseIntersect = Input.mouseRect.Intersects(tileRect);

            if (mouseIntersect)
            {
                if (Input.currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    Main.RemoveTile(this);
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
                if (Main.tiles.PositionArray().Contains(new Vector2(position.X - 16, position.Y)))
                {
                    if (!Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X - 16, position.Y))].ignoredNeighbor)
                    { neighbors.Append("Left"); lN = true; }
                }
                //check for right
                if (Main.tiles.PositionArray().Contains(new Vector2(position.X + 16, position.Y)))
                {
                    if (!Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X + 16, position.Y))].ignoredNeighbor)
                    { neighbors.Append("Right"); rN = true; }
                }
                //check for bottom
                if (Main.tiles.PositionArray().Contains(new Vector2(position.X, position.Y - 16)))
                {
                    if (!Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y - 16))].ignoredNeighbor)
                    { neighbors.Append("Bottom"); tN = true; }
                }
                //check for top
                if (Main.tiles.PositionArray().Contains(new Vector2(position.X, position.Y + 16)))
                {
                    if (!Main.tiles[Main.tiles.PositionArray().ToList<Vector2>().IndexOf(new Vector2(position.X, position.Y + 16))].ignoredNeighbor)
                    { neighbors.Append("Top"); bN = true; }
                }

                ChangeSource();

                checkNeighbors = false;
            }
        }

        public virtual void OnDestroy()
        {
            CollisionManagement.RemoveStatic(collider);
            ForceNeighborCheck();
        }

        public override void Draw()
        {
            if (mouseIntersect)
                Main.spriteBatch.Draw(Main.randomTextures["Tiles_144"], position, new Rectangle(0, 0, 16, 16), Color.White);
        }

        public virtual void ChangeSource()
        {
            tilePattern = Main.rand.Next(0, 2);
            #region One Neighbor
            if (neighbors.ToString() == "")
            {
                switch (tilePattern)
                {
                    case 0:
                        sourceRec = new Rectangle(162, 54, 16, 16);
                        break;
                    case 1:
                        sourceRec = new Rectangle(180, 54, 16, 16);
                        break;
                    case 2:
                        sourceRec = new Rectangle(198, 54, 16, 16);
                        break;
                }
                return;
            }
            #endregion

            #region One Neighbor
            if (neighbors.ToString() == "Left")
            {
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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
                switch (tilePattern)
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

    public static class TileID
    {
        public static int Dirt = 0;
        public static DirtTile DirtT;

    }

}


public static class TileTools
{
    public static Vector2[] PositionArray(this List<Tile> tiles)
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < tiles.Count; i++)
        {
            positions.Add(tiles[i].position);


        }

        return positions.ToArray();
    }
}