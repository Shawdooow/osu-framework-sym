using osu.Framework.Graphics;
using Symcol.Base.Graphics.Containers;

namespace Symcol._3D
{
    public class _3DContainer : SymcolContainer
    {
        protected override DrawNode CreateDrawNode() => new _3DDrawNode();

        protected override void ApplyDrawNode(DrawNode node)
        {
            base.ApplyDrawNode(node);
        }
    }
}
