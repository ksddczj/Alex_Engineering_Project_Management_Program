public static class InputGetter
{
    public static MainMenuUserOption GetMainMenuOptionFromUser()
    {
        Console.WriteLine("Select one of the options from the main menu:");
        return InputReader.ReadMainMenuOptionFromUser();
    }

    public static ManageExistingProjectMenuOption GetManageExistingProjectMenuOptionFromUser()
    {
        Console.WriteLine("Select one of the options from the menu:");
        return InputReader.ReadManageExistingProjectMenuOptionFromUser();
    }

    public static ProjectManagementMenuOption GetProjectManagementMenuOptionFromUser()
    {
        Console.WriteLine("Select one of the options from the menu:");
        return InputReader.ReadProjectManagementMenuOptionFromUser();
    }

    public static ProjectType GetProjectTypeFromUser()
    {
        MenuDisplay.DisplayProjectTypeMenu();
        Console.WriteLine("Select Project Type:");
        return InputReader.ReadProjectTypeFromUser();
    }

    public static string GetProjectNameFromUser()
    {
        Console.Write("Enter new project's name: ");
        return InputReader.ReadStringInputFromUser();
    }

    public static string GetExistingClientFromUser(List<string> existingClients)
    {
        Dictionary<int, string> clientsDictionary = MenuDisplay.DisplayItemsAsMenu(existingClients);
        Console.WriteLine("Select existing client: ");
        int userChoice = InputReader.ReadUserChoice(1, existingClients.Count);
        return clientsDictionary[userChoice];
    }

    public static (string, bool) GetClientFromUser(List<string> existingClients) //Review!! A list of existing client provided to user with option to add a new client if the client does not exist.
    {
        string? userInput;
        bool newClientToAdd;

        void GetNewClientFromUser() // local function for more enscalpslation
        {
            while (true) //get user to type in user name
            {
                Console.Write("Enter client name: ");
                userInput = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(userInput))
                {
                    //compare userinput with existing clients
                    if (existingClients.Contains(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Client already exists. Try a different one....");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("Client name entered.");
                        newClientToAdd = true;
                        break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Client name cannot be empty. Please try again.");
                    Console.ResetColor();
                }
            }
        }

        Console.WriteLine("Select client name or choose to enter new client: ");
        if (existingClients.Any())
        {
            Dictionary<int, string> clientsDictionary = MenuDisplay.DisplayItemsAsMenu(
                existingClients
            );
            Console.WriteLine($"{existingClients.Count + 1}. Add new client");
            int userChoice = InputReader.ReadUserChoice(1, existingClients.Count + 1);

            if (!(userChoice == existingClients.Count + 1))
            {
                userInput = clientsDictionary[userChoice];
                newClientToAdd = false;
            }
            else
            {
                GetNewClientFromUser();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("No client in the system yet. Add a new client ....");
            Console.ResetColor();
            GetNewClientFromUser();
        }

        string clientName = userInput!;

        return (clientName, newClientToAdd);
    }

    public static string GetProjectNameKeyWordFromUser()
    {
        Console.WriteLine("Type in keyword in project name to find project");
        return InputReader.ReadStringInputFromUser();
    }

    public static DateTime GetCompletionDateFromUser(ProjectType projectType)
    {
        Console.Write("Enter completion date (yyyy-mm-dd): ");

        DateTime completionDate;
        bool isValidDate = false;
        do
        {
            completionDate = InputReader.ReadDateInputFromUser();

            switch (projectType)
            {
                case ProjectType.AutomotiveEngineering:
                    if (completionDate > DateTime.Now.AddMonths(12))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Completion date entered.");
                        Console.ResetColor();
                        isValidDate = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "The completion date must be at least 12 months after current date. Please enter a valid future date."
                        );
                        Console.ResetColor();
                    }
                    break;
                case ProjectType.EngineeringDrafting:
                    if (completionDate > DateTime.Now.AddMonths(3))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Completion date entered.");
                        Console.ResetColor();
                        isValidDate = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "The completion date must be at least 3 after months current date. Please enter a valid future date."
                        );
                        Console.ResetColor();
                    }

                    break;
            }
        } while (!isValidDate);

        return completionDate;
    }

    public static DateTime GetDateFromUser()
    {
        Console.Write("Enter date (yyyy-mm-dd): ");
        return InputReader.ReadDateInputFromUser();
    }

    public static StageName GetProjectStageFromUser(ProjectType projectType) //review the logic is not great
    {
        Console.WriteLine("Select stage:");
        MenuDisplay.DisplayStageNameMenu(projectType);
        int choice;
        switch (projectType)
        {
            case ProjectType.AutomotiveEngineering:
                choice = InputReader.ReadUserChoice(
                    1,
                    MenuDisplay.AutomotiveEngineeringStageNameMenuOptionsCount
                );
                return (StageName)choice;
            case ProjectType.EngineeringDrafting:
                choice = InputReader.ReadUserChoice(
                    1,
                    MenuDisplay.EngineeringDraftingStageNameMenuOptionsCount
                );
                if (choice == 1)
                {
                    return (StageName)choice;
                }
                else
                {
                    return (StageName)(
                        choice + MenuDisplay.AutomotiveEngineeringStageNameMenuOptionsCount - 1
                    );
                }

            default:
                throw new ArgumentException("Invalid project type"); //this will never happen cos the selection output is controlled by getUserChoice that ensures the input is
        }
    }

    public static Project GetSelectedProjectFromUser(Dictionary<int, Project> projectsToSelectFrom)
    {
        Console.Write("Select the project you want to manage: ");
        int userChoice = InputReader.ReadUserChoice(1, projectsToSelectFrom.Count);
        return projectsToSelectFrom[userChoice];
    }

    public static string GetProjectIDFromUser()
    {
        Console.Write("Enter project ID: ");
        Console.WriteLine("Project ID starts with P followed by a number eg. P001.");
        return InputReader.ReadStringInputFromUser();
    }

    public static DateTime GetNewDeadlineFromUser()
    {
        Console.Write("Enter new deadline (yyyy-mm-dd): ");
        return InputReader.ReadDateInputFromUser();
    }
}
