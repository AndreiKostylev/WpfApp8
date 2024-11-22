using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

public class CaptchaGenerator
{
    private static readonly Random random = new Random();

    public static string GenerateCaptchaText()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int length = random.Next(4, 8);
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static byte[] GenerateCaptchaImage(string captchaText)
    {
        int width = 200;
        int height = 70;

        using (var bitmap = new Bitmap(width, height))
        using (var g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            // Фон с шумом
            AddNoise(g, width, height);

            var font = new Font("Arial", random.Next(20, 30), FontStyle.Bold);
            int x = 10;
            for (int i = 0; i < captchaText.Length; i++)
            {
                int y = random.Next(10, 30);
                g.RotateTransform(random.Next(-10, 10));
                g.DrawString(captchaText[i].ToString(), font, new SolidBrush(GetRandomColor()), x, y);
                g.ResetTransform();
                x += random.Next(20, 30);
            }

            AddLines(g, width, height);

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }

    private static void AddNoise(Graphics g, int width, int height)
    {
        for (int i = 0; i < 100; i++)
        {
            int x = random.Next(width);
            int y = random.Next(height);
            g.FillEllipse(new SolidBrush(GetRandomColor()), x, y, 2, 2);
        }
    }

    private static void AddLines(Graphics g, int width, int height)
    {
        for (int i = 0; i < 5; i++)
        {
            using (var pen = new Pen(GetRandomColor(), 1))
            {
                g.DrawLine(pen, random.Next(width), random.Next(height), random.Next(width), random.Next(height));
            }
        }
    }

    private static Color GetRandomColor()
    {
        return Color.FromArgb(random.Next(100, 255), random.Next(100, 255), random.Next(100, 255));
    }
}
