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
        Console.WriteLine("New project created.");
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

    public static void DoUpdateStageDeadline(Project projectToUpdate) //review timeline
    {
        StageName stageToUpdate = InputGetter.GetProjectStageFromUser(projectToUpdate.Type);
        DateTime newDealine = InputGetter.GetNewDeadlineFromUser();
        ProjectManager.UpdateProjectStageDeadline(projectToUpdate, stageToUpdate, newDealine);
    }

    public static void DoMarkStageAsCompleted(Project projectToUpdate) //review timeline
    {
        StageName stageToMarkAsCompleted = InputGetter.GetProjectStageFromUser(
            projectToUpdate.Type
        );
        ProjectManager.MarkProjectStageCompleted(projectToUpdate, stageToMarkAsCompleted);
    }

    public static void DoGetProjectStatus(Project projectToShowStatus) //review timeline
    {
        ProjectManager.GetProjectStatus(projectToShowStatus);
    }
}
