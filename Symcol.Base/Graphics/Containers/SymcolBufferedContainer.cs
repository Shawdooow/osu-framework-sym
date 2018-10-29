using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
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
