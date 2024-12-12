namespace CurveLineEditDemo
{

    /// <summary>
    /// Complex Number
    /// </summary>
    internal class Complex
    {
        private double re, im;
        public static readonly Complex Zero = new Complex();

        public Complex() {}
        public Complex(double re)
        {
            this.re = re;
        }
        public Complex(double re, double im)
        {
            this.re = re;
            this.im = im;
        }

        public double Real
        {
            get { return re; }
            set { re = value; }
        }
        public double Imaginary
        {
            get { return im; }
            set { im = value; }
        }

        public override bool Equals(object obj)
        {
            if(base.Equals(obj))
                return true;

            Complex c = obj as Complex;

            if(c == null)
                return false;

            return (c == this);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            if(im == 0.0)
                return re.ToString();

            if(re == 0.0)
                return string.Format("{0}i{1}", im < 0.0 ? "-" : "+", Math.Abs(im));

            return string.Format("{0}{1}i{2}", re, im < 0.0 ? "-" : "+", Math.Abs(im));
        }

        public static bool operator==(Complex a, Complex b)
        {
            if(a.re == b.re && a.im == b.im)
                return true;

            return false;
        }
        public static bool operator!=(Complex a, Complex b)
        {
            return !(a == b);
        }

        public static Complex Sqrt(Complex z)
        {
            if(z.re == 0.0 && z.im == 0.0)
            {
                return new Complex();
            }
            else
            {
                Complex c = new Complex();
                double x = Math.Abs(z.re);
                double y = Math.Abs(z.im);
                double w, r;

                if(x >= y)
                {
                    r = y / x;
                    w = Math.Sqrt(x) * Math.Sqrt(0.5 * (1.0 + Math.Sqrt(1.0 + r * r)));
                }
                else
                {
                    r = x / y;
                    w = Math.Sqrt(y) * Math.Sqrt(0.5 * (r + Math.Sqrt(1.0 + r * r)));
                }

                if(z.re >= 0.0)
                {
                    c.re = w;
                    c.im = z.im / (2.0 * w);
                }
                else
                {
                    c.im = (z.im >= 0) ? w : -w;
                    c.re = z.im / (2.0 * c.im);
                }

                return c;
            }
        }

        #region ÒÅÆú´úÂë

        // complex conjugate operation
        public static Complex Conjugate(Complex z)
        {
            return new Complex(z.re, -z.im);
        }


        public static Complex operator -(Complex a)
        {
            return new Complex(-a.re, -a.im);
        }
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.re + b.re, a.im + b.im);
        }
        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.re - b.re, a.im - b.im);
        }
        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.re * b.re - a.im * b.im, a.im * b.re + a.re * b.im);
        }
        public static Complex operator *(double x, Complex a)
        {
            return new Complex(a.re * x, a.im * x);
        }
        public static Complex operator /(Complex a, Complex b)
        {
            double div = b.re * b.re + b.im * b.im;
            return new Complex((a.re * b.re + a.im * b.im) / div, (a.im * b.re - a.re * b.im) / div);
        }

        #endregion
    }
}
