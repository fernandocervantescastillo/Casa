using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;

namespace Casa
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Grafica",
            };

            using (var window = new Escenario(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

