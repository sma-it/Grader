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
            this.name = "-" + name;
        }

        private string name;
        public string Name { get => name; }

        private List<string> feedback = new List<string>();
        public List<string> Feedback { get => feedback; }

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
                    string allFeedback = failure.Substring(index + 1).Trim();
                    addFeedbackLines(allFeedback);
                } catch
                {
                    penalty = 1;
                    addFeedbackLines(failure);
                }
                
            } else
            {
                penalty = 1;
                addFeedbackLines(failure);
            }
        }

        private void addFeedbackLines(string feedbackLines)
        {
            var lines = feedbackLines.Split(new[] { '\r', '\n' });
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.Length > 0) feedback.Add(">  " + trimmed);
            }
        }

        public void SetPassed()
        {
            passed = true;
            penalty = 0;
            feedback.Add("OK");
        }
    }
}
