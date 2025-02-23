public abstract class Project
{
    public string ProjectName { get; protected set; }
    public string Client { get; protected set; }
    public DateTime CompletionDate { get; protected set; }
    public string ProjectID { get; protected set; }
    public List<Stage> Stages { get; protected set; } //navigatin property
    public ProjectType Type { get; protected set; }

    public Project() { } // Parameterless constructor

    public Project(string projectName, string client, DateTime completionDate, string projectID)
    {
        this.ProjectName = projectName;
        this.Client = client;
        this.CompletionDate = completionDate;
        this.ProjectID = projectID;
        this.Stages = new List<Stage>();
    }

    protected abstract void GenerateStages();

    public void SummarizeProjectStatus()
    {
        Console.WriteLine($"Project Name: {ProjectName}");
        Console.WriteLine($"Client: {Client}");
        Console.WriteLine($"Completion Date: {CompletionDate.ToShortDateString()}");
        Console.WriteLine($"Project ID: {ProjectID}");
        Console.WriteLine($"Project Type: {Type}");
        Console.WriteLine("Stages:");

        foreach (var stage in this.Stages)
        {
            string status = stage.IsCompleted ? "Completed" : "Pending";
            Console.WriteLine(
                $"- {stage.StageName}: {status} (Deadline: {stage.Deadline.ToShortDateString()})"
            );

            if (!stage.IsCompleted && DateTime.Now > stage.Deadline)
            {
                Console.WriteLine(
                    $"  WARNING: {stage.StageName} is past its deadline and not yet completed!"
                );
            }
        }
    }

    public void UpdateCompletionDate(DateTime newCompletionDate)
    {
        this.CompletionDate = newCompletionDate;
    }
}
