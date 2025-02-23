public class EngineeringDraftingProject : Project
{
    public EngineeringDraftingProject()
        : base() { }

    public EngineeringDraftingProject(
        string projectName,
        string client,
        DateTime completionDate,
        string projectID
    )
        : base(projectName, client, completionDate, projectID)
    {
        GenerateStages();
        Type = ProjectType.EngineeringDrafting;
    }

    protected override void GenerateStages()
    {
        this.Stages.Add(new Stage(StageName.ProjectStart, CompletionDate.AddMonths(-3), ProjectID));
        this.Stages.Add(new Stage(StageName.DraftingCompletion, CompletionDate, ProjectID));
    }
}
