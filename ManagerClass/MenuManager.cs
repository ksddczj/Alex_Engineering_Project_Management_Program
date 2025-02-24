public static class MenuManager //get input for menu and handle input
{
    private static class InputHandler //this is a nested helper class only accessible within MenuManager to help handle input
    {
        public static void HandleUserChoiceForMainMenu(MainMenuUserOption userChoice)
        {
            switch (userChoice)
            {
                case MainMenuUserOption.CreateNewProject:
                    TaskManager.DoCreateNewProject();
                    break;
                case MainMenuUserOption.ManageExistingProject:
                    OperateManageExistingProjectMenu();
                    break;
                case MainMenuUserOption.Exit:
                    Console.WriteLine("Quitting now......");
                    Environment.Exit(0);
                    break;
            }
        }

        public static bool HandleUserChoiceForManageExistingProjectMenu(
            ManageExistingProjectMenuOption userChoice
        )
        {
            bool stay;
            List<Project> projectsFound = new List<Project>();
            switch (userChoice)
            {
                case ManageExistingProjectMenuOption.FindProjectByName:
                    projectsFound = TaskManager.DoFindProjectByName();
                    break;

                case ManageExistingProjectMenuOption.FindProjectByClient:
                    if (ProjectManager.FindExistingClientFromDB().Any())
                    {
                        projectsFound = TaskManager.DoFindProjectByClient();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("No existing client yet.......");
                        Console.ResetColor();
                    }
                    break;

                case ManageExistingProjectMenuOption.FindProjectByID:
                    projectsFound = TaskManager.DoFindProjectByID();
                    break;

                case ManageExistingProjectMenuOption.FindProjectByCompletionDate:
                    projectsFound = TaskManager.DoFindProjectByCompletionDate();
                    break;

                case ManageExistingProjectMenuOption.FindProjectsByType:
                    projectsFound = TaskManager.DoFindProjectByType();
                    break;

                case ManageExistingProjectMenuOption.Return:
                    Console.WriteLine("Returnning to Main Menu....");
                    stay = false;
                    return stay;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            if (!projectsFound.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("No project found....");
                Console.ResetColor();
            }
            else
            {
                //display found projects
                Dictionary<int, Project> projectsMenu = MenuDisplay.DisplayItemsAsMenu(
                    projectsFound
                );

                //get user to select project to manage from the projects displayed
                Project selectedProject = InputGetter.GetSelectedProjectFromUser(projectsMenu);

                OperateProjectManagementMenu(selectedProject);
            }

            stay = true;
            return stay;
        }

        public static bool HandleUserChoiceForProjectManagementMenu(
            Project projectToManage,
            ProjectManagementMenuOption userChoice
        )
        {
            bool stay;
            switch (userChoice)
            {
                case ProjectManagementMenuOption.UpdateStageDeadline:
                    TaskManager.DoUpdateStageDeadline(projectToManage);
                    break;
                case ProjectManagementMenuOption.MarkStageAsCompleted:
                    TaskManager.DoMarkStageAsCompleted(projectToManage);
                    break;
                case ProjectManagementMenuOption.GetProjectStatus:
                    TaskManager.DoGetProjectStatus(projectToManage);
                    break;
                case ProjectManagementMenuOption.Return:
                    Console.WriteLine("Returnning to Manage Existing Project Menu....");
                    stay = false;
                    return stay;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
            stay = true;
            return stay;
        }
    }

    public static void OperateMainMenu()
    {
        while (true)
        {
            MenuDisplay.DisplayMainMenu();
            MainMenuUserOption userChoice = InputGetter.GetMainMenuOptionFromUser();
            InputHandler.HandleUserChoiceForMainMenu(userChoice);
        }
    }

    public static void OperateManageExistingProjectMenu()
    {
        bool stayInManageExistingProjectMenu = true;
        while (stayInManageExistingProjectMenu)
        {
            MenuDisplay.DisplayManageExistingProjectMenu();
            ManageExistingProjectMenuOption userChoice =
                InputGetter.GetManageExistingProjectMenuOptionFromUser();
            stayInManageExistingProjectMenu =
                InputHandler.HandleUserChoiceForManageExistingProjectMenu(userChoice);
        }
    }

    public static void OperateProjectManagementMenu(Project selectedProject)
    {
        bool stayInProjectManagement = true;
        while (stayInProjectManagement)
        {
            Console.WriteLine(
                $"Current project:  Project ID: {selectedProject.ProjectID} Project Name: {selectedProject.ProjectName}"
            );

            //get user to select from project management menu
            MenuDisplay.DisplayProjectManagementMenu();
            ProjectManagementMenuOption userChoiceForProjectManagement =
                InputGetter.GetProjectManagementMenuOptionFromUser();

            //handle user choice
            stayInProjectManagement = InputHandler.HandleUserChoiceForProjectManagementMenu(
                selectedProject,
                userChoiceForProjectManagement
            );
        }
    }
}
