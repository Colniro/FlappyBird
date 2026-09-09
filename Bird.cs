using System;
using System.Collections.Generic;
using System.Text;

namespace _5_FlappyBird;

public class Bird : MoveableGameElement
{

    public Bird(int xMovingPixel, int yMovingPixel, Color color, Size size) : base(xMovingPixel, yMovingPixel, color, size)
    {
        _backColor = color;
        _size = size;
        this.BackColor = _backColor;
        this.Size = _size;

        _xMovingPixel = 0;
        _yMovingPixel = yMovingPixel;
    }

    public void Flap()
    {
        _yMovingPixel = -18;
    }
}
