public class Stage
{
    public StageName StageName { get; private set; }
    public DateTime Deadline { get; private set; }
    public bool IsCompleted { get; private set; }
    public string ProjectID { get; private set; } //foreign key not necessary in entity class

    // public Stage() { }

    public Stage(StageName stageName, DateTime deadline, string projectID)
    {
        this.StageName = stageName;
        this.Deadline = deadline;
        this.IsCompleted = false;
        this.ProjectID = projectID;
    }

    public void MarkAsCompleted()
    {
        this.IsCompleted = true;
    }

    public void UpdateDeadline(DateTime newDeadline)
    {
        this.Deadline = newDeadline;
    }
}
