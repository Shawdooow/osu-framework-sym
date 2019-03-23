#region usings

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

#endregion

namespace Sym.Base.Graphics.Containers
{
    /// <summary>
    /// This DeadContainer will not handle input
    /// </summary>
    public class DeadContainer : DeadContainer<Drawable>
    {
    }

    public class DeadContainer<T> : Container<T>
        where T : Drawable
    {
        public override bool HandleNonPositionalInput => false;
        public override bool HandlePositionalInput => false;
    }
}
