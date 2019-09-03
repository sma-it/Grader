using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRader
{
    class Test
    {
        public Test(string name)
        {
            this.name = name;
        }

        private string name;
        public string Name { get => name; }

        private string feedback;
        public string Feedback { get => feedback; }

        private int penalty;
        public int Penalty { get => penalty; }

        private bool passed = false;
        public bool Passed { get => passed; }

        public void SetFailed(string failure)
        {
            failure = failure.Trim();
            passed = false;
            var result = failure.Split(new[] { ':' });
            int index = failure.IndexOf(':');
            
            if (result.Length != -1)
            {
                try
                {
                    string penaltyString = failure.Substring(0, index);
                    penalty = Convert.ToInt32(penaltyString);
                    feedback = "  " + failure.Substring(index + 1).Trim();
                } catch
                {
                    penalty = 1;
                    feedback = failure;
                }
                
            } else
            {
                penalty = 1;
                feedback = failure;
            }
        }

        public void SetPassed()
        {
            passed = true;
            penalty = 0;
            feedback = "OK";
        }
    }
}
