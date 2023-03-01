using System.Linq;
using System.Linq.Expressions;

namespace Celeste_WinForms_Level_Designer
{
    public partial class MainWindow : Form
    {
        // Globální promìnné
        string[] output;
        string terrainArray = "terrainArray = new Terrain[] { ";
        int index = 0;

        public MainWindow()
        {
            InitializeComponent();

            output = new string[gameScreen.Controls.OfType<PictureBox>().Count() + 1];

            Button export = new Button
            {
                Left = gameScreen.Width / 2 - 100,
                Top = gameScreen.Height / 2 - 40,
                Width = 200,
                Height = 80,
                Text = "Exportovat",
                BackColor = Color.White,
            };
            gameScreen.Controls.Add(export);
            export.Font = new Font(export.Font.FontFamily, 12);
            export.Click += export_Click;
            export.BringToFront();
        }

        async void export_Click(object sender, EventArgs e)
        {
            output[0] = $"gameScreen.Height = {gameScreen.Height};\r\n\r\n";

            foreach (PictureBox block in gameScreen.Controls.OfType<PictureBox>().Where(block => block.Name != "player"))
            {
                output[index] += ($"Terrain {block.Name} = new({block.Left}, {block.Top}, {block.Width}, {block.Height}, \"{block.Tag.ToString().Split('.')[0]}\", Color.FromArgb({block.BackColor.R}, {block.BackColor.G}, {block.BackColor.B}), {(block.Image != null ? "true" : "false")}, Resources.{(block.Image != null ? (block.Tag.ToString().Split('.')[1]) : "blank")}, {gameScreen.Name});");
                terrainArray += $"{block.Name}{(index != gameScreen.Controls.OfType<PictureBox>().Count() -1 ? "," : "")} ";
                index++;
            }

            terrainArray += "};";

            output[index] = $"\r\n{terrainArray}\r\n";
            index++;

            output[index] = $"player.Left = {player.Left};" +
                $"\r\nplayer.Top = {player.Top};\r\n" +
                $"\r\nCameraFocus(\"{((player.Top + player.Height/2 < 432) ? "Top" : (player.Top + player.Height / 2 > gameScreen.Height - 432) ? "Bottom" : "Player")}\");";

            // Uložení souboru do zvoleného adresáøe
            await File.WriteAllLinesAsync($"levely/Level{gameScreen.Tag}.txt", output);

            // Otevøení souboru
            System.Diagnostics.Process.Start("notepad.exe", $"levely/Level{gameScreen.Tag}.txt");
        }
    }
}