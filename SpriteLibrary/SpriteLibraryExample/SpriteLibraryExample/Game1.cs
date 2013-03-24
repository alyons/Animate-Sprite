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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int seconds = 0;
        int milliseconds = 0;
        int loops = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Sprite> sprites;
        bool[,] data;
        SpriteFont font;
        Texture2D shadowPixel;
        Vector2 baseResolution = new Vector2(1280, 720);
        Vector2 scaledResolution = new Vector2(1280, 720);
        Vector3 scalingFactor = new Vector3();
        Vector2 mousePos = new Vector2();
        bool useLarger = true;

        public Game1()
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
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprites = Content.Load<List<Sprite>>(@"Sprites");
            font = Content.Load<SpriteFont>("font");
            shadowPixel = Content.Load<Texture2D>("BigPixel");

            foreach (Sprite s in sprites)
                s.EndOfAnimation += new Sprite.SpriteEventHandler(s_EndOfAnimation);

            // TODO: use this.Content to load your game content here
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

            foreach (Sprite s in sprites)
            {
                s.Update(gameTime);
            }

            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            data = sprites[0].GetOpaqueData();

            milliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (milliseconds > 999)
            {
                seconds += 1;
                milliseconds -= 1000;
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
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, globalScaling);
            foreach (Sprite s in sprites)
            {
                s.Draw(spriteBatch);
            }
            string display = "Mouse Pos: " + (new Vector2(mousePos.X / scalingFactor.X, mousePos.Y / scalingFactor.Y)).ToString();
            display += "\nTime: " + seconds + ":" + milliseconds;
            display += "\nLoops: " + loops;

            spriteBatch.DrawString(font, display, new Vector2(0, 30), Color.Yellow);

            Rectangle size = sprites[0].GetSize();

            for (int i = 0; i < size.Width; i++)
                for (int j = 0; j < size.Height; j++)
                    if (data[i, j])
                        spriteBatch.Draw(shadowPixel, new Vector2(768 + (i * 16), (j * 16)), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
