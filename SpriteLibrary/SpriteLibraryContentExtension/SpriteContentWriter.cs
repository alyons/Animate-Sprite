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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SpriteLibrary;

namespace SpriteLibraryContentExtension
{
    [ContentTypeWriter]
    public class SpriteContentWriter : ContentTypeWriter<Sprite>
    {
        protected override void Write(ContentWriter output, Sprite value)
        {
            output.Write(value.Name);
            output.Write(value.FPS);
            output.Write(value.TextureAsset);
            output.Write(value.RectangleAsset);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteContentReader).AssemblyQualifiedName;
        }
    }
}
