namespace TaskBoardApp.Data
{
    public static class DataConstants
    {
        //Task
        public static class TaskConstants
        {
            //Title
            public const int TitleMaxLength = 70;
            public const int TitleMinLength = 5;
            //Description
            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 10;
        }

        public static class BoardConstants
        {
            //Name
            public const int NameMaxLength = 30;
            public const int NameMinLength = 3;
        }



    }
}
