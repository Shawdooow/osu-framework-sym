using osu.Framework.Graphics.Containers;

namespace Symcol.Base.Graphics.Containers
{
    public class SymcolClickableContainer : ClickableContainer
    {
        public virtual void Delete()
        {
            ClearTransforms();
            ClearInternal();
            Expire();
        }
    }
}
