using System.Windows;

namespace CurveLineEditDemo
{
    internal static class SecondDerivativeHelper
    {
        /// <summary>
        /// 获取二阶导数列表
        /// 边界上二阶导数为零。
        /// </summary>
        public static double[] GetSecondDerivatives(List<Point> orderedPoints)
        {
            int n = orderedPoints.Count;
            var secondDerivatives = new double[n];
            double[] tempStorage = new double[n];
            // 起始导数为0
            secondDerivatives[0] = tempStorage[0] = 0.0;

            // 这是三对角算法的分解循环。
            // M_y2和x用于分解因子的临时存储。
            for(int i = 1; i < n - 1; i++)
            {
                double sig = (orderedPoints[i].X - orderedPoints[i - 1].X) / (orderedPoints[i + 1].X - orderedPoints[i - 1].X);
                double entropy = sig * secondDerivatives[i - 1] + 2.0;
                secondDerivatives[i] = (sig - 1.0) / entropy;
                //斜率差
                var slopeDifference = (orderedPoints[i + 1].Y - orderedPoints[i].Y) / (orderedPoints[i + 1].X - orderedPoints[i].X)
                                      - (orderedPoints[i].Y - orderedPoints[i - 1].Y) / (orderedPoints[i].X - orderedPoints[i - 1].X);
                tempStorage[i] = (6.0 * slopeDifference / (orderedPoints[i + 1].X - orderedPoints[i - 1].X) - sig * tempStorage[i - 1]) / entropy;
            }

            // 末尾导数为0
            secondDerivatives[n - 1] = 0;

            // 三对角算法的回代循环
            for(int k = n - 2; k >= 0; k--)
                secondDerivatives[k] = secondDerivatives[k] * secondDerivatives[k + 1] + tempStorage[k];

            return secondDerivatives;
        }
    }
}
