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
            List<PersonInfo> persons = new List<PersonInfo>();

            List<string> personsList = new List<string>();

            string[] dirs = Directory.GetFiles(homeDirPersons, "*.txt");

            for (int i = 0; i < dirs.Length ; i++)
            {

                StreamReader read = new StreamReader(dirs[i]);

                while (!read.EndOfStream)
                {
                    persons.Add(new PersonInfo
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
            return persons;
        }

        /// <summary>
        /// Запись профиля в файл
        /// </summary>
        /// <param name="person">профиль</param>
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

        /// <summary>
        /// Удаление сохранённого профиля
        /// </summary>
        /// <param name="person"></param>
        public void IsLogonFalse()
        {
            File.Delete(homeDirPersons + @"Online\Online.txt");
        }

        /// <summary>
        /// Чтение сохранённого профиля
        /// </summary>
        /// <returns></returns>
        public PersonInfo IsLogonRead()
        {
            StreamReader read = new StreamReader(homeDirPersons + @"Online\Online.txt");

            PersonInfo person = new PersonInfo();

            while (!read.EndOfStream)
            {
                person.login = read.ReadLine();
                person.name = read.ReadLine();
                person.surname = read.ReadLine();
                person.middlename = read.ReadLine();
                person.password = read.ReadLine();
                person.birthDateDay = read.ReadLine();
                person.birthDateMonth = read.ReadLine();
                person.birthDateYear = read.ReadLine();
                person.gender = read.ReadLine();
                person.avatarAddres = read.ReadLine();
            }

            read.Close();

            return person;
        }

        /// <summary>
        /// Запись сохранённого профиля
        /// </summary>
        /// <param name="person">профиль</param>
        public void IsLogonWrite(PersonInfo person)
        {
            StreamWriter write = new StreamWriter(homeDirPersons + @"Online\Online.txt");

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
    }
}
