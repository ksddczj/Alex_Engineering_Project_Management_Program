using System;

public class Program
{
    public static void Main()
    {
        try
        {
            MenuManager.OperateMainMenu();
        }
        catch (System.Exception)
        {
            Console.WriteLine("Something went wrong! Program will Stop.");
            throw;
        }
    }
}
