using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        public Matrix[] boneMatrices;
        public Matrix[] inverseMatrices;

        public CustomModel(GraphicsDevice graphicsDevice, String path)
        {
            LoadModel(graphicsDevice, path);
        }

        private void LoadModel(GraphicsDevice graphicsDevice,String path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                uint mesh_quantity = reader.ReadUInt32();
                for (int j = 0; j < mesh_quantity; j++)
                {
                    uint numVertices = reader.ReadUInt32();
                    VertexSkinned[] vertex = new VertexSkinned[numVertices];
                    for (int i = 0; i < numVertices; i++)
                    {
                        uint boneQuantity = reader.ReadUInt32();
                        uint[] bones2 = { 0, 0, 0, 0};
                        float[] weights2 = { 0f, 0f, 0f, 0f };
                        for (int k = 0; k < boneQuantity; k++)
                        {
                            bones2[k] = reader.ReadUInt32();
                            weights2[k] = reader.ReadSingle();
                        }
                        Byte4 bones = new Byte4(bones2[0], bones2[1], bones2[2], bones2[3]);
                        Vector4 weights = new Vector4(weights2[0], weights2[1], weights2[2], weights2[3]);
                        float x = reader.ReadSingle();
                        float y = reader.ReadSingle();
                        float z = reader.ReadSingle();
                        vertex[i] = new VertexSkinned(new Vector3(x, y, z), bones, weights);
                        if (j == 3)
                        {
                            Console.WriteLine(bones);
                            Console.WriteLine(weights);
                        }
                    }
                    int numIndices = (int)reader.ReadUInt32();
                    ushort[] indices = new ushort[numIndices];
                    for (int i = 0; i < numIndices; i++)
                    {
                        indices[i] = reader.ReadUInt16();
                    }
                    parts.Add(new CustomPart(graphicsDevice, vertex, indices, numIndices / 3));
                    
                }
                
                uint numBones = reader.ReadUInt32();
                boneMatrices = new Matrix[numBones];
                inverseMatrices = new Matrix[numBones];
                for (int i = 0; i < numBones; i++)
                {
                    float[] m = new float[16];
                    for (int j = 0; j < 16; j++)
                        m[j] = reader.ReadSingle();

                    inverseMatrices[i] = new Matrix(
                        m[0], m[1], m[2], m[3],
                        m[4], m[5], m[6], m[7],
                        m[8], m[9], m[10], m[11],
                        m[12], m[13], m[14], m[15]
                    );
                    boneMatrices[i] = Matrix.Invert(inverseMatrices[i]);
                }
            } 
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