using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
{
    public class SymcolContainer : Container
    {
        public virtual void Delete()
        {
            Clear();
            ClearTransforms();
            Expire();
        }
    }

    public class SymcolContainer<T> : Container<T>
        where T : Drawable
    {
        public virtual void Delete()
        {
            Clear();
            ClearTransforms();
            Expire();
        }
    }
}
