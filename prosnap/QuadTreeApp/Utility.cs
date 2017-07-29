using System;
using System.Drawing;

namespace QuadTreeApp
{
    public static class Utility
    {
        static Random m_rand = new Random(DateTime.Now.Millisecond);
        
        public static Color RandomColor
        {
            get
            {
                return Color.FromArgb(
                    255,
                    m_rand.Next(255),
                    m_rand.Next(255),
                    m_rand.Next(255));

            }
        }
    }
}
