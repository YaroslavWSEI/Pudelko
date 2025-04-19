using System;
using System.Collections; 
using System.Collections.Generic;   
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;


namespace PudelkoLibrary
{

    public enum UnitOfMeasure
    {
        milimeter,
        centimeter,
        meter
    }

    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private void OdpWym(params double[] wymiary)
        {
            foreach (var dimension in wymiary)
            {
                if (dimension < MinWymiar || dimension > MaxWymiary)
                {
                    throw new ArgumentOutOfRangeException(nameof(wymiary), "Wymiary muszą być w zakresie 0.1 - 10.0");
                }
            }
        }
        //zad1
        private const double MinWymiar = 0.1;
        private const double MaxWymiary = 10.0;
        private readonly double a;
        private readonly double b;
        private readonly double c;
        //Zad3
        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);
        public UnitOfMeasure Unit { get; }
        //zad2
        private double convertacjaMetr(double value, UnitOfMeasure unit)
        {
            return unit switch
            {
                UnitOfMeasure.milimeter => value / 1000.0,
                UnitOfMeasure.centimeter => value / 100.0,
                UnitOfMeasure.meter => value,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Niepoprawna jednostka")
            };
        }


        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            this.a = Math.Round(convertacjaMetr(a ?? MinWymiar, unit), 3);
            this.b = Math.Round(convertacjaMetr(b ?? MinWymiar, unit), 3);
            this.c = Math.Round(convertacjaMetr(c ?? MinWymiar, unit), 3);
            OdpWym(this.a, this.b, this.c);
            Unit = unit;
        }
        //Zad4
        public  string ToString(string format) {
            return ToString(format, null);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "m";

            formatProvider ??= CultureInfo.InvariantCulture;

            return format.ToLower() switch
            {
                "m" => $"{A.ToString("0.000", formatProvider)} m × {B.ToString("0.000", formatProvider)} m × {C.ToString("0.000", formatProvider)} m",
                "cm" => $"{(A * 100).ToString("0.0", formatProvider)} cm × {(B * 100).ToString("0.0", formatProvider)} cm × {(C * 100).ToString("0.0", formatProvider)} cm",
                "mm" => $"{(A * 1000).ToString("0", formatProvider)} mm × {(B * 1000).ToString("0", formatProvider)} mm × {(C * 1000).ToString("0", formatProvider)} mm",
                _ => throw new FormatException($"Niepoprawny format: {format}")
            };
        }
        //Zad5
        public double Objetosc()
        {
            return Math.Round(a * b * c, 3);
        }
        //zad6
        public double Pole()
        {
            return Math.Round(2 * (a * b + a * c + b * c), 3);
        }
        //zad7
        public bool Equals(Pudelko other)
        {
            if (other == null) return false;

            var dimensions = new[] { A, B, C };
            var otherDimensions = new[] { other.A, other.B, other.C };

            Array.Sort(dimensions);
            Array.Sort(otherDimensions);

            return dimensions.SequenceEqual(otherDimensions);
        }
        public override bool Equals(object obj)
        {
            if (obj is Pudelko other)
            {
                return Equals(other);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(a, b, c);
        }
        public static bool operator  == (Pudelko left, Pudelko right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }
        public static bool operator != (Pudelko left, Pudelko right)
        {
            return !(left == right);
        }
        //zad8
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double newA = Math.Max(p1.A, p2.A);
            double newB = Math.Max(p1.B, p2.B);
            double newC = Math.Max(p1.C, p2.C);

            return new Pudelko(newA, newB, newC, UnitOfMeasure.meter);
        }
        //zad9 
        public static explicit operator double[](Pudelko p)
        {
            return new[]{ p.A,p.B,p.C };
        }
        public static implicit operator Pudelko((int a, int b, int c) dimensions) => new(dimensions.a / 1000.0, dimensions.b / 1000.0, dimensions.c / 1000.0, UnitOfMeasure.meter);
        //zad10
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new IndexOutOfRangeException("Indeks musi być 0, 1 lub 2")
                };
            }
        }
        //zad11
        public IEnumerator<double> GetEnumerator() => ((IEnumerable<double>)new[] { A, B, C }).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //zad12
        public static Pudelko Parse(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                throw new ArgumentNullException(nameof(tekst), "Ciąg wejściowy nie może być pusty");

            var czesci = tekst.Split('×', StringSplitOptions.RemoveEmptyEntries);
            if (czesci.Length != 3)
                throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

            double[] wymiary = new double[3];

            for (int i = 0; i < czesci.Length; i++)
            {
                var czesc = czesci[i].Trim();
                var wartoscJednostka = czesc.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (wartoscJednostka.Length != 2)
                    throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

                if (!double.TryParse(wartoscJednostka[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double wartosc))
                    throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");

                string jednostka = wartoscJednostka[1];
                if (jednostka == "m")
                    wymiary[i] = wartosc;
                else if (jednostka == "cm")
                    wymiary[i] = wartosc / 100;
                else if (jednostka == "mm")
                    wymiary[i] = wartosc / 1000;
                else
                    throw new FormatException("Ciąg wejściowy ma nieprawidłowy format");
            }

            return new Pudelko(wymiary[0], wymiary[1], wymiary[2], UnitOfMeasure.meter);
        }
        public override string ToString()
        {
            return ToString("m", CultureInfo.InvariantCulture);
        }


    }
}
