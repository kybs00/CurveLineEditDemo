using System.Windows;

namespace CurveLineEditDemo
{
    /// <summary>
    /// 平滑曲线修正
    /// </summary>
    public static class SmoothAlgorithm
    {
        private const double Tolerance = 0.5;

        /// <summary>
        /// 获取拟合后的点集
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Point> GetFittingLinePoints(List<Point> points)
        {
            var orderedPoints = (from pt in points orderby pt.X select pt).ToList();
            var secondDerivatives = SecondDerivativeHelper.GetSecondDerivatives(orderedPoints);
            List<Point> polyLinePoints = PointFakeApproximationHelper.GetSplinePolyLineApproximation(orderedPoints, secondDerivatives, Tolerance);
            return polyLinePoints;
        }
    }
}
