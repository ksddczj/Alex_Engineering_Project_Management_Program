using System.Runtime.InteropServices.Marshalling;

public static class InputReader
{
    public static int ReadUserChoice(int min, int max)
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.Write($"Invalid choice. Please enter a number between {min} and {max}: ");
        }
        return choice;
    }

    public static MainMenuUserOption ReadMainMenuOptionFromUser()
    {
        int choice = ReadUserChoice(1, Enum.GetValues(typeof(MainMenuUserOption)).Length);
        return (MainMenuUserOption)choice;
    }

    public static ManageExistingProjectMenuOption ReadManageExistingProjectMenuOptionFromUser()
    {
        int choice = ReadUserChoice(
            1,
            Enum.GetValues(typeof(ManageExistingProjectMenuOption)).Length
        );
        return (ManageExistingProjectMenuOption)choice;
    }

    public static ProjectManagementMenuOption ReadProjectManagementMenuOptionFromUser()
    {
        int choice = ReadUserChoice(1, Enum.GetValues(typeof(ProjectManagementMenuOption)).Length);
        return (ProjectManagementMenuOption)choice;
    }

    public static string ReadStringInputFromUser()
    {
        string? userInput;
        do
        {
            userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
            else
            {
                Console.WriteLine("Input entered.");
            }
        } while (string.IsNullOrEmpty(userInput));

        return userInput!;
    }

    public static ProjectType ReadProjectTypeFromUser()
    {
        int choice = ReadUserChoice(1, Enum.GetValues(typeof(ProjectType)).Length);
        return (ProjectType)choice;
    }

    public static DateTime ReadDateInputFromUser()
    {
        DateTime date;
        string? userInput;
        bool isValidDate;
        do
        {
            userInput = Console.ReadLine()?.Trim();
            isValidDate = DateTime.TryParse(userInput, out date);

            if (isValidDate)
            {
                Console.WriteLine("Date entered.");
            }
            else
            {
                Console.WriteLine(
                    "Invalid date format. Please enter a valid date in the format yyyy-mm-dd."
                );
            }
        } while (!isValidDate);

        return date;
    }
}
