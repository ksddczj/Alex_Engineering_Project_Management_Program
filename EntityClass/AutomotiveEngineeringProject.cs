public class AutomotiveEngineeringProject : Project
{
    public AutomotiveEngineeringProject()
        : base() { }

    public AutomotiveEngineeringProject(
        string projectName,
        string client,
        DateTime completionDate,
        string projectID
    )
        : base(projectName, client, completionDate, projectID)
    {
        GenerateStages();
        Type = ProjectType.AutomotiveEngineering;
    }

    protected override void GenerateStages()
    {
        this.Stages.Add(
            new Stage(StageName.ProjectStart, CompletionDate.AddMonths(-12), ProjectID)
        );
        this.Stages.Add(
            new Stage(StageName.ConceptCompletion, CompletionDate.AddMonths(-6), ProjectID)
        );
        this.Stages.Add(new Stage(StageName.DetailedDesignCompletion, CompletionDate, ProjectID));
    }
}
