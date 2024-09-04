using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class Utilidades
    {
        public static bool BuscarArea(Vector3 position, Vector3 size)
        {
            Collider[] colliders = Physics.OverlapBox(position, size / 2);
            if (colliders.Length > 0)
            {
                // hay objetos
                return true;
            }
            else
            {
                // no hay objetos
                return false;
            }
        }
    }
}
