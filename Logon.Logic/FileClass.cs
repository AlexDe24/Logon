using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Logon.Logic
{
    public class FileClass
    {
        public List<Info> ReadProfiles()
        {
            List<Info> person = new List<Info>();

            List<string> personsList = new List<string>();

            string[] dirs = Directory.GetFiles(".../Файлы/Профили/", "*.txt");

            for (int i = 0; i < dirs.Length - 1; i++)
            {
                foreach (string dir in dirs)
                {
                    String[] substrings = dir.Split('/');

                    personsList.Add(Convert.ToString(substrings[3]));
                }

                StreamReader read = new StreamReader(".../Файлы/Профили/" + personsList[i]);

                while (read.EndOfStream)
                {
                    person.Add(new Info
                    {
                        name = read.ReadLine(),
                        surname = read.ReadLine(),
                        middlename = read.ReadLine(),
                        pasword = read.ReadLine(),
                        birthday = read.ReadLine(),
                        gender = read.ReadLine(),
                        avatarName = read.ReadLine(),
                        custom = Convert.ToInt32(read.ReadLine())
                    });
                }

                read.Close();
            }
            return person;
        }

        public void WriteProfile(Info person)
        {
            StreamWriter write = new StreamWriter(".../Файлы/Профили/" + person.name + " " + person.surname);

            write.WriteLine(person.name);
            write.WriteLine(person.surname);
            write.WriteLine(person.middlename);
            write.WriteLine(person.pasword);
            write.WriteLine(person.birthday);
            write.WriteLine(person.gender);
            write.WriteLine(person.avatarName);
            write.WriteLine(person.custom);

            write.Close();
        }
    }
}
