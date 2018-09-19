using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
{
    public class SymcolContainer : SymcolContainer<Drawable>
    {
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
        }

        protected override void Dispose(bool isDisposing)
        {
            disposed = true;
            base.Dispose(isDisposing);
        }

        public override bool UpdateSubTree()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (disposed) return false;
            return base.UpdateSubTree();
        }
    }
}
