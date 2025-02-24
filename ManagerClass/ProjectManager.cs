using System.Linq;
using Microsoft.EntityFrameworkCore;

public static class ProjectManager
{
    // private static List<Project> projects = new List<Project>();
    // private static List<string> existingIDs = new List<string>();
    // public static List<string> ExistingClients { get; private set; } = new List<string>();

    public static void CreateNewProject(
        string projectName,
        string client,
        ProjectType projectType,
        DateTime completionDate
    )
    {
        Project newProject;
        string newProjectID = GenerateProjectID();

        switch (projectType)
        {
            case ProjectType.AutomotiveEngineering:
                newProject = new AutomotiveEngineeringProject(
                    projectName,
                    client,
                    completionDate,
                    newProjectID
                );
                break;
            case ProjectType.EngineeringDrafting:
                newProject = new EngineeringDraftingProject(
                    projectName,
                    client,
                    completionDate,
                    newProjectID
                );
                break;
            default:
                throw new ArgumentException("Invalid project type");
        }

        //save the newly created project to DB instead of projects(list) which does not exist anymore
        // projects.Add(newProject);

        using (var context = new DatabaseContext())
        {
            context.Projects.Add(newProject); // Add the new project to the DbSet
            context.SaveChanges(); // Save changes to the database
        }
    }

    private static string GenerateProjectID() // review the logic!!!! I need to look into some details
    {
        using (var context = new DatabaseContext())
        {
            // Retrieve the list of existing project IDs from the database
            var existingIDs = context.Projects.Select(p => p.ProjectID).ToList();

            int newIDNumber = 1;

            if (existingIDs.Count > 0)
            {
                string lastID = existingIDs.Last();
                int lastIDNumber = int.Parse(lastID.Substring(1));
                newIDNumber = lastIDNumber + 1;
            }

            string newID = $"P{newIDNumber:D3}";
            existingIDs.Add(newID);

            return newID;
        }
    }

    public static void UpdateProjectStageDeadline(
        Project projectToUpdate,
        StageName nameOfTheStageToUpdate,
        DateTime newDeadline
    )
    {
        //fetch the stage to be updated
        Stage stageToUpdate = projectToUpdate.Stages.Single(s =>
            s.StageName == nameOfTheStageToUpdate
        );

        //judge if the new dealine contradicates other stages' deadline
        bool deadlineIsValid = ValidateStageDeadline(newDeadline, projectToUpdate, stageToUpdate);

        if (deadlineIsValid)
        {
            stageToUpdate.UpdateDeadline(newDeadline);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                $"Deadline for {nameOfTheStageToUpdate} updated to {newDeadline.ToShortDateString()}."
            );
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Deadline for {nameOfTheStageToUpdate} is not updated.");
            Console.ResetColor();
            return;
        }

        using (var context = new DatabaseContext())
        {
            // Re-attach the project entity
            context.Projects.Attach(projectToUpdate);

            // Mark the stage entity as modified
            context.Entry(stageToUpdate).State = EntityState.Modified;

            if (
                nameOfTheStageToUpdate == StageName.DraftingCompletion
                || nameOfTheStageToUpdate == StageName.DetailedDesignCompletion
            )
            {
                projectToUpdate.UpdateCompletionDate(newDeadline);
                // Mark only the CompletionDate property as modified
                context.Entry(projectToUpdate).Property(p => p.CompletionDate).IsModified = true;
            }

            // Save the changes to the database
            context.SaveChanges();
        }
    }

    public static void MarkProjectStageCompleted(
        Project projectToUpdate,
        StageName nameOfTheStageToUpdate
    )
    {
        //fetch stage to update
        Stage stageToUpdate = projectToUpdate.Stages.Single(s =>
            s.StageName == nameOfTheStageToUpdate
        );

        //judge if the stage to be market completed does not have any stages before it marekd uncompleted.
        bool stageCanBeMarkedCompleted = ValidateStageIsCompletedFlag(
            projectToUpdate,
            stageToUpdate
        );

        if (stageCanBeMarkedCompleted)
        {
            stageToUpdate.MarkAsCompleted();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Deadline for {nameOfTheStageToUpdate} marked as completed.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Deadline for {nameOfTheStageToUpdate} is not marked as completed.");
            Console.ResetColor();
            return;
        }

        // Use a new DbContext instance to save the changes
        using (var context = new DatabaseContext())
        {
            // Re-attach the project entity
            context.Projects.Attach(projectToUpdate);

            // Mark the stage entity as modified
            context.Entry(stageToUpdate).State = EntityState.Modified;

            // Save the changes to the database
            context.SaveChanges();
        }
    }

    public static void GetProjectStatus(Project project)
    {
        project.SummarizeProjectStatus();
    }

    public static List<Project> FindProjectsByName(string projectName)
    {
        using (var context = new DatabaseContext())
        {
            Console.WriteLine("Finding projects by name...");
            List<Project> projectsFound;
            projectsFound = context
                .Projects.Include(p => p.Stages)
                .Where(p => p.ProjectName.Contains(projectName))
                .ToList();
            //need to ignore case review later
            return projectsFound;
        }
    }

    public static List<Project> FindProjectsByClient(string client)
    {
        using (var context = new DatabaseContext())
        {
            Console.WriteLine("Finding projects by client...");
            List<Project> projectsFound;
            projectsFound = context
                .Projects.Include(p => p.Stages)
                .Where(p => string.Equals(p.Client, client))
                .ToList();
            return projectsFound;
        }
    }

    public static List<Project> FindProjectsByID(string projectID)
    {
        using (var context = new DatabaseContext())
        {
            Console.WriteLine("Finding projects by ID...");
            List<Project> projectsFound;
            projectsFound = context
                .Projects.Include(p => p.Stages)
                .Where(p =>
                    string.Equals(p.ProjectID, projectID) //review case sensitive
                )
                .ToList();
            return projectsFound;
        }
    }

    public static List<Project> FindProjectsByCompletionDate(DateTime projectCompletionDate)
    {
        using (var context = new DatabaseContext())
        {
            Console.WriteLine("Finding projects by completion date...");
            List<Project> projectsFound;
            projectsFound = context
                .Projects.Include(p => p.Stages)
                .Where(p => p.CompletionDate == projectCompletionDate)
                .ToList();
            return projectsFound;
        }
    }

    public static List<Project> FindProjectsByType(ProjectType projectType)
    {
        using (var context = new DatabaseContext())
        {
            Console.WriteLine("Finding projects by type...");
            List<Project> projectsFound;
            projectsFound = context
                .Projects.Include(p => p.Stages)
                .Where(p => p.Type == projectType)
                .ToList();
            return projectsFound;
        }
    }

    // public static void AddClient(string newClientName) // not needed
    // {
    //     ExistingClients.Add(newClientName);
    // }

    public static List<string> FindExistingClientFromDB()
    {
        using (var context = new DatabaseContext())
        {
            // Query the database to find all unique client names
            var clientNames = context.Projects.Select(p => p.Client).Distinct().ToList();

            return clientNames;
        }
    }

    private static bool ValidateStageDeadline(
        DateTime deadLineToValidate,
        Project project,
        Stage stage
    )
    {
        if (project.Type.Equals(ProjectType.AutomotiveEngineering))
        {
            switch (stage.StageName)
            {
                case StageName.ProjectStart:
                    if (
                        deadLineToValidate
                        > project
                            .Stages.Single(s => s.StageName.Equals(StageName.ConceptCompletion))
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be later than the preceding stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case StageName.ConceptCompletion:
                    if (
                        deadLineToValidate
                        < project
                            .Stages.Single(s => s.StageName.Equals(StageName.ProjectStart))
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be earlier than the previous stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else if (
                        deadLineToValidate
                        > project
                            .Stages.Single(s =>
                                s.StageName.Equals(StageName.DetailedDesignCompletion)
                            )
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be later than the preceding stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case StageName.DetailedDesignCompletion:
                    if (
                        deadLineToValidate
                        < project
                            .Stages.Single(s => s.StageName.Equals(StageName.ConceptCompletion))
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be earlier than the previous stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }

        if (project.Type.Equals(ProjectType.EngineeringDrafting))
        {
            switch (stage.StageName)
            {
                case StageName.ProjectStart:
                    if (
                        deadLineToValidate
                        > project
                            .Stages.Single(s => s.StageName.Equals(StageName.DraftingCompletion))
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be later than the preceding stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case StageName.DraftingCompletion:
                    if (
                        deadLineToValidate
                        < project
                            .Stages.Single(s => s.StageName.Equals(StageName.ProjectStart))
                            .Deadline
                    )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Invalid deadline! New deadline for the current stage can not be earlier than the previous stage!"
                        );
                        Console.ResetColor();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }
        return true;
    }

    private static bool ValidateStageIsCompletedFlag(Project project, Stage stage) //judge if any prior stage is not completed
    {
        switch (stage.StageName)
        {
            case StageName.ProjectStart:
                return true;
            case StageName.ConceptCompletion:
                if (
                    project
                        .Stages.Single(s => s.StageName.Equals(StageName.ProjectStart))
                        .IsCompleted
                )
                {
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "The stage to be marked as completed can not have a previous stage that is not commpleted."
                    );
                    Console.ResetColor();
                    return false;
                }
            case StageName.DetailedDesignCompletion:
                if (
                    project
                        .Stages.Single(s => s.StageName.Equals(StageName.ConceptCompletion))
                        .IsCompleted
                )
                {
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "The stage to be marked as completed can not have a previous stage that is not commpleted."
                    );
                    Console.ResetColor();
                    return false;
                }
            case StageName.DraftingCompletion:
                if (
                    project
                        .Stages.Single(s => s.StageName.Equals(StageName.ProjectStart))
                        .IsCompleted
                )
                {
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "The stage to be marked as completed can not have a previous stage that is not commpleted."
                    );
                    Console.ResetColor();
                    return false;
                }
            default:
                return true;
        }
    }

    public static bool CheckProjectStageIsCompleted(
        Project projectToUpdate,
        StageName nameOfTheStageToUpdate
    )
    {
        if (
            projectToUpdate
                .Stages.Single(s => s.StageName.Equals(nameOfTheStageToUpdate))
                .IsCompleted
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
