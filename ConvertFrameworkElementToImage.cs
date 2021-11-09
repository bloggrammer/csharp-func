private void FrameworkElement CaptureScreen(FrameworkElement element, double dpiX=196.0, double dpiY=196.0)
        {
            if (element == null)
            {
                return null;
            }

            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(element.DesiredSize));
            element.UpdateLayout();

            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)element.Width, (int)element.Height,
                                                                dpiX, dpiY, PixelFormats.Pbgra32);
            bitmap.Render(element);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream1 = System.IO.File.Create(@"C:\555.png"))
            {
                encoder.Save(stream1);
            }
        }
        
        
        
        public static Image CreateImage(FrameworkElement visual, Size desiredSize, double dpiX, double dpiY, Stream stream)
        {
            Image image = new Image();
            try
            {
                visual.Measure(new Size(double.MaxValue, double.MaxValue));
                //visual.UpdateLayout();

                //visual.Arrange(new Rect(0, 0, visual.DesiredSize.Width, visual.DesiredSize.Height));
                //visual.UpdateLayout();

                double renderScaleX = dpiX / 96;
                double renderScaleY = dpiY / 96;

                double renderedWidth = renderScaleX * (visual.DesiredSize.Width);
                double renderedHeight = renderScaleY * visual.DesiredSize.Height;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)renderedWidth, (int)renderedHeight, dpiX, dpiY, PixelFormats.Default);
                rtb.Render(visual);

                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                png.Save(stream);

                BitmapImage bimg = new BitmapImage();
                bimg.BeginInit();
                bimg.StreamSource = stream;
                bimg.EndInit();

                image.Source = bimg;
                image.UpdateLayout();

                image.Arrange(new Rect(0, 0, renderedWidth, renderedHeight));

                ScaleTransform scaleTransform = new ScaleTransform
                {
                    ScaleX = desiredSize.Width / renderedWidth,
                    ScaleY = desiredSize.Height / renderedHeight
                };

                if (scaleTransform.ScaleX < scaleTransform.ScaleY)
                {
                    scaleTransform.ScaleY = scaleTransform.ScaleX;
                }
                else
                {
                    scaleTransform.ScaleX = scaleTransform.ScaleY;
                }

                image.Width = renderedWidth * scaleTransform.ScaleX;
                image.Height = renderedHeight * scaleTransform.ScaleY;
                image.Arrange(new Rect(0, 0, image.Width, image.Height));
                image.UpdateLayout();
            }
            catch (Exception)
            {
            }
            return image;
        }
