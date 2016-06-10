using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NControl.Controls;
using NControl.Controls.WP81;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Size = Xamarin.Forms.Size;

[assembly: ExportRenderer(typeof(BlurImageView), typeof(BlurViewRenderer))]
namespace NControl.Controls.WP81
{
    public class BlurViewRenderer : ViewRenderer<BlurImageView, System.Windows.Controls.Image>
    {
        
        protected override void OnElementChanged(ElementChangedEventArgs<BlurImageView> e)
        {
            base.OnElementChanged(e);

            var image = new System.Windows.Controls.Image();
            this.SetSource(image);
            this.SetAspect(image);
            this.SetNativeControl(image);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Xamarin.Forms.Image.SourceProperty.PropertyName)
            {
                this.SetSource(this.Control);
            }
            else
            {
                if (e.PropertyName != Xamarin.Forms.Image.AspectProperty.PropertyName)
                    return;
                this.SetAspect(this.Control);
            }
        }

        private async void SetSource(System.Windows.Controls.Image image)
        {
            IImageSourceHandler handler;
            var source = (this.Element as BlurImageView).Source;
            if (source == null)
                return;

            if (source is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                handler = new StreamImagesourceHandler(); // sic
            }
            else if (source is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler(); // sic
            }
            else
            {
                throw new NotImplementedException();
            }
            
            System.Windows.Media.ImageSource imagesource;
            try
            {
                imagesource = await handler.LoadImageAsync(source, new CancellationToken());
            }
            catch (TaskCanceledException)
            {
                imagesource = (System.Windows.Media.ImageSource)null;
            }
            
            // Blur              
            if (imagesource is BitmapImage)
            {
                
        
                var bmpImage = imagesource as BitmapImage;
                var wr = new WriteableBitmap(bmpImage);
                BoxBlurHorizontal(wr, 40);
                BoxBlurVertical(wr, 40);
                image.Source = wr;                
            }            
        }

        private void BoxBlurHorizontal(WriteableBitmap bmp, int range)
        {
            int[] pixels = bmp.Pixels;
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int halfRange = range / 2;
            int index = 0;
            int[] newColors = new int[w];
 
            for (int y = 0; y < h; y++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                for (int x = -halfRange; x < w; x++)
                {
                    int oldPixel = x - halfRange - 1;
                    if (oldPixel >= 0)
                    {
                        int col = pixels[index + oldPixel];
                        if (col != 0)
                        {
                            r -= ((byte)(col >> 16));
                            g -= ((byte)(col >> 8 ));
                            b -= ((byte)col);
                        }
                        hits--;
                    }
 
                    int newPixel = x + halfRange;
                    if (newPixel < w)
                    {
                        int col = pixels[index + newPixel];
                        if (col != 0)
                        {
                            r += ((byte)(col >> 16));
                            g += ((byte)(col >> 8 ));
                            b += ((byte)col);
                        }
                        hits++;
                    }
 
                    if (x >= 0)
                    {
                        int color =
                            (255 << 24)
                            | ((byte)(r / hits) << 16)
                            | ((byte)(g / hits) << 8 )
                            | ((byte)(b / hits));
 
                        newColors[x] = color;
                    }
                }
 
                for (int x = 0; x < w; x++)
                {
                    pixels[index + x] = newColors[x];
                }
 
                index += w;
            }
        }

        private void BoxBlurVertical(WriteableBitmap bmp, int range)
        {
            int[] pixels = bmp.Pixels;
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int halfRange = range/2;

            int[] newColors = new int[h];
            int oldPixelOffset = -(halfRange + 1)*w;
            int newPixelOffset = (halfRange)*w;

            for (int x = 0; x < w; x++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int index = -halfRange*w + x;
                for (int y = -halfRange; y < h; y++)
                {
                    int oldPixel = y - halfRange - 1;
                    if (oldPixel >= 0)
                    {
                        int col = pixels[index + oldPixelOffset];
                        if (col != 0)
                        {
                            r -= ((byte) (col >> 16));
                            g -= ((byte) (col >> 8));
                            b -= ((byte) col);
                        }
                        hits--;
                    }

                    int newPixel = y + halfRange;
                    if (newPixel < h)
                    {
                        int col = pixels[index + newPixelOffset];
                        if (col != 0)
                        {
                            r += ((byte) (col >> 16));
                            g += ((byte) (col >> 8));
                            b += ((byte) col);
                        }
                        hits++;
                    }

                    if (y >= 0)
                    {
                        int color =
                            (255 << 24)
                            | ((byte) (r/hits) << 16)
                            | ((byte) (g/hits) << 8)
                            | ((byte) (b/hits));

                        newColors[y] = color;
                    }

                    index += w;
                }

                for (int y = 0; y < h; y++)
                {
                    pixels[y*w + x] = newColors[y];
                }
            }
        }

        private void SetAspect(System.Windows.Controls.Image image)
        {
            Aspect aspect = this.Element.Aspect;
            image.Stretch = this.ToStretch(aspect);
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (this.Control.Source == null)
                return new SizeRequest();
            return new SizeRequest(new Size()
            {
                Width = (double)((BitmapSource)this.Control.Source).PixelWidth,
                Height = (double)((BitmapSource)this.Control.Source).PixelHeight
            });
        }

        private Stretch ToStretch(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.AspectFill:
                    return Stretch.UniformToFill;
                case Aspect.Fill:
                    return Stretch.Fill;
                default:
                    return Stretch.Uniform;
            }
        }

    }
}
