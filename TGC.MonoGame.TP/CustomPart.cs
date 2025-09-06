using System;
using System.Data;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class CustomPart
    {
        private VertexBuffer _vertices;
        private int _triangles;
        private IndexBuffer _indices;

        public CustomPart(GraphicsDevice graphicsDevice, VertexSkinned[] vertex, ushort[] index, int triangles)
        {
            _vertices = new VertexBuffer(graphicsDevice, VertexSkinned.VertexDeclaration, vertex.Length, BufferUsage.WriteOnly);
            _vertices.SetData(vertex);
            _indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, index.Length, BufferUsage.None);
            _indices.SetData(index);
            _triangles = triangles;
        }


        public void Draw(GraphicsDevice graphicsDevice, Effect effect)
        {
            graphicsDevice.SetVertexBuffer(_vertices);
            graphicsDevice.Indices = _indices;
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    _triangles
                );
            }
        }

    }
}