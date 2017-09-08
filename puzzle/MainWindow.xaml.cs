using Microsoft.Win32;
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

namespace puzzle
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Button btEmpty;
        public static Uri uriCurrentDirectory;
        public static Uri uriImage;
        public MainWindow()
        {
            InitializeComponent();
            //Uri uriCurrentDirectory = new Uri(System.IO.Directory.GetCurrentDirectory());
            //Uri uriImage = new Uri(uriCurrentDirectory, "../obj/test.png");
            //makeParts();
        }

        public void makeParts()
        {
            GridLength cellWidth = new GridLength(myGrid.Height / 5);                
            for (int i = 1; i <= 5; i++)
            {
                RowDefinition rd = new RowDefinition();                
                rd.Height = cellWidth;
                myGrid.RowDefinitions.Add(rd);
            }
            for (int i = 1; i <= 5; i++)
            { 
                ColumnDefinition cd = new ColumnDefinition();
                Double a = myGrid.Width;
                cd.Width = new GridLength((myGrid.Width / 5));
                myGrid.ColumnDefinitions.Add(cd);
            }
            //preparation image de base            
            BitmapImage basicBitmapImage = new BitmapImage(uriImage);
            BasicImage.Source = basicBitmapImage;
            BasicImage.Width = 400;
            BasicImage.Height = 400;            
            BasicImage.Margin = new Thickness(myGrid.Margin.Left, myGrid.Margin.Top,0,0);
            BasicImage.Visibility = Visibility.Hidden;
            int croppedImageWidth = (int)(basicBitmapImage.PixelWidth / 5)-1;
            int croppedImageHeight = (int)(basicBitmapImage.PixelHeight / 5)-1;
            //
            myGrid.Children.Clear();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((i != 4) || (j != 4))
                    {                                             
                        Button bt = new Button();
                        bt.Name = "bt" + (i * 5 + j);
                        bt.HorizontalAlignment = HorizontalAlignment.Stretch;
                        bt.VerticalAlignment = VerticalAlignment.Stretch;                        
                        CroppedBitmap croppedBitmap = new CroppedBitmap(basicBitmapImage, new Int32Rect(j * (int)croppedImageWidth, i * (int)croppedImageHeight, (int)croppedImageWidth, (int)croppedImageHeight));
                        ImageBrush croppedImgBruch = new ImageBrush(croppedBitmap);
                        bt.Background = croppedImgBruch;                                                
                        bt.Click += new RoutedEventHandler(move_Click);
                        Grid.SetRow(bt, i);
                        Grid.SetColumn(bt, j);
                        myGrid.Children.Add(bt);
                    }
                }                
            }
            btEmpty = new Button();
            btEmpty.Name = "btEmpty";
            btEmpty.HorizontalAlignment = HorizontalAlignment.Stretch;
            btEmpty.VerticalAlignment = VerticalAlignment.Stretch;
            btEmpty.Content = Char.ConvertFromUtf32(169) + "By Ouissem";
            btEmpty.Background = new SolidColorBrush(Colors.Black);
            btEmpty.Foreground = new SolidColorBrush(Colors.White);            
            Grid.SetRow(btEmpty, 5);
            Grid.SetColumn(btEmpty, 5);
            myGrid.Children.Add(btEmpty);
            //
            this.Show();
            myGrid.Children.MelangerList();
            //UIElementCollection buttonChildren = (from Button item in myGrid.Children where item.GetType() ==  new Button().GetType() select item) as UIElementCollection;
            //List<Button> liste = myGrid.Children.Cast<Button>().ToList();
            //liste.MelangerList();            
        }

        protected void move_Click(object sender, EventArgs e)
        {
            Button btClicked = sender as Button;
            // identify which button was clicked and perform necessary actions
            int clickedRow = Grid.GetRow(btClicked);   
            int clickedColumn = Grid.GetColumn(btClicked);
            int EmptyRow = Grid.GetRow(btEmpty);
            int EmptyColumn = Grid.GetColumn(btEmpty);

            if  ( ( (EmptyColumn == clickedColumn) && ( (EmptyRow == clickedRow + 1) || (EmptyRow == clickedRow - 1) ) ) 
                ||  ( (EmptyRow == clickedRow) && ((EmptyColumn == clickedColumn + 1) || (EmptyColumn == clickedColumn - 1) ) )  )
            {
                Grid.SetColumn(btClicked, EmptyColumn);
                Grid.SetColumn(btEmpty, clickedColumn);
                Grid.SetRow(btClicked, EmptyRow);
                Grid.SetRow(btEmpty, clickedRow);
            }            
        }

        private void btFileName_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                uriImage = new Uri(openFileDialog.FileName);
                makeParts();
            }
        }
    }
    static class Extension
    {
        private static Random r = new Random();
        public static void MelangerList(this UIElementCollection list)
        {
            List<int> tirages = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
            int k;
            int n;
            while (tirages.Count > 1)
            {
                n = tirages[0];
                do
                {
                    k = r.Next(list.Count);
                } while ( (!tirages.Contains(k)) || (n==k) );
                tirages.Remove(k);
                tirages.Remove(n);
                Grid.SetRow(list[k], n/5);
                Grid.SetColumn(list[k], n % 5);
                Grid.SetRow(list[n], k / 5);
                Grid.SetColumn(list[n], k % 5);                  
                //Application.Current.MainWindow.Show();
            }
        }
    }
}
