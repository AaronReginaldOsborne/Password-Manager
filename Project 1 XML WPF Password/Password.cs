using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project_1_XML_WPF_Password
{
    public class Password
    {
        // Private member variables and constants

        private enum CharType { None = -1, Letter = 0, Digit = 1, Symbol = 2, Upper = 3, Lower = 4 };
        private enum Trend { None = -1, Increasing = 0, Decreasing = 1 };
        private const int MIN_LENGTH = 8;
        private XmlDocument _doc = null;

        // Non-default, public constructor method

        public Password(XmlDocument doc)
        {
            if (doc == null)
                throw new ArgumentException("Null DOM object reference");

            _doc = doc;
        }

        public Password(string p)
        {
            _SetStrength(p);
            Value = p;
        }

        // Public properties

        public string Value { get; private set; }

        public ushort StrengthPercent { get; private set; }

        public string StrengthLabel { get; private set; }


        public void Show()
        {
            Console.WriteLine();// padding
            XmlNodeList account = _doc.GetElementsByTagName("account");
            for (var i = 0; i < account.Count; ++i)
            {
                XmlElement accountElement = account[i] as XmlElement;
                var descption = accountElement.GetElementsByTagName("description");
                Console.WriteLine($" {i + 1}. {descption[0].InnerText}");
            }
            Console.WriteLine();// padding
            Console.WriteLine("\t\tPress # frome the above list to select an entry.");
            Console.WriteLine("\t\tPress A to add a new entry.");
            Console.WriteLine("\t\tPress X to quit.\n");
        }

        public bool ShowFromList(int index)
        {
            //grab all accounts from node
            XmlNodeList account = _doc.GetElementsByTagName("account");
            // validation if in range or not
            if (index > 0 && index <= account.Count)
            {
                //grab the element data from the index
                XmlElement accountElement = account[index - 1] as XmlElement;
                //parse the data to print
                Console.WriteLine($"User ID:\t\t\t\t{accountElement.GetAttribute("user-id")}");

                //grab password element 
                var passwords = accountElement.GetElementsByTagName("password");
                var passwordElement = passwords[0] as XmlElement;

                Console.WriteLine($"Password:\t\t\t\t{passwordElement.InnerText}");
                Console.WriteLine($"Password Strength:\t\t\t{passwordElement.GetAttribute("strength")} ({passwordElement.GetAttribute("percentage")}%)");
                Console.WriteLine($"Password Updated:\t\t\t{passwordElement.GetAttribute("date")}");

                //grab login element 
                var logins = accountElement.GetElementsByTagName("login-url");
                if (logins.Count != 0)
                {
                    var login = logins[0] as XmlElement;

                    Console.WriteLine($"login-url:\t\t\t\t{login.InnerText}");
                }

                //grab login element 
                var accountNumbers = accountElement.GetElementsByTagName("account-number");
                if (accountNumbers.Count != 0)
                {
                    var accountNumber = accountNumbers[0] as XmlElement;

                    Console.WriteLine($"Account #:\t\t\t\t{accountNumber.InnerText}");
                    Console.WriteLine();//padding
                }

                Console.WriteLine("\t\tPress P to change this password.");
                Console.WriteLine("\t\tPress D to delete this entry.");
                Console.WriteLine("\t\tPress M to return to the main menu.");
                Console.WriteLine();
                Console.WriteLine("Enter a command:");
                Console.WriteLine();
                return true;
            }
            else
            {
                return false;
            }
        }


        public void AddItem(string desc, string user, string pass, string date, string log, string acc)
        {
            XmlElement account = _doc.CreateElement("account");
            account.SetAttribute("user-id", user);

            XmlElement description = _doc.CreateElement("description");
            description.InnerText = desc;
            account.AppendChild(description);

            XmlElement password = _doc.CreateElement("password");
            _SetStrength(pass);

            password.InnerText = pass;
            password.SetAttribute("strength", StrengthLabel);
            password.SetAttribute("percentage", StrengthPercent.ToString());

            //var today = DateTime.Now;
            //string xmlDate = today.Year + "-";
            //if (today.Month < 10)
            //{
            //    xmlDate += "0" + today.Month + "-";
            //}
            //else
            //{
            //    xmlDate += today.Month + "-";
            //}

            //if (today.Day < 10)
            //{
            //    xmlDate += "0" + today.Day + "-";
            //}
            //else
            //{
            //    xmlDate += today.Day;
            //}

            password.SetAttribute("date", date);
            account.AppendChild(password);

            XmlElement loginURLElement = _doc.CreateElement("login-url");
            loginURLElement.InnerText = log;
            account.AppendChild(loginURLElement);

            XmlElement accountElement = _doc.CreateElement("account-number");
            accountElement.InnerText = acc;
            account.AppendChild(accountElement);
            _doc.DocumentElement.FirstChild.AppendChild(account);
        }
        public void SaveAccount(passwordmanagerAccount acc,int index)
        {
            //get selected account
            XmlNodeList account = _doc.GetElementsByTagName("account");
            XmlElement accountElement = account[index] as XmlElement;

            accountElement.SetAttribute("user-id", acc.userid);

            //get selected description tag
            var description = accountElement.GetElementsByTagName("description");
            XmlElement descElement = description[0] as XmlElement;
            descElement.InnerText = acc.description;

            //get selected element password tag
            var passwords = accountElement.GetElementsByTagName("password");
            XmlElement passwordElement = passwords[0] as XmlElement;
            //run password helper
            _SetStrength(acc.password.Value);
            passwordElement.InnerText = acc.password.Value;
            passwordElement.SetAttribute("strength", acc.password.strength);
            passwordElement.SetAttribute("percentage", acc.password.Value);
            passwordElement.SetAttribute("date", acc.password.date);

            //get selected element login-url tag
            var loginURL = accountElement.GetElementsByTagName("login-url");
            XmlElement loginURLElement = loginURL[0] as XmlElement;
            loginURLElement.InnerText = acc.loginurl;

            //get selected element password tag
            var accountNumber = accountElement.GetElementsByTagName("account-number");
            XmlElement accountNumberElement = accountNumber[0] as XmlElement;
            accountNumberElement.InnerText = acc.accountnumber;


        }

        public passwordmanagerAccount GrabAccount(int index)
        {
            passwordmanagerAccount XMLaccount = new passwordmanagerAccount();

            XmlNodeList account = _doc.GetElementsByTagName("account");
            XmlElement accountElement = account[index] as XmlElement;

            var userID = accountElement.GetAttribute("user-id");
            XMLaccount.userid = userID;

            var descption = accountElement.GetElementsByTagName("description");
            XMLaccount.description = descption[0].InnerText;

            //grab password element 
            var passwords = accountElement.GetElementsByTagName("password");
            var passwordElement = passwords[0] as XmlElement;

            passwordmanagerAccountPassword tempPassword = new passwordmanagerAccountPassword();

            tempPassword.Value = passwordElement.InnerText.ToString();
            tempPassword.strength = passwordElement.GetAttribute("strength");
            tempPassword.percentage = passwordElement.GetAttribute("percentage");
            tempPassword.date = passwordElement.GetAttribute("date");

            XMLaccount.password = tempPassword;

            //grab login element 
            var logins = accountElement.GetElementsByTagName("login-url");
            if (logins.Count != 0)
            {
                var login = logins[0] as XmlElement;
                XMLaccount.loginurl = login.InnerText;
            }

            //grab login element 
            var accountNumbers = accountElement.GetElementsByTagName("account-number");
            if (accountNumbers.Count != 0)
            {
                var accountNumber = accountNumbers[0] as XmlElement;
                XMLaccount.accountnumber = accountNumber.InnerText;
            }
            return XMLaccount;

        }

            public void ChangePassword(int index)
        {
            Console.Write("New Password:");
            string password = Console.ReadLine();

            //get selected account
            XmlNodeList account = _doc.GetElementsByTagName("account");
            XmlElement accountElement = account[index - 1] as XmlElement;

            //get selected element password tag
            var passwords = accountElement.GetElementsByTagName("password");
            XmlElement passwordElement = passwords[0] as XmlElement;

            //run password helper
            _SetStrength(password);
            passwordElement.InnerText = password;
            passwordElement.SetAttribute("strength", StrengthLabel);
            passwordElement.SetAttribute("percentage", StrengthPercent.ToString());

            //easy way
            //string xmlDate = DateTime.Now.ToString("yyyy-MM-dd");

            //change date
            var today = DateTime.Now;
            string xmlDate = today.Year + "-";
            if (today.Month < 10)
            {
                xmlDate += "0" + today.Month + "-";
            }
            else
            {
                xmlDate += today.Month + "-";
            }

            if (today.Day < 10)
            {
                xmlDate += "0" + today.Day + "-";
            }
            else
            {
                xmlDate += today.Day;
            }

            passwordElement.SetAttribute("date", xmlDate);
        }

        public bool DeleteItem(int index)
        {
            XmlNodeList allItems = _doc.GetElementsByTagName("account");
            if (index < 1 || allItems.Count < index)
                return false;

            _doc.DocumentElement.FirstChild.RemoveChild(allItems[index - 1]);
            return true;
        }


        // Private helper methods
        private void _SetStrength(string pwd)
        {
            /*
             * Purpose:   Calculates the 'strength' of a given password where 0% is the weakest
             *            and 100% is the strongest.
             * Accepts:   The password for analysis as a string
             * Returns:   An int which is the strength as a percentage
             */

            if (pwd.Any(x => Char.IsWhiteSpace(x)))
                throw new ArgumentException("ERROR: Passwords may not contain whitespace characters.");

            int score = 0, midNumOrSym = 0, sequenceLength = 0, repeatChars = 0;
            int[] basicCount = { 0, 0, 0, 0, 0 };
            int[] consecutiveCount = { 0, 0, 0, 0, 0 };
            int[] sequentialCount = { 0, 0, 0, 0 };
            char lastChar = ' ';
            CharType type = CharType.None, lastType = CharType.None, sequenceType = CharType.None, lastSequenceType = CharType.None;
            Trend sequenceTrend = Trend.None, lastSequenceTrend = Trend.None;
            int uniqueCount = 0, repeatCount = 0;
            double repeatDeduct = 0;

            // Get counts required for scoring
            for (int i = 0; i < pwd.Count(); i++)
            {
                char c = pwd[i];
                if (Char.IsLetter(c))
                {
                    basicCount[(int)CharType.Letter]++;
                    if (Char.IsUpper(c))
                        type = CharType.Upper;
                    else
                        type = CharType.Lower;
                    sequenceType = CharType.Letter;
                }
                else
                {
                    if (Char.IsNumber(c))
                        type = CharType.Digit;
                    else
                        type = CharType.Symbol;
                    sequenceType = type;
                }

                basicCount[(int)type]++;
                consecutiveCount[(int)type] += (type == lastType) ? 1 : 0;
                repeatChars += (Char.ToLower(c) == Char.ToLower(lastChar)) ? 1 : 0;

                if (c - lastChar == 1)
                    sequenceTrend = Trend.Increasing;
                else if (lastChar - c == 1)
                    sequenceTrend = Trend.Decreasing;
                else
                    sequenceTrend = Trend.None;

                if (sequenceTrend != Trend.None && sequenceType == lastSequenceType && (sequenceTrend == lastSequenceTrend || lastSequenceTrend == Trend.None))
                {
                    sequenceLength++;
                    //if (sequenceLength >= 3 && sequenceLength > longestSequence[(int)sequenceType])
                    if (sequenceLength == 3)
                    {
                        sequentialCount[(int)sequenceType]++;
                        //longestSequence[(int)sequenceType] = sequenceLength;
                    }
                }
                else
                {
                    sequenceTrend = Trend.None;
                    sequenceLength = 1;
                }

                // If character is repeated, adjust repetition deduction
                bool repeated = false;
                for (int j = 0; j < pwd.Length; j++)
                {
                    if (i != j && pwd[i] == pwd[j])
                    {
                        repeated = true;
                        //if (j < i)
                        repeatDeduct += Math.Abs((double)pwd.Length / (j - i));
                    }
                }
                if (repeated)
                {
                    repeatCount++;
                    uniqueCount = pwd.Length - repeatCount;
                    repeatDeduct = (uniqueCount == 0) ? Math.Ceiling(repeatDeduct) : Math.Ceiling(repeatDeduct / (double)uniqueCount);
                }

                // Reinitialize for next character in password
                lastChar = c;
                lastType = type;
                lastSequenceType = sequenceType;
                lastSequenceTrend = sequenceTrend;
            }

            repeatDeduct = (uniqueCount == 0) ? repeatCount : (int)Math.Ceiling((double)repeatCount / uniqueCount);

            midNumOrSym = basicCount[(int)CharType.Digit] + basicCount[(int)CharType.Symbol] - (Char.IsLetter(pwd.First()) ? 0 : 1) - (Char.IsLetter(pwd.Last()) ? 0 : 1);

            // Calculate score

            // + Number of characters
            score = pwd.Length * 4;
            //Console.WriteLine("Number of Characters: " + pwd.Length * 4);

            // + Uppercase letters
            score += (pwd.Length - basicCount[(int)CharType.Upper]) * 2;
            //Console.WriteLine("Uppercase Letters: " + (pwd.Length - basicCount[(int)CharType.Upper]) * 2);

            // + Lowercase letters
            score += (pwd.Length - basicCount[(int)CharType.Lower]) * 2;
            //Console.WriteLine("Lowercase Letters: " + (pwd.Length - basicCount[(int)CharType.Lower]) * 2);

            // + Digits
            score += basicCount[(int)CharType.Digit] * 4;
            //Console.WriteLine("Lowercase Letters: " + basicCount[(int)CharType.Digit] * 4);

            // + Symbols
            score += basicCount[(int)CharType.Symbol] * 6;
            //Console.WriteLine("Lowercase Letters: " + basicCount[(int)CharType.Symbol] * 6);

            // + Middle digits or symbols
            int mid = basicCount[(int)CharType.Digit] + basicCount[(int)CharType.Symbol];
            mid -= (Char.IsLetter(pwd.First()) ? 0 : 1) + (Char.IsLetter(pwd.Last()) ? 0 : 1);
            score += mid * 2;
            //Console.WriteLine("Middle Digits or Symbols: " + mid * 2);

            // + Other requirements
            int req = 0;
            req += (pwd.Length < MIN_LENGTH) ? 0 : 1;
            req += basicCount[(int)CharType.Lower] > 0 ? 1 : 0;
            req += basicCount[(int)CharType.Upper] > 0 ? 1 : 0;
            req += basicCount[(int)CharType.Digit] > 0 ? 1 : 0;
            req += basicCount[(int)CharType.Symbol] > 0 ? 1 : 0;
            req = req < 4 ? 0 : req * 2;
            score += req;
            //Console.WriteLine("Requirements: " + req);

            // - Letters only
            score -= (pwd.Length == basicCount[(int)CharType.Letter]) ? pwd.Length : 0;
            //Console.WriteLine("Letters Only: " + ((pwd.Length == basicCount[(int)CharType.Letter]) ? -pwd.Length : 0));

            // - Digits only
            score -= (pwd.Length == basicCount[(int)CharType.Digit]) ? pwd.Length : 0;
            //Console.WriteLine("Numbers Only: " + ((pwd.Length == basicCount[(int)CharType.Digit]) ? -pwd.Length : 0));

            // - Repeat characters
            score = (int)(score - repeatDeduct);
            //Console.WriteLine("Repeat Characters: " + -(int)repeatDeduct);

            // - Consecutive uppercase letters
            score -= consecutiveCount[(int)CharType.Upper] * 2;
            //Console.WriteLine("Consecutive Uppercase Letters: " + -consecutiveCount[(int)CharType.Upper] * 2);

            // - Consecutive lowercase letters
            score -= consecutiveCount[(int)CharType.Lower] * 2;
            //Console.WriteLine("Consecutive Lowercase Letters: " + -consecutiveCount[(int)CharType.Lower] * 2);

            // - Consecutive digits
            score -= consecutiveCount[(int)CharType.Digit] * 2;
            //Console.WriteLine("Consecutive Digits: " + -consecutiveCount[(int)CharType.Digit] * 2);

            // - Sequential letters
            score -= sequentialCount[(int)CharType.Letter] * 3;
            //Console.WriteLine("Sequential Letters: " + -sequentialCount[(int)CharType.Letter] * 3);

            // - Sequential digits
            score -= sequentialCount[(int)CharType.Digit] * 3;
            //Console.WriteLine("Sequential Digits: " + -sequentialCount[(int)CharType.Digit] * 3);

            // - Sequential letters
            score -= sequentialCount[(int)CharType.Symbol] * 3;
            //Console.WriteLine("Sequential Symbols: " + -sequentialCount[(int)CharType.Symbol] * 3);

            // Prevent exceeding the expected value range
            score = Math.Min(Math.Max(0, score), 100);

            // Initialize the Password's public properties
            StrengthPercent = (ushort)score;
            if (score < 20)
                StrengthLabel = "very weak";
            else if (score < 40)
                StrengthLabel = "weak";
            else if (score < 60)
                StrengthLabel = "good";
            else if (score < 80)
                StrengthLabel = "strong";
            else
                StrengthLabel = "very strong";

        } // end _SetStrength()

    } // end class
}
