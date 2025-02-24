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
        Stage stageToUpdate = projectToUpdate.Stages.Single(s =>
            s.StageName == nameOfTheStageToUpdate
        );
        stageToUpdate.UpdateDeadline(newDeadline);
        Console.WriteLine(
            $"Deadline for {nameOfTheStageToUpdate} updated to {newDeadline.ToShortDateString()}."
        );

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

    public static void MarkProjectStageCompleted(Project project, StageName stageName)
    {
        Stage stageToUpdate = project.Stages.Where(s => s.StageName == stageName).First();
        stageToUpdate.MarkAsCompleted();
        Console.WriteLine($"Deadline for {stageName} marked as completed.");

        // Use a new DbContext instance to save the changes
        using (var context = new DatabaseContext())
        {
            // Re-attach the project entity
            context.Projects.Attach(project);

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
}
