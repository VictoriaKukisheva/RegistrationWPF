using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RegistrationWPF.Authtorization
{
    /// <summary>
    /// Логика взаимодействия для Capcha.xaml
    /// </summary>
    public partial class Capcha : Window
    {
        public Capcha()
        {
            InitializeComponent();
            MakeCapcha();
        }

        //Создание CAPCHA
        private void MakeCapcha()
        {
            String allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z,1,2,3,4,5,6,7,8,9,0";

            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";

            Random ran = new Random();

            for (int i = 0; i < 4; i++)
            {
                temp = ar[(ran.Next(0, ar.Length))];
                pwd += temp;
            }

            textblock.Text = pwd;
        }
        //Реализация блокировки на 10с кнопки
        private void Close_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (textblock.Text == textBox.Text)
            {
                this.Close();
            }
            else if (String.IsNullOrEmpty(textBox.Text))
            {
                MakeCapcha();
            }
            else
            {
				close_Btn.IsEnabled = false;

                DispatcherTimer timer = new DispatcherTimer();
                TimeSpan time = new TimeSpan();

                time = TimeSpan.FromSeconds(10);

                timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    lblTime.Text = time.ToString("c");
                    if (time == TimeSpan.Zero)
                    {
                        timer.Stop();
                        this.Close();
                    }
                    time = time.Add(TimeSpan.FromSeconds(-1));

                }, Application.Current.Dispatcher);

                timer.Start();
            }
        }
    }
}
