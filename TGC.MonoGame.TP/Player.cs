using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Player
    {
        public Matrix world = Matrix.Identity;
        public Matrix world2 = Matrix.Identity;
        private float rotation;
        private Model model;
        private Effect effect;
        private Matrix[] _boneTransforms;

        public Player(Model Model, Effect Effect)
        {
            model = Model;
            effect = Effect;
            foreach (var mesh in model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect;
                }
            }
            _boneTransforms = new Matrix[model.Bones.Count];
        }

        public void Update(GameTime gameTime)
        {
            rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            world = Matrix.CreateScale(0.1f);
            world2 = Matrix.CreateRotationX(rotation);
            
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix projection, Matrix view)
        {
            model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

            //Aca hago todos los cambios de mis bones
            _boneTransforms[1] = world2 * _boneTransforms[1];

            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());

            foreach (var mesh in model.Meshes)
            {
                
                foreach (var meshPart in mesh.MeshParts) {
                    effect.Parameters["World"].SetValue(_boneTransforms[mesh.ParentBone.Index] * world);
                    graphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    graphicsDevice.Indices = meshPart.IndexBuffer;
                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            meshPart.VertexOffset,
                            meshPart.StartIndex,
                            meshPart.PrimitiveCount
                        );
                    }
                }
            }
        }

    }
}