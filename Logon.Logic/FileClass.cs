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

            string[] dirs = Directory.GetFiles(@"C:\Logon\", "*.txt");

            for (int i = 0; i < dirs.Length ; i++)
            {

                StreamReader read = new StreamReader(dirs[i]);

                while (!read.EndOfStream)
                {
                    person.Add(new Info
                    {
                        name = read.ReadLine(),
                        surname = read.ReadLine(),
                        middlename = read.ReadLine(),
                        password = read.ReadLine(),
                        birthday = read.ReadLine(),
                        gender = read.ReadLine(),
                        avatarAddres = read.ReadLine(),
                        custom = Convert.ToInt32(read.ReadLine())
                    });
                }

                read.Close();
            }
            return person;
        }

        public void WriteProfile(Info person)
        {
            StreamWriter write = new StreamWriter(@"C:\Logon\" + person.name + person.surname + ".txt");

            write.WriteLine(person.name);
            write.WriteLine(person.surname);
            write.WriteLine(person.middlename);
            write.WriteLine(person.password);
            write.WriteLine(person.birthday);
            write.WriteLine(person.gender);
            write.WriteLine(person.avatarAddres);
            write.WriteLine(person.custom);

            write.Close();
        }

        public void ImageLoad()
        {

        }
    }
}
