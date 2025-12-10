using System;


public static class InvalidIds02
{
    public static void Run(string fileName = null)
    {
        string relPath = @fileName;

        if (!File.Exists(relPath))
        {
            Console.WriteLine("File not found");
            return;
        }
        string invalidIdsLine = File.ReadLines(relPath).First();
        string[] invalidIdsArray = invalidIdsLine.Split(",");
        foreach (string invId in invalidIdsArray)
        {
            Console.WriteLine("ID inválido: " + invId);
        }

    }
}
