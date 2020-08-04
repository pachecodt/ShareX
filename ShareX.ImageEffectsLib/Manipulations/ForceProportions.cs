﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2020 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace ShareX.ImageEffectsLib
{
    [Description("Force Proportions")]
    internal class ForceProportions : ImageEffect
    {
        private int width = 1;
        private int height = 1;

        [DefaultValue(typeof(int), "1")]
        public int ProportionalWidth
        {
            get {
                return width;
            }
            set
            {
                width = Math.Max(1, value);
            }
        }
        [DefaultValue(typeof(int), "1")]
        public int ProportionalHeight
        {
            get
            {
                return height;
            }
            set
            {
                height = Math.Max(1, value);
            }
        }

        public enum ForceMethod
        {
            Grow,
            Crop
        }

        public ForceMethod Method { get; set; } = ForceMethod.Grow;

        [DefaultValue(typeof(Color), "Transparent"), Editor(typeof(MyColorEditor), typeof(UITypeEditor)), TypeConverter(typeof(MyColorConverter))]
        public Color GrowFillColor { get; set; } = Color.FromArgb(0);

        public ForceProportions()
        {
            this.ApplyDefaultPropertyValues();
        }

        public override Bitmap Apply(Bitmap bmp)
        {
            float current_ratio = bmp.Width / (float)bmp.Height;
            float target_ratio = width / (float)height;

            bool is_target_wider = target_ratio > current_ratio;

            int target_width = bmp.Width;
            int target_height = bmp.Height;
            int margin_left = 0;
            int margin_top = 0;

            if (Method == ForceMethod.Crop)
            {
                if (is_target_wider)
                {
                    target_height = (int)Math.Round(bmp.Width / target_ratio);
                    margin_top = (bmp.Height - target_height) / 2;
                }
                else
                {
                    target_width = (int)Math.Round(bmp.Height * target_ratio);
                    margin_left = (bmp.Width - target_width) / 2;
                }
                return ImageHelpers.CropBitmap(bmp, new Rectangle(margin_left, margin_top, target_width, target_height));
            }
            if (Method == ForceMethod.Grow)
            {
                if (is_target_wider)
                {
                    target_width = (int)Math.Round(bmp.Height * target_ratio);
                }
                else
                {
                    target_height = (int)Math.Round(bmp.Width / target_ratio);
                }
                return ImageHelpers.ResizeImage(bmp, target_width, target_height, false, true, GrowFillColor);
            }

            return bmp;
        }
    }
}