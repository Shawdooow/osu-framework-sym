﻿using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
{
    public class SymcolCircularContainer : CircularContainer
    {
        public virtual void Delete()
        {
            ClearTransforms();
            ClearInternal();
            Expire();
        }
    }
}
