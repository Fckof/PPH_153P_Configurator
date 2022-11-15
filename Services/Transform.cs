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
            return (float)Math.Round((100 * value / max - min),1);
        }
        public static float ToAbsValue(float percent, float max, float min)
        {
            return ((max - min) * percent) / 100f;
        }
        public static float ToFourDigits(float value)
        {
            int lgth = Math.Truncate(Math.Abs(value)).ToString().Length;
            return lgth>4 ? 0 : (float)Math.Round(value, 4-lgth);
        }
    }
}
