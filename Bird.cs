using System;
using System.Collections.Generic;
using System.Text;

namespace _5_FlappyBird;

public class Bird : MoveableGameElement
{
    public static Color _backColor;
    public static Size _size;
    public Bird(int xMovingPixel, int yMovingPixel) : base(xMovingPixel, yMovingPixel)
    {
        _backColor = Color.Blue;
        _size = new Size(50, 50);
        this.BackColor = _backColor;
        this.Size = _size;

        xMovingPixel = 0;
        _yMovingPixel = yMovingPixel;
    }

    public void Flap()
    {
        _yMovingPixel = -18;
    }
}
