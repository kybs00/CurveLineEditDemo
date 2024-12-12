using System.Collections.ObjectModel;
using System.Windows;

namespace CurveLineEditDemo
{
    /// <summary>
    /// 数据点近似伪造类
    /// </summary>
    internal static class PointFakeApproximationHelper
    {
        /// <summary>
        /// 用给定公差的多段线近似样条曲线。
        /// </summary>
        /// <param name="spline">The spline.</param>
        /// <param name="tolerance">The tolerance, i.e. the maximum distance from the spline
        ///     to the approximating polyline.</param>
        /// <returns>List of points of the PolyLine approximating the spline
        ///     with the tolerance given.</returns>
        public static List<Point> GetSplinePolyLineApproximation(List<Point> points, double[] secondDerivatives, double tolerance)
        {
            List<Point> fakeExtensionPoints = new List<Point>();
            fakeExtensionPoints.Add(points[0]);

            // Loop by the spline subintervals.
            for(int i = 1; i < points.Count; ++i)
            {
                try
                {
                    Collection<Point> cpPoints = GetApproximation(points[i - 1], points[i]
                                                 , secondDerivatives[i - 1], secondDerivatives[i], tolerance);

                    // Copy points but the first one.
                    for(int j = 1; j < cpPoints.Count; ++j)
                    {
                        fakeExtensionPoints.Add(cpPoints[j]);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return fakeExtensionPoints;
        }

        /// <summary>
        /// 用折线逼近三次多项式并给出公差。
        /// </summary>
        /// <param name="leftPoint">三次多项式左点</param>
        /// <param name="rightPoint">三次多项式右点</param>
        /// <param name="secondDerivativeLeft">左点三次多项式二阶导数.</param>
        /// <param name="secondDerivativeRight">右端三次多项式二阶导数.</param>
        /// <param name="tolerance">公差，即样条到近似折线的最大距离。</param>
        /// <returns>在给定公差的情况下逼近三次多项式的多边形点的列表。</returns>
        private static Collection<Point> GetApproximation(Point leftPoint, Point rightPoint, double secondDerivativeLeft, double secondDerivativeRight, double tolerance)
        {
            // 左右俩点的X、Y轴值
            double leftPointX = leftPoint.X, rightPointX = rightPoint.X;
            double leftPointY = leftPoint.Y, rightPointY = rightPoint.Y;
            // 次区间多项式系数
            double a = (secondDerivativeRight - secondDerivativeLeft) / (6 * (rightPointX - leftPointX));
            double b = (secondDerivativeLeft - 6 * a * leftPointX) / 2;
            double c = (rightPointY - rightPointX * rightPointX * (a * rightPointX + b) - leftPointY + leftPointX * leftPointX * (a * leftPointX + b)) / (rightPointX - leftPointX);
            double d = leftPointY - leftPointX * (leftPointX * (a * leftPointX + b) + c);

            //如果a的值为0，则给a赋值double类型的最小正数
            if(a == 0)
                a = double.Epsilon;

            //通过多项式与拆线的逼近，获取多边形点的列表
            Collection<Point> points = CubicPolynomialPolylineApproximation.Approximate(new Polynomial(new double[] { d, c, b, a }), leftPointX, rightPointX, tolerance);
            return points;
        }
    }
}
