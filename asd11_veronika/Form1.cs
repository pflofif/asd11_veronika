using System.Security.Cryptography.X509Certificates;

namespace asd11_veronika
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var indexOfSubstring = SubstringWithTime();

            if (indexOfSubstring is -1)
            {
                return;
            }

            OutputText(indexOfSubstring);
        }

        private void OutputText(int indexOfSubstring)
        {
            var (start, middle, end) = (
                richTextBox1.Text.AsSpan(0, indexOfSubstring).ToString(),
                richTextBox1.Text.AsSpan(indexOfSubstring, textBox1.Text.Length).ToString(),
                richTextBox1.Text.AsSpan(indexOfSubstring + textBox1.Text.Length).ToString()
            );

            //var (start, middle, end) = (
            //        richTextBox1.Text[..indexOfSubstring],
            //        richTextBox1.Text[indexOfSubstring..(indexOfSubstring + textBox1.Text.Length)],
            //        richTextBox1.Text[(indexOfSubstring + textBox1.Text.Length)..]
            //    );

            richTextBox1.Clear();

            richTextBox1.AppendText(start);

            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.AppendText(middle);
            richTextBox1.SelectionColor = Color.Black;

            richTextBox1.AppendText(end);
        }

        private int SubstringWithTime()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var indexOfSubstring = FindSubstring(textBox1.Text, richTextBox1.Text);
            watch.Stop();
            textBox2.Text = watch.Elapsed.ToString();

            return indexOfSubstring;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var text = richTextBox1.Text.Where(ch => !char.IsDigit(ch))
                .Aggregate(string.Empty, (current, ch) => current + ch);

            richTextBox1.Text = text;
        }
        static int[] GetPrefix(string s)
        {
            var result = new int[s.Length];
            result[0] = 0;
            int index = 0;

            for (int i = 1; i < s.Length; i++)
            {
                while (index >= 0 && s[index] != s[i]) { index--; }
                index++;
                result[i] = index;
            }

            return result;
        }

        static int FindSubstring(string pattern, string text)
        {
            int[] pf = GetPrefix(pattern);
            int index = 0;

            for (int i = 0; i < text.Length; i++)
            {
                while (index > 0 && pattern[index] != text[i]) { index = pf[index - 1]; }
                if (pattern[index] == text[i]) index++;
                if (index == pattern.Length)
                {
                    return i - index + 1;
                }
            }

            return -1;
        }
    }
}