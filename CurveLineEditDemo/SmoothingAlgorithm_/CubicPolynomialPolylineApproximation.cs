using System.Collections.ObjectModel;
using System.Windows;

namespace CurveLineEditDemo
{
    /// <summary>
    /// 用折线逼近三次多项式。
    /// </summary>
    internal static class CubicPolynomialPolylineApproximation
    {
        /// <summary>
        /// 获取多项式与折线的逼近。
        /// </summary>
        /// <param name="polynomial">多项式</param>
        /// <param name="x1">起始点</param>
        /// <param name="x2">结束点</param>
        /// <param name="tolerance">公差，是从三次多项式到近似多段线的最大距离。</param>
        /// <returns></returns>
        public static Collection<Point> Approximate(Polynomial polynomial, double x1, double x2, double tolerance)
        {
            var points = new Collection<Point>();
            // 得到给定多项式与通过其节点的直线之间的差值。
            Polynomial deviation = DeviationPolynomial(polynomial, x1, x2);

            if(deviation[0] == 0 && deviation[1] == 0 && deviation[2] == 0 && deviation[3] == 0)
            {
                points.Add(new Point(x1, polynomial.GetValue(x1)));
                points.Add(new Point(x2, polynomial.GetValue(x2)));
                return points;
            }

            //得到一阶导数多项式
            Polynomial firstDerivative = new Polynomial(new double[] { deviation[1], 2 * deviation[2], 3 * deviation[3] });
            // 一阶导数根。
            Complex[] complexRoots = firstDerivative.Solve();
            // 在[x1，x2]中找到真正的根。
            List<double> roots = new List<double>();

            foreach(Complex complexRoot in complexRoots)
            {
                if(complexRoot.Imaginary == 0)
                {
                    double r = complexRoot.Real;

                    if(r > x1 && r < x2)
                        roots.Add(r);
                }
            }

            // 检查差分多项式极值。
            bool approximates = true;

            foreach(double x in roots)
            {
                if(Math.Abs(deviation.GetValue(x)) > tolerance)
                {
                    approximates = false;
                    break;
                }
            }

            //如果近似值满足公差（容忍度）
            if(approximates)
            {
                points.Add(new Point(x1, polynomial.GetValue(x1)));
                points.Add(new Point(x2, polynomial.GetValue(x2)));
                return points;
            }

            //对根进行排序、去重
            if(roots.Count == 2)
            {
                if(roots[0] == roots[1])
                    roots.RemoveAt(1);
                else if(roots[0] > roots[1])
                {
                    double x = roots[0];
                    roots[0] = roots[1];
                    roots[1] = x;
                }
            }

            //加上尾数横坐标。
            roots.Add(x2);
            //第一个子间隔。
            Collection<Point> pts = Approximate(polynomial, x1, roots[0], tolerance);

            foreach(Point pt in pts)
            {
                points.Add(pt);
            }

            //子间隔的剩余部分。
            for(int i = 0; i < roots.Count - 1; ++i)
            {
                pts = Approximate(polynomial, roots[i], roots[i + 1], tolerance);

                //复制除第一个点以外的所有点
                for(int j = 1; j < pts.Count; ++j)
                {
                    points.Add(pts[j]);
                }
            }

            return points;
        }

        /// <summary>
        /// 获取给定多项式与通过其节点的直线之间的差值。
        /// </summary>
        /// <param name="polynomial">The polynomial.</param>
        /// <param name="x1">The abscissas start.</param>
        /// <param name="x2">The abscissas stop.</param>
        /// <returns></returns>
        static Polynomial DeviationPolynomial(Polynomial polynomial, double x1, double x2)
        {
            double y1 = polynomial.GetValue(x1);
            double y2 = polynomial.GetValue(x2);
            double a = (y2 - y1) / (x2 - x1);
            double b = y1 - a * x1;

            if(a != 0)
                return polynomial.Subtract(new Polynomial(new double[] { b, a }));
            else if(b != 0)
                return polynomial.Subtract(new Polynomial(new double[] { b }));
            else
                return polynomial;
        }
    }
}
