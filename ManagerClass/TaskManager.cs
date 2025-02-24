public static class TaskManager
{
    public static void DoCreateNewProject()
    {
        Console.WriteLine("Creating new project...");

        //get project type
        ProjectType projectType = InputGetter.GetProjectTypeFromUser();

        //get project name
        string projectName = InputGetter.GetProjectNameFromUser();

        //get client name. if the client from user does not exist add it to the client list.
        var result = InputGetter.GetClientFromUser(ProjectManager.FindExistingClientFromDB()); // review it does not need to return a bool value to signal adding client
        string client = result.Item1;
        // bool newClientToAdd = result.Item2; //not needed anymore
        // if (newClientToAdd)
        // {
        //     ProjectManager.AddClient(result.Item1);
        // }

        //get completion date
        DateTime completionDate = InputGetter.GetCompletionDateFromUser(projectType);

        //create new project
        ProjectManager.CreateNewProject(projectName, client, projectType, completionDate);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("New project created.");
        Console.ResetColor();
    }

    public static List<Project> DoFindProjectByName()
    {
        string keyWord = InputGetter.GetProjectNameKeyWordFromUser();
        return ProjectManager.FindProjectsByName(keyWord);
    }

    public static List<Project> DoFindProjectByClient()
    {
        string projectClientFromUser = InputGetter.GetExistingClientFromUser(
            ProjectManager.FindExistingClientFromDB()
        );
        return ProjectManager.FindProjectsByClient(projectClientFromUser);
    }

    public static List<Project> DoFindProjectByID()
    {
        string projectIDFromUser = InputGetter.GetProjectIDFromUser();
        return ProjectManager.FindProjectsByID(projectIDFromUser);
    }

    public static List<Project> DoFindProjectByCompletionDate()
    {
        DateTime projectCompletionDateFromUser = InputGetter.GetDateFromUser();
        return ProjectManager.FindProjectsByCompletionDate(projectCompletionDateFromUser);
    }

    public static List<Project> DoFindProjectByType()
    {
        ProjectType projectTypeFromUser = InputGetter.GetProjectTypeFromUser();
        return ProjectManager.FindProjectsByType(projectTypeFromUser);
    }

    public static void DoUpdateStageDeadline(Project projectToUpdate)
    {
        StageName nameOfTheStageToUpdate = InputGetter.GetProjectStageFromUser(
            projectToUpdate.Type
        );

        //judge if the stage is already marked completed
        if (ProjectManager.CheckProjectStageIsCompleted(projectToUpdate, nameOfTheStageToUpdate))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                "Stage is alreay makred as completed. Deadline of it can not be updated."
            );
            Console.ResetColor();
            return;
        }

        DateTime newDealine = InputGetter.GetNewDeadlineFromUser();
        ProjectManager.UpdateProjectStageDeadline(
            projectToUpdate,
            nameOfTheStageToUpdate,
            newDealine
        );
    }

    public static void DoMarkStageAsCompleted(Project projectToUpdate)
    {
        StageName nameOfTheStageToMarkAsCompleted = InputGetter.GetProjectStageFromUser(
            projectToUpdate.Type
        );
        if (
            ProjectManager.CheckProjectStageIsCompleted(
                projectToUpdate,
                nameOfTheStageToMarkAsCompleted
            )
        )
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Stage is alreay makred as completed.");
            Console.ResetColor();
            return;
        }

        ProjectManager.MarkProjectStageCompleted(projectToUpdate, nameOfTheStageToMarkAsCompleted);
    }

    public static void DoGetProjectStatus(Project projectToShowStatus)
    {
        ProjectManager.GetProjectStatus(projectToShowStatus);
    }
}
