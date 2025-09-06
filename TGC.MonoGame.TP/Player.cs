using System;
using System.Collections.Generic;
using System.Data;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Player
    {
        public Matrix world = Matrix.Identity;
        public Matrix world2 = Matrix.Identity;
        public Matrix world3 = Matrix.Identity;
        public Vector3 _position = Vector3.Zero;
        public Vector3 _rotation = Vector3.Forward;
        private float yaw;
        private float turret_pitch = 0;
        private Effect effect;

        public float t = 0.0f;
        Matrix[] boneMatrices = new Matrix[4];

        private CustomModel cust;

        public Player(GraphicsDevice graphicsDevice, Effect Effect)
        {
            effect = Effect;

            cust = new CustomModel(graphicsDevice);
        }

        public void Update(GameTime gameTime, BoundingBox piso)
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
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(-0.01f));
            }
            else if (Mouse.GetState().X < 910)
            {
                yaw += 0.01f;
                _rotation = Vector3.Transform(_rotation, Matrix.CreateRotationY(0.01f));
            }

            if(Mouse.GetState().Y > 490)
            {
                turret_pitch += 0.1f;
                turret_pitch = Math.Min(turret_pitch, 3f);
            }
            else if (Mouse.GetState().Y < 490)
            {
                turret_pitch -= 0.1f;
                turret_pitch = Math.Max(turret_pitch, 1f);
            }
            Mouse.SetPosition(910, 490);
            world = Matrix.CreateScale(10f) * Matrix.CreateRotationY(yaw) * Matrix.CreateTranslation(_position);
            world2 = Matrix.CreateScale(turret_pitch);



        }


        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix projection, Matrix view)
        {
            for (int i = 0; i < 4; i++)
            {
                boneMatrices[i] = Matrix.Identity;
            }
            boneMatrices[2] = world2;
            effect.Parameters["Bones"].SetValue(boneMatrices);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            effect.Parameters["World"].SetValue(world);
            cust.Draw(graphicsDevice,effect);

        }

    }
}