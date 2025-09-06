using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    
    public class KeyFrame
    {

        public int BoneIndex;
        public Matrix Transform;


        public KeyFrame(int boneIndex, Matrix transform)
        {
            BoneIndex = boneIndex;
            Transform = Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * transform;
        }

        public Matrix Interpolate(Matrix a, float t)
        {
            Vector3 scaleA, transA;
            Quaternion rotA;
            Transform.Decompose(out scaleA, out rotA, out transA);

            Vector3 scaleB, transB;
            Quaternion rotB;
            a.Decompose(out scaleB, out rotB, out transB);

            Vector3 scale = Vector3.Lerp(scaleA, scaleB, t);
            Quaternion rot = Quaternion.Slerp(rotA, rotB, t);
            Vector3 trans = Vector3.Lerp(transA, transB, t);

            return Matrix.CreateScale(scaleA) * Matrix.CreateFromQuaternion(rot) * Matrix.CreateTranslation(trans);
        }
    }
}