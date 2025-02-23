public static class MenuDisplay
{
    public static int AutomotiveEngineeringStageNameMenuOptionsCount { get; } = 3;
    public static int EngineeringDraftingStageNameMenuOptionsCount { get; } = 2;

    public static void DisplayMainMenu()
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Create New Project");
        Console.WriteLine("2. Manage Existing Project");
        Console.WriteLine("3. Exit");
        Console.WriteLine("----------------------------");
    }

    public static void DisplayManageExistingProjectMenu()
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Manage Existing Project Menu:");
        Console.WriteLine("1. Find Projects by Name");
        Console.WriteLine("2. Find Projects by Client");
        Console.WriteLine("3. Find Projects by ID");
        Console.WriteLine("4. Find Projects by Completion Date");
        Console.WriteLine("5. Find Projects by Project Type");
        Console.WriteLine("6. Return");
        Console.WriteLine("----------------------------");
    }

    public static void DisplayProjectTypeMenu()
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("1. Automotive Engineering");
        Console.WriteLine("2. Engineering Drafting");
        Console.WriteLine("----------------------------");
    }

    public static void DisplayStageNameMenu(ProjectType projectType)
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Select Stage Name:");

        switch (projectType)
        {
            case ProjectType.AutomotiveEngineering:

                Console.WriteLine("1. Project Start");
                Console.WriteLine("2. Concept Completion");
                Console.WriteLine("3. Detailed Design Completion");
                break;

            case ProjectType.EngineeringDrafting:
                Console.WriteLine("1. Project Start");
                Console.WriteLine("2. Drafting Completion");
                break;
            default:
                Console.WriteLine("No menu to display");
                break;
        }
        Console.WriteLine("----------------------------");
    }

    public static void DisplayProjectManagementMenu()
    {
        Console.WriteLine("----------------------------");
        Console.WriteLine("Managing Project: ");
        Console.WriteLine("1. Update Stage Deadline");
        Console.WriteLine("2. Mark Stage as Completed");
        Console.WriteLine("3. Get Project Status");
        Console.WriteLine("4. Return");
        Console.WriteLine("----------------------------");
    }

    public static Dictionary<int, Project> DisplayItemsAsMenu(List<Project> projectsToDisplay) //method overload
    {
        Dictionary<int, Project> indexToProject = new Dictionary<int, Project>();
        for (int i = 0; i < projectsToDisplay.Count; i++)
        {
            indexToProject.Add(i + 1, projectsToDisplay[i]);
        }
        Console.WriteLine(
            "----------------------------------------------------------------------------------------------------------------------------------------"
        );
        foreach (KeyValuePair<int, Project> item in indexToProject)
        {
            Project project = item.Value;
            int index = item.Key;
            Console.WriteLine(
                $"{index}. Name: {project.ProjectName}, Type: {project.Type}, Client: {project.Client}, Completion Date: {project.CompletionDate.ToShortDateString()}, Project ID: {project.ProjectID}"
            );
        }
        Console.WriteLine(
            "----------------------------------------------------------------------------------------------------------------------------------------"
        );
        return indexToProject;
    }

    public static Dictionary<int, string> DisplayItemsAsMenu(List<string> clientsToDisplay) //method overload
    {
        Dictionary<int, string> indexToClient = new Dictionary<int, string>();
        for (int i = 0; i < clientsToDisplay.Count; i++)
        {
            indexToClient.Add(i + 1, clientsToDisplay[i]);
        }

        Console.WriteLine("-------------------------------------------");
        foreach (KeyValuePair<int, string> item in indexToClient)
        {
            string clientName = item.Value;
            int index = item.Key;
            Console.WriteLine($"{index}. Client: {clientName}");
        }
        Console.WriteLine("-------------------------------------------");
        return indexToClient;
    }
}
