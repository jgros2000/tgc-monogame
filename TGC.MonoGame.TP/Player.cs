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
        private Matrix[] boneMatrices = new Matrix[11];
        

        private CustomModel cust;

        public Player(GraphicsDevice graphicsDevice, Effect Effect)
        {
            effect = Effect;

            cust = new CustomModel(graphicsDevice, @"C:\Users\PC\Documents\TGC\mesh_data.bin");
        }

        public void Update(GameTime gameTime, BoundingBox piso)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //_position = _position + world.Forward;
                cust.boneMatrices[0] = cust.boneMatrices[0] * Matrix.CreateTranslation(0,.01f*turret_pitch,0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _position = _position + world.Left;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //_position = _position + world.Backward;
                 cust.boneMatrices[1] = cust.boneMatrices[1] * Matrix.CreateRotationY(.1f * turret_pitch);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cust.boneMatrices[3] = cust.boneMatrices[3] * Matrix.CreateRotationY(.01f*turret_pitch);
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
                turret_pitch = Math.Min(turret_pitch, 1);
            }
            else if (Mouse.GetState().Y < 490)
            {
                turret_pitch -= 0.1f;
                turret_pitch = Math.Max(turret_pitch, -1f);
            }
            Mouse.SetPosition(910, 490);
            world = Matrix.CreateScale(10f) * Matrix.CreateRotationY(yaw) * Matrix.CreateTranslation(_position);
            //boneMatrices[2] = 



        }


        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix projection, Matrix view)
        {
            for (int i = 0; i < 11; i++)
            {
                boneMatrices[i] = cust.inverseMatrices[i] * cust.boneMatrices[i];
            }
            boneMatrices[4] = cust.inverseMatrices[4] * cust.boneMatrices[4]*boneMatrices[3];
            
            effect.Parameters["Bones"].SetValue(boneMatrices);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            effect.Parameters["World"].SetValue(world);
            cust.Draw(graphicsDevice,effect);

        }

    }
}