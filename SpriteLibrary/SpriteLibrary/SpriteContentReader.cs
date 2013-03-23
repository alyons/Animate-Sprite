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
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace SpriteLibrary
{
    public class SpriteContentReader : ContentTypeReader<Sprite>
    {
        protected override Sprite Read(ContentReader input, Sprite existingInstance)
        {
            Sprite sprite = new Sprite();

            sprite.Name = input.ReadString();
            sprite.Position = input.ReadVector2();
            sprite.FPS = input.ReadInt32();
            sprite.TextureAsset = input.ReadString();
            sprite.RectangeAsset = input.ReadString();

            sprite.Load(input.ContentManager);

            return sprite;
        }
    }
}
