using System;
using System.Data;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public struct VertexSkinned : IVertexType
    {
        public Vector3 Position;
        public Byte4 BoneIndices;
        public Vector4 BoneWeights;

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Byte4, VertexElementUsage.BlendIndices, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0)
        );
        public VertexSkinned(Vector3 position, Byte4 boneIndices, Vector4 boneWeights)
        {
            Position = position;
            BoneIndices = boneIndices;
            BoneWeights = boneWeights;
        }

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }

}