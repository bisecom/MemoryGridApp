using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MemoryGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimer;
        private Button temp;
        private int galleryIndex;
        private int imageFirstIndex;
        private int clicksCounter;
        private DispatcherTimer t;
        private DateTime start;

        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            imageFirstIndex = 0;
            galleryIndex = 0; clicksCounter = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (StackPanel sp in FindVisualChildren<StackPanel>(myGrid))
            {
                if (sp.Children.Count != 0)
                sp.Children.RemoveAt(0);
            }

            foreach (Button buttonElement in FindVisualChildren<Button>(myGrid))
            {
                if (buttonElement.Visibility == Visibility.Collapsed)
                    buttonElement.Visibility = System.Windows.Visibility.Visible;
            }
            Operating.GettingImgPaths();
            Operating.FillingMatrix();
            for (int i = 0; i < Operating.myGallery.Count; i++)
            {
                foreach (var sp in myGrid.Children)
                {
                    if (sp is StackPanel)
                    {
                        int myStackPanelNumber = Convert.ToInt32((sp as StackPanel).Name.Remove(0, 1));
                        if (Operating.myGallery[i].IndexRandom1 == myStackPanelNumber && i < Operating.myGallery.Count - 1)
                        {
                            CreateViewImageDynamically(Operating.myGallery[i].ImgPath, sp as StackPanel, i);
                        }
                        if (Operating.myGallery[i].IndexRandom2 == myStackPanelNumber && i < Operating.myGallery.Count - 1)
                        {
                            CreateViewImageDynamically(Operating.myGallery[i].ImgPath, sp as StackPanel, i);
                        }
                        if (Operating.myGallery[i].IndexRandom1 == myStackPanelNumber && i == Operating.myGallery.Count - 1)
                        {
                            CreateViewImageDynamically(Operating.myGallery[i].ImgPath, sp as StackPanel, i);
                        }

                    }
                }

            }

            

            t = new DispatcherTimer(new TimeSpan(0,0,0), DispatcherPriority.Background,
                t_Tick, Dispatcher.CurrentDispatcher); t.IsEnabled = true;
            start = DateTime.Now;
            clicksCounter = 0;
            ClicksDisplay.Text = clicksCounter.ToString();
        }

        private void mybutton_Click(object sender, RoutedEventArgs e)
        {
            temp = sender as Button;
            int myButtonNumber = 0; 
            int remainedImagesQty = 0;
            string strtmp = temp.Name.ToString().Remove(0, 1);
            myButtonNumber = Convert.ToInt32(strtmp.Replace("b", ""));
            if (Operating.myGallery.Count == 0) return;
            clicksCounter++;
            ClicksDisplay.Text = clicksCounter.ToString();
            if (imageFirstIndex != 0) // determine the second picture match
            {
                int tempIndex = 0;
                if (Operating.myGallery[galleryIndex].IndexRandom1 == imageFirstIndex && Operating.myGallery[galleryIndex].IndexRandom2 == myButtonNumber ||
                    Operating.myGallery[galleryIndex].IndexRandom1 == myButtonNumber && Operating.myGallery[galleryIndex].IndexRandom2 == imageFirstIndex)
                {
                    foreach (Button buttonElement in FindVisualChildren<Button>(myGrid))
                    {
                        tempIndex++;
                        if (tempIndex == imageFirstIndex || tempIndex == myButtonNumber)
                            buttonElement.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    //fill answers

                    if (Operating.myGallery[galleryIndex].IndexRandom1 == imageFirstIndex)
                    { Operating.myGallery[galleryIndex].IndexAnswer1 = imageFirstIndex; }
                    else { Operating.myGallery[galleryIndex].IndexAnswer1 = myButtonNumber; }

                    if (Operating.myGallery[galleryIndex].IndexRandom2 == myButtonNumber)
                    { Operating.myGallery[galleryIndex].IndexAnswer2 = myButtonNumber; }
                    else { Operating.myGallery[galleryIndex].IndexAnswer2 = imageFirstIndex; }

                    for (int i = 0; i < Operating.myGallery.Count; i++)
                    {
                        if (Operating.myGallery[i].IndexAnswer1 == 0)
                        {
                            remainedImagesQty++;
                        }
                    }

                    if (remainedImagesQty < 2)
                    {
                        foreach (Button buttonElement in FindVisualChildren<Button>(myGrid))
                        {
                            if (buttonElement.Visibility == Visibility.Visible)
                                buttonElement.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        t.Stop(); 
                    }
                }
                else
                {
                    temp.Visibility = System.Windows.Visibility.Collapsed;
                    dispatcherTimer.Start();
                }
                imageFirstIndex = myButtonNumber = 0;
                return;
            }

            if (imageFirstIndex == 0)
            {
                imageFirstIndex = myButtonNumber;
                for (int i = 0; i < Operating.myGallery.Count; i++) // find out which node is in work, remember the first picture
                {
                    if (Operating.myGallery[i].IndexRandom1 == imageFirstIndex || Operating.myGallery[i].IndexRandom2 == imageFirstIndex)
                    {
                        galleryIndex = i;
                        break;
                    }
                }
                temp.Visibility = System.Windows.Visibility.Collapsed;
                dispatcherTimer.Start();
            }

        }


        public void CreateViewImageDynamically(string str, StackPanel obj, int i)
        {
            Image dynamicImage = new Image();
            dynamicImage.Height = 100;
            dynamicImage.Stretch = Stretch.Uniform;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(str);
            bitmap.EndInit();
            // Set Image.Source  
            dynamicImage.Source = bitmap;
            obj.Children.Add(dynamicImage);
         }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
          
            dispatcherTimer.Stop();
            temp.Visibility = System.Windows.Visibility.Visible;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            var src = DateTime.Now - start;
            TimerDisplay.Text = Convert.ToString(src.Minutes) +":"+ Convert.ToString(src.Seconds);
    }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The application is developed within STEP Computer Academy \nby Bichurin S. 2019");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon! :)");
        }
    }
}
