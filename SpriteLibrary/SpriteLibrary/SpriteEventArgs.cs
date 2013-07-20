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
using System.Text;

namespace SpriteLibrary
{
    public class SpriteEventArgs : EventArgs
    {
        #region Properties
        public int CurrentFrame { get; private set; }
        #endregion

        #region Constructors
        public SpriteEventArgs()
        {

        }

        public SpriteEventArgs(int frame)
        {
            CurrentFrame = frame;
        }
        #endregion
    }
}
