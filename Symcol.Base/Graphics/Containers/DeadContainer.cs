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
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;
    }
}
