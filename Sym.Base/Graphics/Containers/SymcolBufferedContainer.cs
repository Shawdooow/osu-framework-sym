using osu.Framework.Graphics.Containers;

namespace Sym.Base.Graphics.Containers
{
    public class SymcolBufferedContainer : BufferedContainer
    {
        public virtual void Delete()
        {
            ClearTransforms();
            ClearInternal();
            Expire();
        }
    }
}
