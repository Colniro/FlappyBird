using Microsoft.VisualBasic;
using Timer = System.Windows.Forms.Timer;
namespace _5_FlappyBird;
public partial class Form1 : Form
{
    const int FormHeight = 800;
    const int FormWidth = 1200;
    bool spaceIsPressed = false;
    Bird player = null;
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
        player.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 2 - Bird._size.Height / 2);
        Controls.Add(player);

        SpawnPipe();
        tmrMain.Enabled = true;
        tmrSpawnPipes.Enabled = true;
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

        int bottomLocationHeight = rnd.Next(350, 700);
        topPipe.Location = new Point(ClientSize.Width + 30, bottomLocationHeight - 1300);
        bottomPipe.Location = new Point(ClientSize.Width + 30, bottomLocationHeight);

        Controls.Add(topPipe);
        Controls.Add(bottomPipe);
    }

    private void TmrMain_Tick(object? sender, EventArgs e)
    {
        foreach (var item in MoveableGameElement.gameElements)
        {
            if (spaceIsPressed)
            {
                player.Flap();
            }
            if (item.Left < -10)
            {
                Controls.Remove(item);
                item.Dispose();
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
    }
}
