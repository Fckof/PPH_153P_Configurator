using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator
{
    public static class Transform
    {
        public static float ToPercent(float value, float max, float min)
        {
            return (100 * Convert.ToSingle(Math.Round(value * 10) / 10f) / (max - min));
        }
        public static float ToAbsValue(float percent, float max, float min)
        {
            return ((max - min) * percent) / 100f;
        }
    }
}
