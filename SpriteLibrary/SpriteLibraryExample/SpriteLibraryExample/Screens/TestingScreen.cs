using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using SpriteLibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpriteLibraryExample.Screens
{
    public class TestingScreen : GameScreen
    {
        #region Variables
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
        #endregion

        #region Initialization
        public TestingScreen()
        {
            pointsOfCollision = new List<Vector2>();
        }
        #endregion

        #region Methods
        public void Load(ContentManager Content)
        {
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
        }

        public override void Unload()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Dispose();
            }
        }
        #endregion

        #region Event Handler Methods
        void s_EndOfAnimation(object sender, SpriteEventArgs e)
        {
            loops += 1;
        }
        #endregion
    }
}
