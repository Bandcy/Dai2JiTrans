using RomLibrary;
using RomLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dai2JiTrans
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public byte[] Bytes { get; set; }
        public TBL TBL { get; set; }
        public string HexValue { get; set; }
        public string ThingyValue { get; set; }
        public string Output
        {
            get { return txtOutput.Text; }
            set { txtOutput.Text = value; }
        }

        private void OpenFile()
        {
            string file = OpenFileDialog();
            if (string.IsNullOrEmpty(file))
                return;

            Bytes = ByteHelper.ReadFile(file);
            PrintHexCode();
            //PrintScript();
        }

        private void PrintHexCode()
        {
            if (Bytes == null)
                return;

            HexValue = Bytes.AsHexMerged();
        }

        private void PrintScript()
        {
            if (Bytes == null)
                return;

            ThingyValue = TBL.ApplyTBL(Bytes);
        }

        private void ExportScript(int start, int end)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file|*.txt";
            dialog.ShowDialog();

            string file = dialog.FileName;
            if (string.IsNullOrEmpty(file))
                return;

            DumpFile(file, start, end);
        }

        private void DumpFile(string file, int start, int end, string endSign = @"\End")
        {
            int length = end - start;
            IEnumerable<byte> target = Bytes.Skip(start).Take(length).ToArray();
            string result = TBL.ApplyTBL(target.ToArray());

            var lines = result.Split(new string[] { endSign }, StringSplitOptions.RemoveEmptyEntries);

            using (StreamWriter writer = File.CreateText(file))
            {
                lines.ToList()
                    .ForEach(l => writer.WriteLine(l));
                writer.Close();
            }
        }

        private string OpenFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            return dialog.FileName;
        }

        private void Modify()
        {
            // Pointer table

            // Script
            // '마즈와 ' = '1FCE1AAF'
            // '사키와 ' = '0B071AAF'
            string keyword = "1FCE1AAF";
            string replaced = "0B071AAF";

            int index = Bytes.IndexOf(replaced);
            Bytes.Replace(keyword.AsByteArray(), index);
        }

        private void SetTargetFileName()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            Modify();
            ByteHelper.WriteFile(dialog.FileName, Bytes);
        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void openTBLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "TBL file|*.tbl";
            dialog.ShowDialog();

            string file = dialog.FileName;
            if (string.IsNullOrEmpty(file))
                return;

            this.TBL = new RomLibrary.TBL(file);
            TBLManagerForm viewer = new TBLManagerForm(file);
            viewer.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exportScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportScript(131376, 145859);
        }
    }
}
