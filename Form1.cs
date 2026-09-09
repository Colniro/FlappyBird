using Microsoft.VisualBasic;
using Timer = System.Windows.Forms.Timer;
namespace _5_FlappyBird;
public partial class Form1 : Form
{
    const int FormHeight = 800;
    const int FormWidth = 1200;
    bool spaceIsPressed = false;
    bool isRunning = false;
    int score = 0;
    Bird player = null;
    Label lblScore = new Label();
    Label lblHighScore = new Label();
    Label lblMessage = new Label();
    Timer tmrMain = new Timer() { Interval = 20, Enabled = false };
    Timer tmrSpawnPipes = new Timer { Interval = 2500, Enabled = false };
    public Form1()
    {
        InitGame();
        tmrMain.Tick += TmrMain_Tick;
        tmrSpawnPipes.Tick += TmrSpawnPipes_Tick;
        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;

    }


    public void InitGame()
    {
        Height = FormHeight;
        Width = FormWidth;
        BackColor = Color.WhiteSmoke;

        player = new Bird(0, 5, Color.Blue, new Size(50, 52));
        player.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 2 - player.Height / 2);
        Controls.Add(player);

        lblScore.Font = new Font(Font.FontFamily, 20, FontStyle.Bold);
        lblScore.AutoSize = true;
        lblScore.Location = new Point(0, 0);
        lblScore.Text = "Score: 0";
        Controls.Add(lblScore);

        lblMessage.Font = new Font(Font.FontFamily, 24, FontStyle.Bold);
        lblMessage.Text = "Press Space to start";
        lblMessage.TextAlign = ContentAlignment.MiddleCenter;
        lblMessage.BackColor = Color.WhiteSmoke;
        lblMessage.Size = new Size(ClientSize.Width, 100);
        lblMessage.Location = new Point(0, ClientSize.Height / 3);
        Controls.Add(lblMessage);

        lblHighScore.Font = new Font(Font.FontFamily, 20, FontStyle.Bold);
        lblHighScore.AutoSize = true;
        lblHighScore.Location = new Point(0, 70);
        lblHighScore.Text = $"Highscore: {LoadHighscore()}";
        Controls.Add(lblHighScore);

        lblScore.BringToFront();
        lblMessage.BringToFront();
    }

    private void StartGame()
    {
        foreach (var pipe in MoveableGameElement.gameElements.OfType<Pipe>().ToList())
        {
            Controls.Remove(pipe);
            MoveableGameElement.gameElements.Remove(pipe);
            pipe.Dispose();
        }

        player.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 2 - player.Height / 2);
        player._yMovingPixel = 5;

        score = 0;
        lblScore.Text = "Score: 0";
        lblMessage.Visible = false;

        SpawnPipe();
        tmrMain.Enabled = true;
        tmrSpawnPipes.Enabled = true;
        isRunning = true;
    }
    private void Form1_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
        {
            spaceIsPressed = false;
        }
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
        {
            spaceIsPressed = true;
            if (!isRunning)
            {
                StartGame();
            }
        }
    }
    private void TmrSpawnPipes_Tick(object? sender, EventArgs e)
    {
        SpawnPipe();
    }

    private void SpawnPipe()
    {
        Random rnd = new Random();

        Pipe bottomPipe = new Pipe(-5, 0, Color.Green, new Size(50, 1000));
        Pipe topPipe = new Pipe(-5, 0, Color.Green, new Size(50, 1000));
        topPipe.CountsForScore = true;

        int bottomLocationHeight = rnd.Next(350, 700);
        topPipe.Location = new Point(ClientSize.Width + 30, bottomLocationHeight - 1300);
        bottomPipe.Location = new Point(ClientSize.Width + 30, bottomLocationHeight);

        Controls.Add(topPipe);
        Controls.Add(bottomPipe);

        lblScore.BringToFront();
        lblMessage.BringToFront();
    }

    private void TmrMain_Tick(object? sender, EventArgs e)
    {
        foreach (var item in MoveableGameElement.gameElements.ToList())
        {
            if (spaceIsPressed)
            {
                player.Flap();
                spaceIsPressed = false;
            }
            if (item.Left < -30)
            {
                Controls.Remove(item);
                MoveableGameElement.gameElements.Remove(item);
                item.Dispose();
                continue;
            }
            if (item is Pipe pipe && pipe.CountsForScore && !pipe.Scored && item.Right < player.Left)
            {
                pipe.Scored = true;
                score++;
                lblScore.Text = $"Score: {score}";
            }
            if (item is Bird p)
            {
                CheckForCollision(p, MoveableGameElement.gameElements);
            }
            item.Move();
        }
        if (player._yMovingPixel < 30)
        {
            player._yMovingPixel += 2;
        }
    }

    private void CheckForCollision(Bird player, List<MoveableGameElement> list)
    {
        foreach (var item in list)
        {
            if (item is Bird)
                continue;
            if (player.Top < item.Bottom && player.Bottom > item.Top 
                && player.Left < item.Right && player.Right > item.Left 
                || player.Top < 0 || player.Bottom > ClientSize.Height)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        tmrMain.Enabled = false;
        tmrSpawnPipes.Enabled = false;
        isRunning = false;

        if (score > LoadHighscore())
        {
            SaveHighscore(score);
            lblHighScore.Text = $"Highscore: {score}";
        }

        lblMessage.Text = "Press Space to restart";
        lblMessage.Visible = true;
        lblMessage.BringToFront();
    }

    private static readonly string HighscorePath = Path.Combine(AppContext.BaseDirectory, "highscore.csv");

    private void SaveHighscore(int score)
    {
        using (StreamWriter sw = new StreamWriter(HighscorePath))
        {
            sw.WriteLine(score.ToString());
        }
    }

    private int LoadHighscore()
    {
        if (!File.Exists(HighscorePath))
        {
            return 0;
        }
        using (StreamReader sr = new StreamReader(HighscorePath))
        {
            return int.Parse(sr.ReadLine());
        }
    }
}
