    public static ImageSource GetImageFromBase64String(string imageString)
    {
        var byteArray = Convert.FromBase64String(ImageString);
        BitmapImage biImg = new BitmapImage();
        MemoryStream ms = new MemoryStream(byteArray);
        biImg.BeginInit();
        biImg.StreamSource = ms;
        biImg.EndInit();
        return biImg as ImageSource;
    }