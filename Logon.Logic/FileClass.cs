using System;
using System.Collections.Generic;
using System.IO;

namespace Logon.Logic
{
    /// <summary>
    /// Класс работы с файлами
    /// </summary>
    public class FileClass
    {
        public string homeDirPersons;
        public string homeDirImage;

        public FileClass()
        {
            homeDirPersons = @"Persons\";
            homeDirImage = @"Resources\";
        }

        /// <summary>
        /// Чтение профилей из файла
        /// </summary>
        /// <returns>список пользователей</returns>
        public List<PersonInfo> ReadProfiles()
        {
            List<PersonInfo> person = new List<PersonInfo>();

            List<string> personsList = new List<string>();

            string[] dirs = Directory.GetFiles(homeDirPersons, "*.txt");

            for (int i = 0; i < dirs.Length ; i++)
            {

                StreamReader read = new StreamReader(dirs[i]);

                while (!read.EndOfStream)
                {
                    person.Add(new PersonInfo
                    {
                        login = read.ReadLine(),
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
        /// Запись профиля в файл
        /// </summary>
        /// <param name="person">класс пользователя
        /// </param>
        public void WriteProfile(PersonInfo person)
        {
            StreamWriter write = new StreamWriter(homeDirPersons + person.login + ".txt");

            write.WriteLine(person.login);
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

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="person"></param>
        public void DelProfile(PersonInfo person)
        {
            File.Delete(homeDirPersons + person.login + ".txt");
        }
    }
}
