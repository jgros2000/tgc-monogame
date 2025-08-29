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
        public Vector3 _rotation = Vector3.Forward;
        private float yaw;
        private Model model;
        private Effect effect;
        private Matrix[] _boneTransforms;
        private Ray piernaD;
        private Ray piernaI;

        public Player(Model Model, Effect Effect)
        {
            model = Model;
            effect = Effect;
            piernaD = new Ray(_position, Vector3.Down);
            piernaI = new Ray(_position, Vector3.Down);
            foreach (var mesh in model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect;
                }
            }
            _boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
        }

        public void Update(GameTime gameTime, BoundingBox piso)
        {
            if (piernaI.Intersects(piso) == null && piernaD.Intersects(piso) == null)
            {
                _position -= Vector3.UnitY;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _position = _position + world.Forward * 5;
                //Walk();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _position = _position + world.Left * 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _position = _position + world.Backward * 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _position = _position + world.Right * 5;
            }
            if (Mouse.GetState().X > 910)
            {
                yaw -= 0.01f;
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(-0.01f));
            }
            else if (Mouse.GetState().X < 910)
            {
                yaw += 0.01f;
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(0.01f));
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

            piernaD.Position = (_boneTransforms[10] * world).Translation;
            piernaI.Position = (_boneTransforms[11] * world).Translation;
            
            
        }

        private void Walk()
        {
            _boneTransforms[10] = Matrix.CreateRotationX(0.02f) * _boneTransforms[10];
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix projection, Matrix view)
        {
            

            //Aca hago todos los cambios de mis bones
            //_boneTransforms[1] = world * _boneTransforms[1];

            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());

            foreach (var mesh in model.Meshes)
            {

                foreach (var meshPart in mesh.MeshParts)
                {
                    Console.WriteLine(mesh.Name);
                    Console.WriteLine(mesh.ParentBone.Index);
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