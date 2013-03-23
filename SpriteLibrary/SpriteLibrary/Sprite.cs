/// Copyright Alexander Lyons 2011, 2013
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

namespace SpriteLibrary
{
    public class Sprite
    {
        #region Variables
        int frame = 0;
        int delay = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Name refers to the name given to this specific sprite.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Position refers to the position on the screen at which the sprite will be rendered.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// FPS (short for frames per second) refers to the rate at which the sprite changes image.
        /// </summary>
        public int FPS { get; set; }
        /// <summary>
        /// TextureAsset refers to which texture assest (by relative path) is used to render the sprite.
        /// </summary>
        public String TextureAsset { get; set; }
        /// <summary>
        /// RectangleAsset refers to which xml asset (by relative path) is used to render the sprite.
        /// </summary>
        public String RectangeAsset { get; set; }

        /// <summary>
        /// This is the texture used to pull image data from.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture { get; set; }
        /// <summary>
        /// This is the list of rectangles used to pull data from Texture.
        /// </summary>
        [ContentSerializerIgnore]
        public List<Rectangle> Rectangles { get; set; }
        #endregion

        #region Events
        public event SpriteEventHandler EndOfAnimation;
        #endregion

        #region Delegates
        public delegate void SpriteEventHandler(object sender, SpriteEventArgs e);
        #endregion

        #region Constructors
        public Sprite()
        {
            Rectangles = new List<Rectangle>();
        }
        #endregion

        #region Methods
        public void Load(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TextureAsset);
            StringToRectangle(content.Load<List<string>>(RectangeAsset));
        }

        public void Update(GameTime gameTime)
        {
            delay -= gameTime.ElapsedGameTime.Milliseconds;

            if (delay <= 0)
            {
                frame++;
                if (frame >= Rectangles.Count)
                {
                    frame = 0;
                    if (EndOfAnimation != null) EndOfAnimation(this, new SpriteEventArgs());
                }
                delay += 1000 / FPS;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], color);
        }
        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(Texture, destinationRectangle, Rectangles[frame], color, rotation, origin, effects, layerDepth);
        }
        public void Draw(SpriteBatch spriteBatch, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], color, rotation, origin, scale, effects, layerDepth);
        }
        public void Draw(SpriteBatch spriteBatch, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], color, rotation, origin, scale, effects, layerDepth);
        }

        private void StringToRectangle(List<string> strings)
        {
            foreach (string s in strings)
            {
                string[] tempsS = s.Split(' ');
                int[] tempI = new int[tempsS.Length];
                for (int i = 0; i < tempsS.Length; i++) tempI[i] = Convert.ToInt32(tempsS[i]);
                Rectangles.Add(new Rectangle(tempI[0], tempI[1], tempI[2], tempI[3]));
            }
        }

        public bool[,] GetOpaqueData()
        {
            return GetOpaqueData(255);
        }
        public bool[,] GetOpaqueData(int alpha)
        {
            int x = Rectangles[frame].X;
            int y = Rectangles[frame].Y;
            int h = Rectangles[frame].Height;
            int w = Rectangles[frame].Width;
            bool[,] data = new bool[w, h];
            Color[] pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(pixels);

            for (int i = x; i < x + w; i++)
                for (int j = y; j < y + h; j++)
                    data[i - x, j - y] = (pixels[i + j * Texture.Width].A >= alpha);

            return data;
        }
        public bool[,] GetOpaqueData(Color toIgnore)
        {
            int x = Rectangles[frame].X;
            int y = Rectangles[frame].Y;
            int h = Rectangles[frame].Height;
            int w = Rectangles[frame].Width;
            bool[,] data = new bool[w, h];
            Color[] pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(pixels);

            for (int i = x; i < x + w; i++)
                for (int j = y; j < y + h; j++)
                    data[i - x, j - y] = !(pixels[i + j * Texture.Width].Equals(toIgnore));

            return data;
        }

        public Rectangle GetSize()
        {
            return Rectangles[frame];
        }
        #endregion
    }
}
