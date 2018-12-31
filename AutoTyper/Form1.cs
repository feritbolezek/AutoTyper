using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace AutoTyper
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer mainTimer = new System.Timers.Timer();

        /** Amount of times the data in the textBox should be repeated. */
        private int repetitionCount;

        /** Amount of times the data in the textBox has been repeated. */
        private int currentRepetition;

        /** Amount of time before the data in the textBox is executed. */
        private int startDelay;

        /** Amount of time for the next line of data to be executed until the end of repeates. */
        private int lineDelay;

        private int currentLine;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadOptions();
            button1.Enabled = false;
            button2.Enabled = true;
            label6.Text = "Repetition count: " + currentRepetition + "/" + repetitionCount;
            mainTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mainTimer.Interval = startDelay;
            mainTimer.Start();

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mainTimer.Interval = lineDelay;

            UpdateUI();

            Write(); // {ENTER}

            if (currentRepetition == repetitionCount)
            {
                Reset();
            }
        }

        private void Reset() {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Reset(); });
                return;
            }
            mainTimer.Stop();
            currentRepetition = 0;
            button1.Enabled = true;
            button2.Enabled = false;
            mainTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
            label6.Text = "Repetition count: " + currentRepetition + "/" + repetitionCount;
        }
        private void UpdateUI() {

            // Update UI on main thread.
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { UpdateUI(); });
                return;
            }
            label6.Text = "Repetition count: " + currentRepetition + "/" + repetitionCount;
        }

        private void Write() {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Write(); });
                return;
            }
            SendKeys.SendWait(richTextBox1.Lines[currentLine]);

            currentLine++;

            if (currentLine >= richTextBox1.Lines.Length)
            {
                currentRepetition++;
                currentLine = 0;
            }
        }


        /** Read preferences from textBoxes, repetition count, start delay etc. */
        private void ReadOptions() {
            int.TryParse(textBox3.Text,out repetitionCount);
            int.TryParse(textBox2.Text, out lineDelay);
            int.TryParse(textBox1.Text, out startDelay);

            if (lineDelay < 100) lineDelay = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
