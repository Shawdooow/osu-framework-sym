using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;

namespace Symcol._3D
{
    public class _3DDrawNode : DrawNode
    {
        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            GLWrapper.SetDepthTest(true);

            //Stuff

            GLWrapper.SetDepthTest(false);
        }
    }
}
