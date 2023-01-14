using airarreT.Collision;
using airarreT.Tiles;
using airarreT.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace airarreT
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static List<Loop> loops = new List<Loop>();

        public static Dictionary<int, Texture2D> tileTextures = new Dictionary<int, Texture2D>();
        public static Dictionary<int, Texture2D> wallTextures = new Dictionary<int, Texture2D>();
        public static Dictionary<string, Texture2D> playerTextures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> randomTextures = new Dictionary<string, Texture2D>();

       // public static List<Rectangle> grid = new List<Rectangle>();

        public static List<Tile> tiles = new List<Tile>();
        public static List<Wall> walls = new List<Wall>();
        public static int tileCount = 31;
        public static int wallCount = 4 + 1;

        public static Random rand = new Random();

        public static OrthographicCamera camera;

        List<SpriteFont> fonts = new List<SpriteFont>();

        public static float refreshblocks = 1f;

        public static List<Player> players = new List<Player>();

        public static Player localPlayer;



        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            camera = new OrthographicCamera(viewportAdapter);

            camera.Position = new Vector2(0, 0);
            camera.MinimumZoom = .2f;
        }


        void LoadRandomStuff()
        {
            randomTextures.Add("Tiles_144", Content.Load<Texture2D>("Images\\Misc\\TileOutlines\\Tiles_144"));
            randomTextures.Add("Goku", Content.Load<Texture2D>("Images\\Goku"));
        }

        public static void RemoveTile(Tile tile)
        {
            int id = tiles.IndexOf(tile);

            tiles[id].OnDestroy();

            loops.Remove(tile);
            tiles[id] = null;
            tiles.RemoveAt(id);
        }

        public static void RemoveWall(Wall wall)
        {
            int id = walls.IndexOf(wall);

            tiles[id].OnDestroy();

            loops.Remove(wall);
            walls[id] = null;
            walls.RemoveAt(id);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            players.Add(new());

            localPlayer = players[0];

            for (int i = 0; i < 13; i++)
            {
                //tileTypes[i].texture = //GetType().Name);
                playerTextures.Add("Player_0_" + i, Content.Load<Texture2D>("Images\\Player_0_" + i));
            }

            for (int i = 0; i < tileCount; i++)
            {
                //tileTypes[i].texture = //GetType().Name);
                tileTextures.Add(i, Content.Load<Texture2D>("Images\\Tiles_" + i));
            }


            for (int i = 1; i < wallCount; i++)
            {
                //tileTypes[i].texture = //GetType().Name);
                wallTextures.Add(i, Content.Load<Texture2D>("Images\\Wall_" + i));
            }

            LoadRandomStuff();

            //Debug.WriteLine(wallTextures);

            //new PlantTile() { position = new Vector2(0, 0) };
            //new GrassTile() { position = new Vector2(0, 16) };

            //for (int x = 0; x < 150; x++)
            //{
            //    for (int y = 0; y < 10; y++)
            //    {
            //            new WoodWallWall() { position = new Vector2(x * 16, y * 16) };
            //    }
            //}

            //new WoodWallWall() { position = new Vector2(0 * 16, 0 * 16) };



            for (int x = 0; x < 150; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    if (rand.Next(0, 2) == 1)
                        new PlantTile() { position = new Vector2(x * 16, y * 16), collider = new Collider(x * 16, y * 16, 16, 16) };
                }
            }


            for (int x = 0; x < 150; x++)
            {
                for (int y = 1; y < 2; y++)
                {
                    new GrassTile() { position = new Vector2(x * 16, y * 16), collider = new Collider(x * 16, y * 16, 16, 16) };
                }
            }

            for (int x = 0; x < 150; x++)
            {
                for (int y = 2; y < 5; y++)
                {
                    new DirtTile() { position = new Vector2(x * 16, y * 16), collider = new Collider(x * 16, y * 16, 16, 16) };
                }
            }

            for (int x = 0; x < 150; x++)
            {
                for (int y = 5; y < 31; y++)
                {
                    new StoneTile() { position = new Vector2(x * 16, y * 16), collider = new Collider(x * 16, y * 16, 16, 16) };
                }
            }
            //new DirtTile() { position = new Vector2(1 * 16 + (16 * 12), 1 * 16 + (16 * 6)), collider = new Collider(new Rectangle(1 * 16 + (16 * 12), 1 * 16 + (16 * 6),16,16)) };

            fonts.Add(Content.Load<SpriteFont>("Fonts\\Andy"));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Input.SetState();
            Time.SetValues(gameTime);

            if (Input.GetKeyDown(Keys.Escape))
                Exit();

            if(Input.GetKeyDown(Keys.RightAlt) && Input.GetKeyDown(Keys.Enter))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            if (Input.GetKeyDown(Keys.B))
            {
                CollisionManagement.showHitBoxes = !CollisionManagement.showHitBoxes;
            }

            //Debug.WriteLine(tiles.Count);

            // TODO: Add your update logic here

            base.Update(gameTime);

            /*const*/
            //float movementSpeed = 200 / camera.Zoom;
            //camera.Move(GetMovementDirection() * movementSpeed * Time.deltaTime);

            // Debug.WriteLine("Zoom: " + camera.Zoom + " Min Zoom: " + camera.MinimumZoom + " Max Zoom: " + camera.MaximumZoom + " Scroll: " + Input.GetScrollWheel());
            // camera.Zoom = Math.Clamp(camera.Zoom + Input.GetScrollWheel(), camera.MinimumZoom, camera.MaximumZoom);

            if (refreshblocks > 0)
                refreshblocks -= 1 * Time.gameDelta;
            else
                refreshblocks = 100f;

            for (int i = 0; i < loops.Count; i++)
            {
                loops[i].Update();
            }



            //camera.Origin = camera.Center;
            camera.LookAt(new Vector2(localPlayer.center.X, localPlayer.center.Y - 10));
        }

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            if (Input.GetKey(Keys.S) || Input.GetKey(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (Input.GetKey(Keys.W) || Input.GetKey(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (Input.GetKey(Keys.A) || Input.GetKey(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (Input.GetKey(Keys.D) || Input.GetKey(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }
            return movementDirection;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            var transformMatrix = camera.GetViewMatrix();
           // RenderTarget2D renderTarget2D = new RenderTarget2D(GraphicsDevice, 800, 400);


           // renderTarget2D.

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);

            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i] != null)
                    spriteBatch.Draw(walls[i].texture, walls[i].position, walls[i].sourceRec, Color.White, 0f, new Vector2(16, 16), 1, SpriteEffects.None, walls[i].depth);
            }


            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i] != null)
                    spriteBatch.Draw(tiles[i].texture, tiles[i].position, tiles[i].sourceRec, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, tiles[i].depth);
            }

            for (int i = 0; i < loops.Count; i++)
            {
                loops[i].Draw();
            }

            CollisionManagement.Draw();

            spriteBatch.End();


            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.DrawString(fonts[0], "FPS: " + MathF.Floor(Time.framerate), new Vector2(1, 459), Color.White, 0, new Vector2(0, 1), 0.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(fonts[0], "Tile: " + localPlayer.selectedName, new Vector2(5, 5), Color.White, 0, new Vector2(0, 1), 0.35f, SpriteEffects.None, 0);


            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}