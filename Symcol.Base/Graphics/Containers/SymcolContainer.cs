using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
{
    public class SymcolContainer : Container
    {
        private bool disposed;

        /// <summary>
        /// Delete this fucking object!
        /// </summary>
        public virtual void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            Dispose();
            disposed = true;
        }

        public override bool UpdateSubTree()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (disposed) return false;
            return base.UpdateSubTree();
        }
    }

    public class SymcolContainer<T> : Container<T>
        where T : Drawable
    {
        private bool disposed;

        /// <summary>
        /// Delete this fucking object!
        /// </summary>
        public virtual void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            Dispose();
            disposed = true;
        }

        public override bool UpdateSubTree()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (disposed) return false;
            return base.UpdateSubTree();
        }
    }
}
