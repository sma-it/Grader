using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRader
{
    class Parser
    {
        XmlDocument doc;
        bool valid = false;

        int totalTests = 0;
        int passedTests = 0;
        int failedTests = 0;
        int inconclusiveTests = 0;
        int skippedTests = 0;

        List<Test> tests;

        public Parser(string documentName)
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(documentName);
                valid = true;
            } catch (Exception e)
            {
                Console.WriteLine("Error while loading NUnit result file: " + e.Message);
                valid = false;
            }
        }



        public void Parse()
        {
            if (!valid) return;
            totalTests = doc.DocumentElement.Attributes["total"] != null ? Convert.ToInt32(doc.DocumentElement.Attributes["total"].Value) : 0;
            passedTests = doc.DocumentElement.Attributes["passed"] != null ? Convert.ToInt32(doc.DocumentElement.Attributes["passed"].Value) : 0;
            failedTests = doc.DocumentElement.Attributes["failed"] != null ? Convert.ToInt32(doc.DocumentElement.Attributes["failed"].Value) : 0;
            inconclusiveTests = doc.DocumentElement.Attributes["inconclusive"] != null ? Convert.ToInt32(doc.DocumentElement.Attributes["inconclusive"].Value) : 0;
            skippedTests = doc.DocumentElement.Attributes["skipped"] != null ? Convert.ToInt32(doc.DocumentElement.Attributes["skipped"].Value) : 0;

            tests = new List<Test>();

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Name == "test-suite")
                {
                    parseTestSuite(node);
                }
            }
        }

        private void parseTestSuite(XmlNode node)
        {
            foreach(XmlNode child in node)
            {
                if (child.Name == "test-suite")
                {
                    parseTestSuite(child);
                } else if (child.Name == "test-case")
                {
                    parseTests(child);
                }
            }
        }

        private void parseTests(XmlNode node)
        {
            string name = new TestName(node).Name;
            string result = node.Attributes["result"].Value;

            var test = new Test(name);
            if (result == "Passed")
            {
                test.SetPassed();
            } else
            {
                string failure = "Failed"; // should not happen
                foreach (XmlNode child in node)
                {
                    if (child.Name == "failure")
                    {
                        failure = getFailure(child);
                    }
                }
                test.SetFailed(failure);
            }

            tests.Add(test);
        }

        private string getFailure(XmlNode node)
        {
            foreach(XmlNode child in node)
            {
                if (child.Name == "message")
                {
                    return child.InnerText;
                }
            }

            return "";
        }

        public void printResult()
        {
            Console.WriteLine("total: " + totalTests);
            Console.WriteLine("passed: " + passedTests);
            Console.WriteLine("failed: " + failedTests);
            Console.WriteLine("inconclusive: " + inconclusiveTests);
            Console.WriteLine("skipped: " + skippedTests);
        }

        public bool IsConsistent()
        {
            if (passedTests + failedTests != totalTests)
            {
                Console.WriteLine("The total number of tests is not equal to the number of passed and failed tests.");
                return false;
            }
            if (tests.Count != totalTests)
            {
                Console.WriteLine("The total number of tests is not equal to the real number of tests");
                return false;
            }
            return true;
        }

        public void DisplayComments()
        {
            List<string> comments = new List<string>();

            int nameLength = getLongestTestName();

            foreach(var test in tests)
            {
                string name = test.Name;
                while (name.Length < nameLength) name += " ";

                string feedback;
                if (test.Passed)
                {
                    comments.Add(name + ": " + test.Feedback);
                }
                else 
                {
                    comments.Add(name + ": -" + test.Penalty);
                    comments.Add(test.Feedback);
                }
            }
            PrintLongComment(comments.ToArray());
        }

        private int getLongestTestName()
        {
            int longest = 0;
            foreach(var test in tests)
            {
                if(test.Name.Length > longest)
                {
                    longest = test.Name.Length;
                }
            }
            return longest;
        }

        public void DisplayGrade()
        {
            int grade = 100;
            foreach(var test in tests)
            {
                if (!test.Passed)
                {
                    grade -= test.Penalty;
                }
            }

            PrintGrade(grade);
        }

        private void PrintLongComment(string[] comment)
        {
            Console.WriteLine("<|--");
            foreach(var line in comment)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("--|>");
        }

        private void PrintShortComment(string comment)
        {
            Console.WriteLine("Comment :=>> " + comment);
        }

        private void PrintGrade(int grade)
        {
            Console.WriteLine("Grade :=>> " + grade);
        }
    }
}
