using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskbarSampleExt
{
    [Guid("B042F593-F406-45A7-8C4A-43DCA5786180")]
    [DeskBandInfo("Beispiel Erweiterung", "Diese ist eine Beispiel Erweiterung für die Taskleiste.")]
    public partial class SampleExtension : DeskBand
    {

        public SampleExtension()
        {
            this.MinSize = new Size(110, 40);
            this.MinSizeVertical = new Size(100, 40);
            this.Title = "Beispiel Erweiterung";

            InitializeComponent();
        }

        // Sample Code
        public bool IsClicked = false;

        private void HelloWorld_Click(object sender, EventArgs e)
        {
            if (IsClicked == false)
            {
                label1.Text = "Copyright Patrick Becker";

            } else {
                label1.Text = "Hello World";
            }

            IsClicked = (IsClicked == false);
        }
    }
}
