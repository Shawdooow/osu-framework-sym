namespace Sym.Base.Extentions
{
    /// <summary>
    /// Additional Math functions
    /// </summary>
    public static class SymcolMath
    {
        /// <summary>
        /// Scales the given value between the inputMin and inputMax to between outputMin and outputMax
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inputMin"></param>
        /// <param name="inputMax"></param>
        /// <param name="outputMin"></param>
        /// <param name="outputMax"></param>
        /// <returns></returns>
        public static float Scale(float value, float inputMin, float inputMax, float outputMin = 0, float outputMax = 1)
        {
            float scale = (outputMax - outputMin) / (inputMin - inputMax);
            return outputMin + (value - inputMax) * scale;
        }

        /// <summary>
        /// Scales the given value between the inputMin and inputMax to between outputMin and outputMax
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inputMin"></param>
        /// <param name="inputMax"></param>
        /// <param name="outputMin"></param>
        /// <param name="outputMax"></param>
        /// <returns></returns>
        public static double Scale(double value, double inputMin, double inputMax, double outputMin = 0, double outputMax = 1)
        { 
            double scale = (outputMax - outputMin) / (inputMin - inputMax);
            return outputMin + (value - inputMax) * scale;
        }
    }
}
