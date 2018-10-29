using osu.Framework.Graphics;

namespace Symcol.Base.Graphics.Containers
{
    /// <summary>
    /// This DeadContainer will not handle input
    /// </summary>
    public class DeadContainer : DeadContainer<Drawable>
    {
    }

    public class DeadContainer<T> : SymcolContainer<T>
        where T : Drawable
    {
        public override bool HandleNonPositionalInput => false;
        public override bool HandlePositionalInput => false;
    }
}
