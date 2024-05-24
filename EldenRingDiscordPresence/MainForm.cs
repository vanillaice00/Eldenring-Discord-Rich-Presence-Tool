namespace EldenRingDiscordPresence
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        public void SetStatus(String statusText, Color color)
        {
            Invoke(() =>
            {
                statusLabel.Text = statusText;
                statusLabel.ForeColor = color;
            });
        }

  
    }
}
