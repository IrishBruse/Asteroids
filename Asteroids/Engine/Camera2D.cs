﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Camera2D
    {
        private Matrix transformationMatrix = Matrix.Identity;
        private Matrix inverseMatrix = Matrix.Identity;
        private bool hasChanged;
        private Vector2 position = Vector2.Zero;
        private Vector2 zoom = Vector2.One;
        private Vector2 origin = Vector2.Zero;
        private float rotation;
        public Viewport Viewport;

        /// <summary>
        ///     Creates a new 2D camera instance
        /// </summary>
        /// <param name="width">The width of the viewport</param>
        /// <param name="height">The height of the viewport</param>
        public Camera2D(int width, int height)
        {
            Viewport = new Viewport
            {
                Width = width,
                Height = height
            };
            Origin = new Vector2(width / 2, height / 2);
        }

        /// <summary>
        ///     Updates the values for our transformation matrix and 
        ///     the inverse matrix.  
        /// </summary>
        private void UpdateMatrices()
        {

            //  Create a translation matrix based on the position of the camera
            Matrix positionTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = -(int)Math.Floor(position.X),
                Y = -(int)Math.Floor(position.Y),
                Z = 0
            });

            //  Create a rotation matrix around the Z axis
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

            //  Create a scale matrix based on the zoom
            Matrix scaleMatrix = Matrix.CreateScale(new Vector3()
            {
                X = zoom.X,
                Y = zoom.Y,
                Z = 1
            });

            //  Create a translation matrix based on the origin position of the camera
            Matrix originTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = (int)Math.Floor(origin.X),
                Y = (int)Math.Floor(origin.Y),
                Z = 0
            });

            //  Perform matrix multiplication of all of the above to create our
            //  transformation matrix
            transformationMatrix = positionTranslationMatrix * rotationMatrix * scaleMatrix * originTranslationMatrix;

            //  Get our inverse matrix of the transformation matrix
            inverseMatrix = Matrix.Invert(transformationMatrix);

            //  Since the matrices have now been updated, set that there is no longer a change
            hasChanged = false;

        }

        /// <summary>
        ///     Gets the cameras transformation matrix
        /// </summary>
        public Matrix TransformationMatrix
        {
            get
            {
                //  If a change is detected, update matraces before
                //  returning value
                if (hasChanged)
                {
                    UpdateMatrices();
                }
                return transformationMatrix;
            }
        }

        /// <summary>
        ///     Gets the inverse of the cameras transformation matrix
        /// </summary>
        public Matrix InverseMatrix
        {
            get
            {
                //  If a change is detected, update matraces before
                //  returning value
                if (hasChanged)
                {
                    UpdateMatrices();
                }
                return inverseMatrix;
            }
        }

        /// <summary>
        ///     Gets or Sets the xy-coordinate position of the camera relative
        ///     to the world space of the game
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (position == value) { return; }

                //  Set the position value
                position = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }

        /// <summary>
        ///     Gets or Sets the rotation angle of the camera
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (rotation == value) { return; }

                //  Set the rotation value
                rotation = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }

        /// <summary>
        ///     Gets or Sets the zoom level of the camera
        /// </summary>
        public Vector2 Zoom
        {
            get { return zoom; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (zoom == value) { return; }

                //  Set the zoom value
                zoom = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }

        /// <summary>
        ///     Gets or Sets the origin point of the camera relative to the
        ///     ViewPort
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (origin == value) { return; }

                //  Set the origin value
                origin = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }

        /// <summary>
        ///     Gets or Sets the camera's x-coordinate position relative to the world
        ///     space of the game
        /// </summary>
        public float X
        {
            get { return position.X; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (position.X == value) { return; }

                //  Set the position x value
                position.X = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }

        /// <summary>
        ///     Gets or Sets teh camera's y-coordinate position relative to the world
        ///     space of the game
        /// </summary>
        public float Y
        {
            get { return position.Y; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (position.Y == value) { return; }

                //  Set the position y value
                position.Y = value;

                //  Flag that a change has been made
                hasChanged = true;
            }
        }


        /// <summary>
        ///     Translate the given screen space xy-coordinate position
        ///     to the equivilant world space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in screen space to translate</param>
        /// <returns>
        ///     The xy-coodinate position in world space
        /// </returns>
        public Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, InverseMatrix);
        }

        /// <summary>
        ///     Translates the given world space xy-coordinate position
        ///     to the equivilant screen space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in world space to translate</param>
        /// <returns>
        ///     The xy-coordinate position in screen space
        /// </returns>
        public Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, TransformationMatrix);
        }
    }
}
