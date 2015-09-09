using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Text.RegularExpressions;
using System.Windows.Forms;
using Aptima.Asim.DDD.CommonComponents.PasswordHashing;

namespace Aptima.Asim.DDD.CommonComponents.UserTools
{
    public class Authenticator
    {
        public const Boolean EnablePassword = true;

        private static string fileName = "passwords.txt";
        public static int GetNumUsers()
        {
            return userTable.Count;
        }

        public static bool Authenticate(string username, string password)
        {
            if (userTable.ContainsKey(username))
            {
                if (userTable[username].Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        public static void LoadUserFile()
        {
            Regex regex = new Regex(@"^<Username>(.*)</Username><Passwords>(.*)</Passwords>$");
            Regex passregex = new Regex(@"<Password>(.*?)</Password>");
            userTable.Clear();
            User u;
            string username;
            List<string> passwords;
            string line;
            try
            {
                string path = String.Format("{0}\\{1}", Application.StartupPath, fileName);

                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    passwords = new List<string>();
                    line = sr.ReadLine();
                    Match m = regex.Match(line);
                    if (m.Success)
                    {
                        username = m.Groups[1].Value;
                        string s = m.Groups[2].Value;
                        foreach (Match m2 in passregex.Matches(s))
                        {
                            passwords.Add(m2.Groups[1].Value);
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format("Password file: \"{0}\" invalid",fileName));
                    }
                    //string[] values = line.Split(' ');
                    //username = sr.ReadLine();
                    //password = sr.ReadLine();

                    u = new User(username);
                    u.passwords = passwords;
                    userTable[u.Username] = u;
                }

                sr.Close();
                fs.Close();
            }
            catch (System.IO.FileNotFoundException)
            {

            }
        }

        public static void WriteUserFile()
        {
            //FileStream fs = new FileStream(fileName, FileAccess.Write);
            string path = String.Format("{0}\\{1}", Application.StartupPath, fileName);
            StreamWriter sw = new StreamWriter(path, false);

            
            foreach (User u in userTable.Values)
            {
                StringBuilder b = new StringBuilder();
                b.Append(String.Format("<Username>{0}</Username>", u.username));
                b.Append("<Passwords>");
                foreach (string p in u.passwords)
                {
                    b.Append(String.Format("<Password>{0}</Password>", p));
                }

                b.Append("</Passwords>");
                sw.WriteLine(b.ToString());
            }

            sw.Close();
        }

        public static List<User> GetUsers()
        {
            return new List<User>(userTable.Values);
        }
        public static void SetUsers(List<User> users)
        {
            userTable.Clear();
            foreach (User u in users)
            {
                userTable[u.username] = u;
            }
        }

        private static Dictionary<string, User> userTable = new Dictionary<string, User>();


        public static bool IsStrongPassword(string username, string password)
        {
            // 2+ punctuation
            Regex punctuationRegex2 = new Regex(@"\p{P}");
            MatchCollection c;
            c = punctuationRegex2.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // Minimum of 10 characters
            if (password.Length < 10)
            {
                return false;
            }

            // 2+ numbers
            Regex numbersRegex = new Regex(@"[0-9]");
            c = numbersRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ uppercase letters
            Regex uppercaseRegex = new Regex(@"[A-Z]");
            c = uppercaseRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ lowercase letters
            Regex lowercaseRegex = new Regex(@"[a-z]");
            c = lowercaseRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ punctuation
            Regex punctuationRegex = new Regex(@"\p{P}");
            c = punctuationRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // Cannot re-use the last 10 passwords

            if (userTable.ContainsKey(username))
            {
                if (userTable[username].passwords.Contains(PasswordHashUtility.HashPassword(password)))
                {
                    return false;
                }
            }

            



            return true;
        }

        public static bool IsStrongPassword(User user, string password)
        {
            // 2+ punctuation
            Regex punctuationRegex2 = new Regex(@"\p{P}");
            MatchCollection c;
            c = punctuationRegex2.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // Minimum of 10 characters
            if (password.Length < 10)
            {
                return false;
            }

            // 2+ numbers
            Regex numbersRegex = new Regex(@"[0-9]");
            c = numbersRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ uppercase letters
            Regex uppercaseRegex = new Regex(@"[A-Z]");
            c = uppercaseRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ lowercase letters
            Regex lowercaseRegex = new Regex(@"[a-z]");
            c = lowercaseRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // 2+ punctuation
            Regex punctuationRegex = new Regex(@"\p{P}");
            c = punctuationRegex.Matches(password);
            if (c.Count < 2)
            {
                return false;
            }

            // Cannot re-use the last 10 passwords
            if (user.passwords.Contains(PasswordHashUtility.HashPassword(password)))
            {
                return false;
            }
            
            

            return true;
        }
    }



    public class User
    {
        internal string username;
        internal List<string> passwords;

        public User(string user)
        {
            username = user;
            passwords = new List<string>();
        }

        override public string ToString()
        {
            return username;
        }

        public bool PasswordUsed(string pass)
        {
            if (passwords.Contains(pass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return passwords[0];
            }
            set
            {
                passwords.Insert(0, value);
                if (passwords.Count > 10)
                {
                    passwords.RemoveAt(10);
                }
            }
        }
    }
}
