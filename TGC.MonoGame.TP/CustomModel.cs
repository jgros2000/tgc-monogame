using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class CustomModel
    {
        private List<CustomPart> parts = new List<CustomPart>();

        public CustomModel(GraphicsDevice graphicsDevice)
        {
            var triangleVertices = new VertexSkinned[]
                    {
                        new VertexSkinned(new Vector3(1f, 1f, 1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(1f, 1f, -1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(1f, -1f, 1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(1f, -1f, -1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(-1f, 1f, 1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(-1f, 1f, -1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,1f,0f)),
                        new VertexSkinned(new Vector3(-1f, -1f, 1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),
                        new VertexSkinned(new Vector3(-1f, -1f, -1f), new Byte4(0, 1, 2, 3),new Vector4(0,0,0f,1f)),

                    };
            // Array of indices
            var triangleIndices = new ushort[]
                    {
                        0, 1, 2,1,2,3,0,4,5,0,5,1,4,5,6,6,5,7,6,7,2,7,2,3,3,7,1,7,5,1,4,2,0,6,4,2,
                    };
            parts.Add(new CustomPart(graphicsDevice, triangleVertices, triangleIndices,12));
        }


        public void Draw(GraphicsDevice graphicsDevice, Effect effect)
        {
            foreach (var part in parts)
            {
                part.Draw(graphicsDevice, effect);
            }
        }

    }
}