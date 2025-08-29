using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    
    public class Hitbox
    {
        private Effect effect;
        private Matrix world;
        private VertexBuffer VertexBuffer;
        private IndexBuffer _indices;
        private VertexPosition[] _vertices = new VertexPosition[8];

        public Hitbox(GraphicsDevice graphicsDevice, Matrix World, Effect Effect)
        {
            effect = Effect;
            world = World;
            short[] triangleIndices = {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7
            };
            _indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 24, BufferUsage.None);
            _indices.SetData(triangleIndices);

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPosition), 8, BufferUsage.WriteOnly);
            Vector3[] verts = {
                new(1, -1, 1), new(-1, -1, 1), new(-1, -1, -1), new(1, -1, -1),
                new(1, 1, 1), new(-1, 1, 1), new(-1, 1, -1), new(1, 1, -1)
            };
            for (int i = 0; i < 8; i++) _vertices[i] = new VertexPosition(verts[i]);
            VertexBuffer.SetData(_vertices);
        }


        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = _indices;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 12);
            }
        }

    }
}