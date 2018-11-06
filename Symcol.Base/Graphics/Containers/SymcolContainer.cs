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
            Dispose();
        }

        public override bool UpdateSubTree()
        {
            if (IsDisposed) return false;
            try
            {
                return base.UpdateSubTree();
            }
            catch
            {
                return false;
            }
        }
    }

    public class SymcolContainer<T> : Container<T>
        where T : Drawable
    {
        public virtual void Delete()
        {
            Clear();
            ClearTransforms();
            Dispose();
        }

        public override bool UpdateSubTree()
        {
            if (IsDisposed) return false;
            try
            {
                return base.UpdateSubTree();
            }
            catch
            {
                return false;
            }
        }
    }
}
