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
        public Vector3 _position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Forward;
        private float yaw;
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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _position = _position + world.Forward;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _position = _position + world.Left;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _position = _position + world.Backward;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _position = _position + world.Right;
            }
            if (Mouse.GetState().X > 910)
            {
                yaw -= 0.01f;
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(0.1f));
            }
            else if (Mouse.GetState().X < 910)
            {
                yaw += 0.01f;
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(-.1f));
            }

            /*if (Mouse.GetState().Y > 490)
            {
                turret_pitch += elapsedTime * 0.1f;
                turret_pitch = Math.Min(turret_pitch, 0.2f);
            }
            else if (Mouse.GetState().Y < 490)
            {
                turret_pitch -= elapsedTime * 0.1f;
                turret_pitch = Math.Max(turret_pitch, -0.2f);
            }*/
            Mouse.SetPosition(910, 490);
            world = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(yaw) * Matrix.CreateTranslation(_position);
            
            
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix projection, Matrix view)
        {
            model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

            //Aca hago todos los cambios de mis bones
            //_boneTransforms[1] = world * _boneTransforms[1];

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