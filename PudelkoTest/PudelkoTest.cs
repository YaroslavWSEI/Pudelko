using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace PudelkoUnitTests
{

    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
        
    }

    // ========================================
    [TestClass]
    public class UnitTestsPudelkoConstructors
    {
        private static double defaultSize = 0.1; // w metrach
        private static double accuracy = 0.001; //dokładność 3 miejsca po przecinku

        private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
        {
            Assert.AreEqual(expectedA, p.A, delta: accuracy);
            Assert.AreEqual(expectedB, p.B, delta: accuracy);
            Assert.AreEqual(expectedC, p.C, delta: accuracy);
        }

        #region Constructor tests ================================

        [TestMethod, TestCategory("Constructors")]
        public void Constructor_Default()
        {
            Pudelko p = new Pudelko();

            Assert.AreEqual(defaultSize, p.A, delta: accuracy);
            Assert.AreEqual(defaultSize, p.B, delta: accuracy);
            Assert.AreEqual(defaultSize, p.C, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_DefaultMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_InMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100.0, 25.5, 3.1,
                 1.0, 0.255, 0.031)]
        [DataRow(100.0, 25.58, 3.13,
                 1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
        public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100, 255, 300, 0.1, 0.255, 0.3)] // Корректные данные
        [DataRow(1000, 2000, 3000, 1.0, 2.0, 3.0)] // Ещё один пример
        public void Constructor_3params_InMilimeters(double a, double b, double c, double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        // ----

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a, b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a: a, b: b, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 2.5, 0.11, 0.025)]
        [DataRow(100.1, 2.599, 1.001, 0.025)]
        [DataRow(2.0019, 0.25999, 0.02, 0.002)]
        public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 2.0, 0.011, 0.002)]
        [DataRow(100.1, 2599, 0.1, 2.599)]
        [DataRow(200.19, 2.5999, 0.2, 0.002)]
        public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        // -------

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_DefaultMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_InMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 0.11)]
        [DataRow(100.1, 1.001)]
        [DataRow(2.0019, 0.02)]
        public void Constructor_1param_InCentimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 0.011)]
        [DataRow(100.1, 0.1)]
        [DataRow(200.19, 0.2)]
        public void Constructor_1param_InMilimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        // ---

        public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.01, 0.1, 1)]
        [DataRow(0.1, 0.01, 1)]
        [DataRow(0.1, 0.1, 0.01)]
        [DataRow(1001, 1, 1)]
        [DataRow(1, 1001, 1)]
        [DataRow(1, 1, 1001)]
        [DataRow(1001, 1, 1001)]
        [DataRow(1, 1001, 1001)]
        [DataRow(1001, 1001, 1)]
        [DataRow(1001, 1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.1, 1, 1)]
        [DataRow(1, 0.1, 1)]
        [DataRow(1, 1, 0.1)]
        [DataRow(10001, 1, 1)]
        [DataRow(1, 10001, 1)]
        [DataRow(1, 1, 10001)]
        [DataRow(10001, 10001, 1)]
        [DataRow(10001, 1, 10001)]
        [DataRow(1, 10001, 10001)]
        [DataRow(10001, 10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);
        }


        public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.01, 1)]
        [DataRow(1, 0.01)]
        [DataRow(0.01, 0.01)]
        [DataRow(1001, 1)]
        [DataRow(1, 1001)]
        [DataRow(1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.1, 1)]
        [DataRow(1, 0.1)]
        [DataRow(0.1, 0.1)]
        [DataRow(10001, 1)]
        [DataRow(1, 10001)]
        [DataRow(10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.milimeter);
        }




        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(0.01)]
        [DataRow(1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.milimeter);
        }

        #endregion


        #region ToString tests ===================================

        [TestMethod, TestCategory("String representation")]
        public void ToString_Default_Culture_EN()
        {
            var p = new Pudelko(2.5, 9.321);
            string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

            Assert.AreEqual(expectedStringEN, p.ToString());
        }

        [DataTestMethod, TestCategory("String representation")]
        [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
        [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
        public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
        {
            var p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
            Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
        }

        [TestMethod, TestCategory("String representation")]
        [ExpectedException(typeof(FormatException))]
        public void ToString_Formattable_WrongFormat_FormatException()
        {
            var p = new Pudelko(1);
            var stringformatedrepreentation = p.ToString("wrong code");
        }

        #endregion


        #region Pole, Objętość ===================================

        // Test sprawdza, czy właściwość Objetosc poprawnie oblicza objętość pudełka.
        [TestMethod, TestCategory("Pole, Objętość")]
        public void Objetosc_PoprawneObliczenie()
        {
            // AAA

            // Arrange
            var pudelko = new Pudelko(2.5, 3.1, 4.2);
            double oczekiwanaObjetosc = Math.Round(2.5 * 3.1 * 4.2, 9);
            // Act
            double rzeczywistaObjetosc = pudelko.Objetosc();
            // Assert
            Assert.AreEqual(oczekiwanaObjetosc, rzeczywistaObjetosc, 1e-9);
        }

        // Test sprawdza, czy właściwość Pole poprawnie oblicza pole powierzchni pudełka.
        [TestMethod, TestCategory("Pole, Objętość")]
        public void Pole_PoprawneObliczenie()
        {
            // AAA

            // Arrange
            var pudelko = new Pudelko(2.5, 3.1, 4.2);
            double oczekiwanePole = Math.Round(2 * (2.5 * 3.1 + 3.1 * 4.2 + 2.5 * 4.2), 6);
            // Act
            double rzeczywistePole = pudelko.Pole();
            // Assert
            Assert.AreEqual(oczekiwanePole, rzeczywistePole, 1e-6);
        }

        // Test sprawdza właściwości Objetosc i Pole dla różnych wymiarów pudełka.
        [DataTestMethod, TestCategory("Pole, Objętość")]
        [DataRow(1.0, 1.0, 1.0, 1.0, 6.0)]
        [DataRow(2.0, 3.0, 4.0, 24.0, 52.0)]
        [DataRow(0.1, 0.2, 0.3, 0.006, 0.22)]
        public void ObjetoscIPole_TestParametryzowany(double a, double b, double c, double oczekiwanaObjetosc, double oczekiwanePole)
        {
            // AAA

            // Arrange
            var pudelko = new Pudelko(a, b, c);
            // Act
            double rzeczywistaObjetosc = pudelko.Objetosc();
            double rzeczywistePole = pudelko.Pole();
            // Assert
            Assert.AreEqual(oczekiwanaObjetosc, rzeczywistaObjetosc, 1e-9);
            Assert.AreEqual(oczekiwanePole, rzeczywistePole, 1e-6);
        }


        #endregion

        #region Equals ===========================================
        [TestMethod, TestCategory("Equals")]
        // Test sprawdza, czy metoda Equals poprawnie identyfikuje dwa pudełka o tych samych wymiarach jako równe.
        public void Equals_PudelkaRowne()
        {
            // AAA 

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            var p2 = new Pudelko(2.5, 3.1, 4.2);

            // Act
            bool wynik = p1.Equals(p2);

            // Assert
            Assert.IsTrue(wynik);
        }

        // Test sprawdza, czy metoda Equals poprawnie identyfikuje dwa pudełka o różnych wymiarach jako różne.
        [TestMethod, TestCategory("Equals")]
        public void Equals_PudelkaRozne()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            var p2 = new Pudelko(3.0, 2.0, 5.0);

            // Act
            bool wynik = p1.Equals(p2);

            // Assert
            Assert.IsFalse(wynik);
        }

        // Test sprawdza, czy metoda Equals poprawnie identyfikuje pudełko jako różne od null.
        [TestMethod, TestCategory("Equals")]
        public void Equals_PudelkoZNull()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            Pudelko? p2 = null;

            // Act
            bool wynik = p1.Equals(p2);

            // Assert
            Assert.IsFalse(wynik);
        }

        // Test sprawdza, czy metoda Equals poprawnie identyfikuje dwa pudełka o tych samych wymiarach w innej kolejności jako równe.
        [TestMethod, TestCategory("Equals")]
        public void Equals_RozneKolejnosciWymiarow()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            var p2 = new Pudelko(4.2, 2.5, 3.1);

            // Act
            bool wynik = p1.Equals(p2);

            // Assert
            Assert.IsTrue(wynik);
        }

        // Test sprawdza, czy metoda Equals poprawnie identyfikuje obiekt innego typu jako różny.
        [TestMethod, TestCategory("Equals")]
        public void Equals_PudelkoZInnymTypem()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            var innyObiekt = "Nie Pudelko";

            // Act
            bool wynik = p1.Equals(innyObiekt);

            // Assert
            Assert.IsFalse(wynik);
        }


        #endregion

        #region Operators overloading ===========================
        [TestMethod, TestCategory("Operators overloading")]
        // Test sprawdza, czy operator "+" poprawnie łączy pudełko z domyślnym pudełkiem
        public void Operator_Plus_LaczeniePudelek()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(2.5, 3.1, 4.2);
            var p2 = new Pudelko(3.0, 2.0, 5.0);

            // Act
            var result = p1 + p2;

            // Assert
            Assert.AreEqual(3.0, result.A, 1e-3);
            Assert.AreEqual(3.1, result.B, 1e-3);
            Assert.AreEqual(5.0, result.C, 1e-3);
        }

        // Test sprawdza, czy operator "+" poprawnie łączy dwa pudełka o identycznych wymiarach,  gdzie wynikowe wymiary powinny być takie same jak wymiary wejściowe.
        [TestMethod, TestCategory("Operators overloading")]
        public void Operator_Plus_LaczenieZRowneWymiary()
        {
            // AAA

            // Arrange
            var p1 = new Pudelko(1.0, 2.0, 3.0);
            var p2 = new Pudelko();

            // Act
            var result = p1 + p2;

            // Assert
            Assert.AreEqual(1.0, result.A, 1e-3);
            Assert.AreEqual(2.0, result.B, 1e-3);
            Assert.AreEqual(3.0, result.C, 1e-3);
        }

        #endregion

        #region Conversions =====================================
        [TestMethod]
        public void ExplicitConversion_ToDoubleArray_AsMeters()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            double[] tab = (double[])p;
            Assert.AreEqual(3, tab.Length);
            Assert.AreEqual(p.A, tab[0]);
            Assert.AreEqual(p.B, tab[1]);
            Assert.AreEqual(p.C, tab[2]);
        }

        [TestMethod]
        public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
        {
            var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
            Pudelko p = (a, b, c);
            Assert.AreEqual((int)(p.A * 1000), a);
            Assert.AreEqual((int)(p.B * 1000), b);
            Assert.AreEqual((int)(p.C * 1000), c);
        }

        #endregion

        #region Indexer, enumeration ============================
        [TestMethod]
        public void Indexer_ReadFrom()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            Assert.AreEqual(p.A, p[0]);
            Assert.AreEqual(p.B, p[1]);
            Assert.AreEqual(p.C, p[2]);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            var tab = new[] { p.A, p.B, p.C };
            int i = 0;
            foreach (double x in p)
            {
                Assert.AreEqual(x, tab[i]);
                i++;
            }
        }

        #endregion

        #region Parsing =========================================

        [TestMethod, TestCategory("Parsing")]
        // Test sprawdza, czy metoda Parse poprawnie parsuje ciąg wejściowy w metrach.
        public void Parse_PoprawnyFormat_Metry()
        {
            // TAA

            // Arrange
            string input = "2.500 m × 3.100 m × 4.200 m";

            // Act
            var wynik = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(2.5, wynik.A, 1e-3);
            Assert.AreEqual(3.1, wynik.B, 1e-3);
            Assert.AreEqual(4.2, wynik.C, 1e-3);
        }

        [TestMethod, TestCategory("Parsing")]
        // Test sprawdza, czy metoda Parse poprawnie parsuje ciąg wejściowy w centymetrach.
        public void Parse_PoprawnyFormat_Centymetry()
        {
            // AAA

            // Arrange
            string input = "250.0 cm × 310.0 cm × 420.0 cm";

            // Act
            var wynik = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(2.5, wynik.A, 1e-3);
            Assert.AreEqual(3.1, wynik.B, 1e-3);
            Assert.AreEqual(4.2, wynik.C, 1e-3);
        }

        [TestMethod, TestCategory("Parsing")]
        // Test sprawdza, czy metoda Parse poprawnie parsuje ciąg wejściowy w milimetrach.
        public void Parse_PoprawnyFormat_Milimetry()
        {
            // AAA

            // Arrange
            string input = "2500 mm × 3100 mm × 4200 mm";

            // Act
            var wynik = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(2.5, wynik.A, 1e-3);
            Assert.AreEqual(3.1, wynik.B, 1e-3);
            Assert.AreEqual(4.2, wynik.C, 1e-3);
        }

        [TestMethod, TestCategory("Parsing")]
        [ExpectedException(typeof(ArgumentNullException))]
        // Test sprawdza, czy metoda Parse rzuca wyjątek ArgumentNullException dla pustego ciągu.
        public void Parse_NullInput_ArgumentNullException()
        {
            // AAA

            // Arrange
            string input = null;

            // Act
            Pudelko.Parse(input);

            // Assert 
        }

        [TestMethod, TestCategory("Parsing")]
        [ExpectedException(typeof(FormatException))]
        // Test sprawdza, czy metoda Parse rzuca wyjątek FormatException dla nieprawidłowego formatu.
        public void Parse_NieprawidlowyFormat_FormatException()
        {
            // AAA

            // Arrange
            string input = "2.5m × 3.1m";

            // Act
            Pudelko.Parse(input);

            // Assert 
        }

        [TestMethod, TestCategory("Parsing")]
        [ExpectedException(typeof(FormatException))]
        // Test sprawdza, czy metoda Parse rzuca wyjątek FormatException dla nieobsługiwanej jednostki.
        public void Parse_NieprawidlowaJednostka_FormatException()
        {
            // AAA

            // Arrange
            string input = "2.5 km × 3.1 km × 4.2 km";

            // Act
            Pudelko.Parse(input);

            // Assert 
        }

        [TestMethod, TestCategory("Parsing")]
        // Test sprawdza, czy metoda Parse poprawnie parsuje ciąg wejściowy z różnymi jednostkami.
        public void Parse_RozneJednostki()
        {
            // AAA
            // Arrange
            string input = "2.5 m × 310 cm × 4200 mm";

            // Act
            var wynik = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(2.5, wynik.A, 1e-3);
            Assert.AreEqual(3.1, wynik.B, 1e-3);
            Assert.AreEqual(4.2, wynik.C, 1e-3);
        }
        #endregion
    }
}