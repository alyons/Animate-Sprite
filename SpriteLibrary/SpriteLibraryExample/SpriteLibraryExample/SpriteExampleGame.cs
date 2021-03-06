/// Copyright Alexander Lyons 2010, 2011, 2013
///
/// This file is part of SpriteLibrary.
///
/// SpriteLibrary is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
/// 
/// SpriteLibrary is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public License
/// along with SpriteLibrary.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpriteLibrary;

namespace SpriteLibraryExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpriteExampleGame : Microsoft.Xna.Framework.Game
    {
        int minutes = 0;
        int seconds = 0;
        int milliseconds = 0;
        int loops = 0;
        string timeFormat = "{0,-2:D2}:{1,-2:D2}:{2,-3:D3}";
        bool collision = false;
        bool boundsCollide = false;
        string displayFormat = "Time: {0}\nBounds: {1}\nBounds Collide: {2}\nPixels Collide: {3}";
        const float rotationSpeed = 0.005f;
        List<Vector2> pointsOfCollision;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Sprite> sprites;
        bool[,] data;
        SpriteFont font;
        Texture2D shadowPixel;
        Texture2D lightPixel;
        Vector2 baseResolution = new Vector2(1280, 720);
        Vector2 scaledResolution = new Vector2(1280, 720);
        Vector3 scalingFactor = new Vector3();
        Vector2 mousePos = new Vector2();
        bool useLarger = true;

        Effect collideEffect;

        public Matrix View;
        public Matrix Projection;

        public SpriteExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            if (useLarger)
            {
                graphics.PreferredBackBufferHeight = (int)scaledResolution.Y;
                graphics.PreferredBackBufferWidth = (int)scaledResolution.X;
            }
            else
            {
                graphics.PreferredBackBufferHeight = (int)baseResolution.Y;
                graphics.PreferredBackBufferWidth = (int)baseResolution.X;
            }
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            //this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            pointsOfCollision = new List<Vector2>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprites = Content.Load<List<Sprite>>(@"Sprites");
            font = Content.Load<SpriteFont>("font");
            shadowPixel = Content.Load<Texture2D>("BigPixel");
            lightPixel = Content.Load<Texture2D>("BigPixelYellow");

            foreach (Sprite s in sprites)
            {
                if (s.Name.Equals("Sonic Standing"))
                {
                    s.Position = new Vector2(640.0f, 360.0f);
                    s.Scale = 5.0f;
                    s.SpriteEffects |= SpriteEffects.FlipHorizontally;
                }
                if (s.Name.Equals("Chaos Emerald"))
                {
                    s.Scale = 5.0f;
                    s.SpriteEffects |= SpriteEffects.FlipVertically;
                }
                s.EndOfAnimation += new Sprite.SpriteEventHandler(s_EndOfAnimation);
            }

            collideEffect = Content.Load<Effect>("superCollide");

            float aspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            Projection = Matrix.CreatePerspectiveFieldOfView(1.0f, aspectRatio, 1.0f, 100000f);
        }

        void s_EndOfAnimation(object sender, SpriteEventArgs e)
        {
            loops += 1;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            float horScaling = (float)graphics.PreferredBackBufferWidth / baseResolution.X;
            float verScaling = (float)graphics.PreferredBackBufferHeight / baseResolution.Y;
            scalingFactor = new Vector3(horScaling, verScaling, 1);

            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (sprites != null)
            {
                if (sprites.Count > 0)
                {
                    foreach (Sprite s in sprites)
                    {
                        s.Update(gameTime);
                    }
                    //sprites[0].Rotation += rotationSpeed;
                    data = sprites[0].OpaqueData;
                }

                if (sprites.Count > 1)
                {
                    //sprites[1].Rotation -= (float)(2.0 * Math.PI * gameTime.ElapsedGameTime.Milliseconds * 0.001);
                    sprites[1].Position = mousePos;
                    boundsCollide = sprites[0].Bounds.Intersects(sprites[1].Bounds);
                    collision = sprites[0].Collide(sprites[1]);
                    pointsOfCollision.Clear();
                    if (collision)
                    {
                        pointsOfCollision.AddRange(sprites[0].PointsOfCollision(sprites[1]));
                        if (sprites[0].Effect == null) sprites[0].Effect = collideEffect;
                        //sprites[0].Effect.Parameters["Projection"].SetValue(Projection);
                        //sprites[0].Effect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
                    }
                    else
                    {
                        sprites[0].Effect = null;
                    }
                } 
            }

            milliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (milliseconds > 999)
            {
                seconds += 1;
                milliseconds -= 1000;

                if (seconds > 59)
                {
                    minutes += 1;
                    seconds -= 60;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Matrix globalScaling = Matrix.CreateScale(scalingFactor);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //

            if (sprites != null)
            {
                if (sprites.Count > 0)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, globalScaling);

                    spriteBatch.DrawString(font, String.Format(displayFormat, String.Format(timeFormat, minutes, seconds, milliseconds), sprites[0].Bounds, boundsCollide, collision), new Vector2(0, 0), Color.Yellow);

                    spriteBatch.Draw(shadowPixel, sprites[0].Bounds, Color.AntiqueWhite);

                    spriteBatch.End();

                    foreach (Sprite s in sprites)
                    {
                        s.Draw(spriteBatch);
                    }

                    spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, globalScaling);

                    Rectangle size = sprites[0].CurrentRectangle;

                    for (int i = 0; i < size.Width; i++)
                        for (int j = 0; j < size.Height; j++)
                            if (data[i, j])
                                if (pointsOfCollision.Exists(v => v.X == i && v.Y == j))
                                    spriteBatch.Draw(lightPixel, new Vector2(768 + (i * 16), (j * 16)), Color.White);
                                else
                                    spriteBatch.Draw(shadowPixel, new Vector2(768 + (i * 16), (j * 16)), Color.White);

                    spriteBatch.End();
                }
            }

            //

            base.Draw(gameTime);
        }
    }
}
