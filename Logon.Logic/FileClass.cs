using System;
using System.Collections.Generic;
using System.IO;

namespace Logon.Logic
{
    public class FileClass
    {
        public string homeDir;

        public FileClass()
        {
            homeDir = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Logon\";
        }
        /// <summary>
        /// Чтение пользователей из файла
        /// </summary>
        /// <returns>список пользователей</returns>
        public List<PersonInfo> ReadProfiles()
        {
            List<PersonInfo> person = new List<PersonInfo>();

            List<string> personsList = new List<string>();

            string[] dirs = Directory.GetFiles(homeDir, "*.txt");

            for (int i = 0; i < dirs.Length ; i++)
            {

                StreamReader read = new StreamReader(dirs[i]);

                while (!read.EndOfStream)
                {
                    person.Add(new PersonInfo
                    {
                        name = read.ReadLine(),
                        surname = read.ReadLine(),
                        middlename = read.ReadLine(),
                        password = read.ReadLine(),
                        birthDateDay = read.ReadLine(),
                        birthDateMonth = read.ReadLine(),
                        birthDateYear = read.ReadLine(),
                        gender = read.ReadLine(),
                        avatarAddres = read.ReadLine(),
                    });
                }

                read.Close();
            }
            return person;
        }

        /// <summary>
        /// Запись пользователя в файл
        /// </summary>
        /// <param name="person">класс пользователя
        /// </param>
        public void WriteProfile(PersonInfo person)
        {
            StreamWriter write = new StreamWriter(homeDir + person.name + person.surname + ".txt");

            write.WriteLine(person.name);
            write.WriteLine(person.surname);
            write.WriteLine(person.middlename);
            write.WriteLine(person.password);
            write.WriteLine(person.birthDateDay);
            write.WriteLine(person.birthDateMonth);
            write.WriteLine(person.birthDateYear);
            write.WriteLine(person.gender);
            write.WriteLine(person.avatarAddres);

            write.Close();
        }

        public void DoCopyIn()
        {
            //File.Copy(HomeDir, "Logon");
        }
    }
}
