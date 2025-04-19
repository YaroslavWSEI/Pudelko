using System;
using System.Collections.Generic;
using PudelkoLibrary;

class Program
{
    static void Main()
    {
        List<Pudelko> pudelka = new List<Pudelko>
        {
            new Pudelko(1.5, 2.5, 3.5),
            new Pudelko(100, 200, 300, UnitOfMeasure.centimeter),
            new Pudelko(500, 1000, 1500, UnitOfMeasure.milimeter),
            new Pudelko(1.0, 1.0, 1.0),
            new Pudelko(0.5, 0.5, 0.5)
        };

        Console.WriteLine("Pudełka przed sortowaniem:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Dane pudełka: {pudelko}");
        }

        pudelka.Sort((p1, p2) =>
        {
            int byVolume = p1.Objetosc().CompareTo(p2.Objetosc());
            if (byVolume != 0) return byVolume;

            int byArea = p1.Pole().CompareTo(p2.Pole());
            if (byArea != 0) return byArea;

            double edges1 = p1.A + p1.B + p1.C;
            double edges2 = p2.A + p2.B + p2.C;
            return edges1.CompareTo(edges2);
        });

        Console.WriteLine("\nPudełka po sortowaniu:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Dane pudełka: {pudelko}");
        }

        Console.WriteLine("\nObjętości pudełek:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Dane pudełka: {pudelko}, Objętość: {pudelko.Objetosc()} m3");
        }

        Console.WriteLine("\nPola powierzchni pudełek:");
        foreach (var pudelko in pudelka)
        {
            Console.WriteLine($"Dane pudełka: {pudelko}, Powierzchnia: {pudelko.Pole()} m2");
        }

        Console.WriteLine("\nSuma krawędzi pudełek:");
        foreach (var pudelko in pudelka)
        {
            double edges = pudelko.A + pudelko.B + pudelko.C;
            Console.WriteLine($"Dane pudełka: {pudelko}, Suma krawędzi: {edges} m");
        }

        Console.WriteLine("\nSprawdzanie poprawności sortowania:");
        bool isSorted = true;
        for (int i = 0; i < pudelka.Count - 1; i++)
        {
            if (pudelka[i].Objetosc().CompareTo(pudelka[i + 1].Objetosc()) > 0)
            {
                isSorted = false;
                Console.WriteLine($"Blad: {pudelka[i]} i {pudelka[i + 1]} sa w zlej kolejności.");
            }
        }

        Console.WriteLine(isSorted ? "Sortowanie zakończone pomyślnie." : "Sortowanie zakończone niepowodzeniem.");
    }
}
