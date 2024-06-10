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

namespace ипотека
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void raschitat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение данных из текстовых полей
                double price = Convert.ToDouble(cena.Text);
                int years = Convert.ToInt32(srok.Text);
                double rate = Convert.ToDouble(stavka.Text);
                double startPercent = Convert.ToDouble(vznos.Text);

                // Расчеты
                double startPay = price * (startPercent / 100);
                double loanAmount = price - startPay;
                double monthlyRate = rate / 12 / 100;
                int months = years * 12;

                // Ежемесячный платеж по формуле аннуитетных платежей
                double monthlyPayment = loanAmount * (monthlyRate + monthlyRate / (Math.Pow(1 + monthlyRate, months) - 1));

                // Основной долг
                double remainingLoan = loanAmount;
                double principalPaymentFirstMonth = monthlyPayment - (remainingLoan * monthlyRate);
                double principalPayment = principalPaymentFirstMonth;

                // Проценты
                double interestPayment = remainingLoan * monthlyRate;
                double interestSum = interestPayment;

                // Переплата
                double overpayment = monthlyPayment * months - loanAmount;

                // Заполнение текстовых полей с результатами

                nach.Text = DateTime.Now.ToString("MM/yyyy");
                data.Text = DateTime.Now.AddMonths(months).ToString("MM/yyyy");
                pvznos.Text = startPay.ToString("N2");
                summa.Text = loanAmount.ToString("N2");
                pereplata.Text = overpayment.ToString("N2");

                // Очистка DataGrid перед заполнением
                dgv.Items.Clear();

                // Формирование графика платежей
                for (int i = 1; i <= months; i++)
                {
                    remainingLoan -= principalPayment;

                    dgv.Items.Add(new
                    {
                        Number = i,
                        MonthYear = DateTime.Now.AddMonths(i).ToString("MM/yyyy"),
                        Payment = monthlyPayment.ToString("N2"),
                        Principal = principalPayment.ToString("N2"),
                        Interest = interestPayment.ToString("N2"),
                        Balance = remainingLoan.ToString("N2")
                    });

                    // Пересчет основного долга и процентов для следующего месяца
                    interestPayment = remainingLoan * monthlyRate;
                    interestSum += interestPayment;
                    principalPayment = monthlyPayment - interestPayment;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расчете: " + ex.Message);
            }
        }

        private void vihod_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
