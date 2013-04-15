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
using System.Diagnostics;

namespace SpriteLibrary
{
    public class Sprite
    {
        #region Variables
        int frame = 0;
        int delay = 0;
        Matrix transform;
        Vector2 position;
        float rotation;
        float scale;
        List<bool[,]> opaqueData;
        #endregion

        #region Properties
        /// <summary>
        /// Name refers to the name given to this specific sprite.
        /// </summary>
        public String Name { get; set; }
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
        public String RectangleAsset { get; set; }

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
        /// <summary>
        /// Position refers to the position on the screen at which the sprite will be rendered.
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if(!(position.Equals(value)))
                {
                    position = value;
                    UpdateTransform();
                }
            }
        }
        /// <summary>
        /// Rotation referes to how the sprite is being rotated.
        /// </summary>
        [ContentSerializerIgnore]
        public float Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    while (rotation > 2.0f * Math.PI) rotation -= (float)(2.0f * Math.PI);
                    while (rotation < -(2.0f * Math.PI)) rotation += (float)(2.0f * Math.PI);
                    UpdateTransform();
                }
            }
        }
        /// <summary>
        /// Scale refers to how the sprite is being scaled.
        /// </summary>
        [ContentSerializerIgnore]
        public float Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    scale = value;
                    UpdateTransform();
                }
            }
        }
        /// <summary>
        /// Origin specifies the center of the sprite at the current frame of animation
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 Origin
        {
            get { return new Vector2(Rectangles[frame].Width / 2.0f, Rectangles[frame].Height / 2.0f); }
        }
        /// <summary>
        /// Transforms refers to the transformed qualities of the sprite.
        /// </summary>
        [ContentSerializerIgnore]
        Matrix Transform
        {
            get 
            {
                if (transform == null) UpdateTransform();
                return transform; 
            }
        }
        /// <summary>
        /// Referes to the bounds set by the rectange after the sprite has been rotated
        /// </summary>
        [ContentSerializerIgnore]
        public Rectangle Bounds
        {
            get
            {
                Vector2 topLft = new Vector2(Rectangles[frame].Left, Rectangles[frame].Top);
                Vector2 botLft = new Vector2(Rectangles[frame].Left, Rectangles[frame].Bottom);
                Vector2 topRgt = new Vector2(Rectangles[frame].Right, Rectangles[frame].Top);
                Vector2 botRgt = new Vector2(Rectangles[frame].Right, Rectangles[frame].Bottom);

                Vector2.Transform(ref topLft, ref transform, out topLft);
                Vector2.Transform(ref botLft, ref transform, out botLft);
                Vector2.Transform(ref topRgt, ref transform, out topRgt);
                Vector2.Transform(ref botRgt, ref transform, out botRgt);

                Vector2 min = Vector2.Min(Vector2.Min(topLft, topRgt), Vector2.Min(botLft, botRgt));
                Vector2 max = Vector2.Max(Vector2.Max(topLft, topRgt), Vector2.Max(botLft, botRgt));


                return new Rectangle((int)(Position.X - ((max.X - min.X) / 2.0f)), (int)(Position.Y - ((max.Y - min.Y) / 2.0f)), (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }
        /// <summary>
        /// Returns the rectange which is being used in the current frame.
        /// </summary>
        [ContentSerializerIgnore]
        public Rectangle CurrentRectangle
        {
            get { return Rectangles[frame]; }
        }
        /// <summary>
        /// Gets a 2D boolean array which states which pixels in the image are opaqe.
        /// </summary>
        [ContentSerializerIgnore]
        public bool[,] OpaqueData
        {
            get 
            {
                if (opaqueData.Count != Rectangles.Count) BuildOpaqueData();
                return opaqueData[frame]; 
            }
        }
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
            opaqueData = new List<bool[,]>();
            Position = Vector2.Zero;
            Scale = 1.0f;
            Rotation = 0.0f;
        }
        #endregion

        #region Methods
        public void Load(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TextureAsset);
            StringToRectangle(content.Load<List<string>>(RectangleAsset));
        }

        public void Update(GameTime gameTime)
        {
            if (FPS > 0)
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, Position, Rectangles[frame], color, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
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

        public void BuildOpaqueData()
        {
            BuildOpaqueData(255);
        }
        public void BuildOpaqueData(int alpha)
        {
            opaqueData.Clear();

            for (int q = 0; q < Rectangles.Count; q++)
            {
                int x = Rectangles[q].X;
                int y = Rectangles[q].Y;
                int h = Rectangles[q].Height;
                int w = Rectangles[q].Width;
                bool[,] data = new bool[w, h];
                Color[] pixels = new Color[Texture.Width * Texture.Height];
                Texture.GetData<Color>(pixels);

                for (int i = x; i < x + w; i++)
                    for (int j = y; j < y + h; j++)
                        data[i - x, j - y] = (pixels[i + j * Texture.Width].A >= alpha);

                opaqueData.Add(data);
            }
        }
        public void BuildOpaqueData(Color toTest)
        {
            BuildOpaqueData(toTest, true);
        }
        public void BuildOpaqueData(List<Color> toTest)
        {
            BuildOpaqueData(toTest, true);
        }
        public void BuildOpaqueData(Color toTest, bool isIgnore)
        {
            opaqueData.Clear();

            for (int q = 0; q < Rectangles.Count; q++)
            {
                int x = Rectangles[q].X;
                int y = Rectangles[q].Y;
                int h = Rectangles[q].Height;
                int w = Rectangles[q].Width;
                bool[,] data = new bool[w, h];
                Color[] pixels = new Color[Texture.Width * Texture.Height];
                Texture.GetData<Color>(pixels);

                for (int i = x; i < x + w; i++)
                    for (int j = y; j < y + h; j++)
                        if (isIgnore)
                        {
                            data[i - x, j - y] = !(pixels[i + j * Texture.Width].Equals(toTest));
                            //Debug.WriteLine(String.Format("Pixel ({0},{1}): {2}", (i - x), (j - y), pixels[i + j * Texture.Width].ToString()));
                        }
                        else
                        {
                            data[i - x, j - y] = (pixels[i + j * Texture.Width].Equals(toTest));
                        }

                opaqueData.Add(data);
            }
        }
        public void BuildOpaqueData(List<Color> toTest, bool isIgnore)
        {
            opaqueData.Clear();

            for (int q = 0; q < Rectangles.Count; q++)
            {
                int x = Rectangles[q].X;
                int y = Rectangles[q].Y;
                int h = Rectangles[q].Height;
                int w = Rectangles[q].Width;
                bool[,] data = new bool[w, h];
                Color[] pixels = new Color[Texture.Width * Texture.Height];
                Texture.GetData<Color>(pixels);

                for (int i = x; i < x + w; i++)
                    for (int j = y; j < y + h; j++)
                        if (isIgnore)
                            data[i - x, j - y] = !toTest.Exists(c => c.Equals(pixels[i + j * Texture.Width]));
                        else
                            data[i - x, j - y] = toTest.Exists(c => c.Equals(pixels[i + j * Texture.Width]));

                opaqueData.Add(data);
            }
        }

        public bool Collide(Sprite other)
        {
            if (Bounds.Intersects(other.Bounds))
            {
                Matrix transformAToB = Transform * Matrix.Invert(other.Transform);

                Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
                Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

                Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

                for (int yA = 0; yA < CurrentRectangle.Height; yA++)
                {
                    Vector2 posInB = yPosInB;

                    for (int xA = 0; xA < CurrentRectangle.Width; xA++)
                    {
                        int xB = (int)Math.Round(posInB.X);
                        int yB = (int)Math.Round(posInB.Y);

                        if (0 <= xB && xB < other.CurrentRectangle.Width && 0 <= yB && yB < other.CurrentRectangle.Height)
                        {
                            bool[,] thisData = OpaqueData;
                            bool[,] thatData = other.OpaqueData;

                            if (thisData[xA, yA] && thatData[xB, yB]) return true;
                        }

                        posInB += stepX;
                    }

                    yPosInB += stepY;
                }
            }

            return false;
        }
        public List<Vector2> PointsOfCollision(Sprite other)
        {
            List<Vector2> output = new List<Vector2>();

            if (Bounds.Intersects(other.Bounds))
            {
                Matrix transformAToB = Transform * Matrix.Invert(other.Transform);

                Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
                Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

                Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

                for (int yA = 0; yA < CurrentRectangle.Height; yA++)
                {
                    Vector2 posInB = yPosInB;

                    for (int xA = 0; xA < CurrentRectangle.Width; xA++)
                    {
                        int xB = (int)Math.Round(posInB.X);
                        int yB = (int)Math.Round(posInB.Y);

                        if (0 <= xB && xB < other.CurrentRectangle.Width && 0 <= yB && yB < other.CurrentRectangle.Height)
                        {
                            bool[,] thisData = OpaqueData;
                            bool[,] thatData = other.OpaqueData;

                            if (thisData[xA, yA] && thatData[xB, yB]) output.Add(new Vector2(xA, yA));
                        }

                        posInB += stepX;
                    }

                    yPosInB += stepY;
                }
            }

            return output;
        }

        private void UpdateTransform()
        {
            if (Rectangles.Count > 0)
            {
                transform = Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) * Matrix.CreateScale(Scale) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            }
        }
        #endregion
    }
}
