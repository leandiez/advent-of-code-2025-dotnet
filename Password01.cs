using System;

public static class Password01
{
    // Ejecuta la función principal. Puedes pasar argumentos desde Program.cs:
    // Password01.Run(new string[] { "mi-entrada" });
    public static void Run(string fileName = null)
    {
        Console.WriteLine(fileName);
        if (!File.Exists(fileName))
        {
            Console.WriteLine("File not found");
            return;
        }
        Dial myDial = new Dial(50, true);
        int lineNum = 0;
        foreach (string line in File.ReadLines(fileName))
        {
            lineNum++;
            string newLine = line.Trim();
            if (string.IsNullOrEmpty(line)) continue;
            int num;
            int.TryParse(newLine.Substring(1), out num);
            if (newLine[0] == 'R') myDial.TurnRight(num);
            if (newLine[0] == 'L') myDial.TurnLeft(num);
        }
        Console.WriteLine("La contraseña es: " + myDial.ZeroPasses.ToString());
    }
}

public class Dial
{
    public int CurrentPosition { get; private set; }
    public int ZeroPasses { get; private set; }
    private const int _maxPositions = 100;
    private bool CountZeroPasses = false; // Cuenta todas las veces que paso por Zero si es true. Sino solo cuenta cuando cae en cero.
    public Dial(int initialPosition, bool countZeroPasses = false)
    {
        CurrentPosition = initialPosition;
        CountZeroPasses = countZeroPasses;
        ZeroPasses = 0;
    }

    public void TurnRight(int amount)
    {
        Console.WriteLine("Muevo a Derecha");
        int realAmount = AdjustAmount(amount);
        // Si la suma de la posicion actual mas el movimiento se va de rango, ajusto.
        if (CurrentPosition + realAmount >= _maxPositions)
        {
            CurrentPosition = (CurrentPosition + realAmount) % _maxPositions;
            if (CountZeroPasses) ZeroPasses++; // Si estoy contando todos los pases, sumo uno.
            if (!CountZeroPasses && CurrentPosition == 0) ZeroPasses++; // Si solo cuento cuando cae en cero, verifico y sumo.
            return;
        }
        // Si no se va de rango, sumo.
        CurrentPosition += realAmount;
        return;
    }

    public void TurnLeft(int amount)
    {
        Console.WriteLine("Muevo a Izquierda");
        int realAmount = AdjustAmount(amount);
        // Si la resta de la posicion actual con el movimiento se va de rango, pase por cero.
        if (CurrentPosition - realAmount <= 0)
        {
            //Console.WriteLine("Posicion Actual: " + CurrentPosition.ToString() + "Restando: " + realAmount.ToString());
            CurrentPosition = (CurrentPosition - realAmount + _maxPositions) % _maxPositions; //Ajusto teniendo en cuenta que al quedar negativo tengo que sumar el maximo.
            //Console.WriteLine("Posicion Nueva: " + CurrentPosition.ToString());
            if (CountZeroPasses) ZeroPasses++; // Si estoy contando todos los pases, sumo uno.
            if (!CountZeroPasses && CurrentPosition == 0) ZeroPasses++; // Si solo cuento cuando cae en cero, verifico y sumo.
            return;
        }
        // Si no se va de rango, resto.
        CurrentPosition -= realAmount;
        return;
    }
    private int AdjustAmount(int initialAmount)
    {
        Console.WriteLine("Cantidad a mover: " + initialAmount.ToString());
        int realAmount;
        if (CountZeroPasses)
        {
            // Si la cantidad a mover es mayor al maximo entonces puedo descontar esas veces como un pase seguro por cero.
            // Similar al calculo del modulo o resto de un numero.
            if (initialAmount >= _maxPositions)
            {
                while (initialAmount >= _maxPositions)
                {
                    initialAmount -= _maxPositions;
                    ZeroPasses++;
                }
                realAmount = initialAmount;
            }
            else
            {
                realAmount = initialAmount;
            }
        }
        else
        {
            // Si no quiero contar todos los pases y solo cuando cae exacto, descarto las vueltas extra
            // Y me quedo con un numero mas chico para calcular.
            realAmount = initialAmount % _maxPositions;
        }
        Console.WriteLine("Pasajes por cero: " + ZeroPasses.ToString());
        return realAmount;
    }
}