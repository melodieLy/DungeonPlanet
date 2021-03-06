﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace DungeonPlanet.Library
{
    public class WeaponLib
    {
        public float Rotation { get; set; }
        public Vector2 Direction { get; set; }
        public WeaponLib()
        {
        }

        public void Update(float X, float Y)
        {
            Vector2 distance = new Vector2( X, Y);
            Rotation = RotationSet(distance);
            Direction = DirectionSet(Rotation);
        }
        
        public float RotationSet(Vector2 distance)
        {
            return (float)Math.Atan2(distance.Y, distance.X); 
        }
        public Vector2 DirectionSet(float rotation)
        {
             return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)); 
        }

    }
}
