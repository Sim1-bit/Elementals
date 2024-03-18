using System;

namespace GameOfYear.Logger
{
    static class Logger
    {
	    //Livelli classici del logger
        public enum Grades
        {
            Off,
            Fatal = 100,
            Error = 200,
            Warn = 300,
            Info = 400,
            Debug = 500,
            Trace = 600
        }
	
	    //Normalmente settato ad OFF, se lo si vuole usare va settato esternamente ad un livello diverso compresp tra 0 e 600
        private static Grades grade = Grades.Off;

        public static int Grade
        {
            get
            {
                return (int)grade;
            }
            set
            {
                if (value < 0)
                    grade = 0;
                else if (value > 600)
                    grade = (Grades)600;
                else
                    grade = (Grades)value;
            }
        }

	    //Se il logger non è settato ad 0 e il messaggio ha una priorità minore o uguale al grado allora procede con la stampa su linea senza a capo
        public static void Write(string Text, int grade)
        {
            if (grade < 1)
                grade = 100;
            else if (grade > 600)
                grade = 600;

            if ((int)Grade >= grade)
                Console.Write(Text);
        }
        public static void Write(string Text)
        {

            if ((int)Grade >= 600)
                Console.Write(Text);
        }
        //Se il logger non è settato ad 0 e il messaggio ha una priorità minore o uguale al grado allora procede con la stampa su linea con a capo
        public static void WriteLine(string Text, int grade)
        {
            if (grade < 1)
                grade = 100;
            else if (grade > 600)
                grade = 600;

            if ((int)Grade >= grade)
                Console.WriteLine(Text);
        }
        public static void WriteLine(string Text)
        {

            if ((int)Grade >= 600)
                Console.WriteLine(Text);
        }
    }
}
