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
            passed = false;
            var result = failure.Split(new[] { ':' });
            if (result.Length == 2)
            {
                try
                {
                    penalty = Convert.ToInt32(result[0]);
                    feedback = result[1];
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
