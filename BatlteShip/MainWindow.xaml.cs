using BatlteShip.ExcelAccess;
using BatlteShip.Models;
using BatlteShip.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BatlteShip
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Excel ea = new Excel();
        private int score = 0;
        private int fieldsL = 10;
        private List<Fields> ChosenFields = new List<Fields>() { new Fields { FieldName = "A1" } };
        public MainWindow()
        {
            InitializeComponent();
            DataFields.ItemsSource = ChosenFields;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (ChosenFields.Count != 10)
            {
                MessageBox.Show("You can choose only 10 fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                FieldsRules fr = new FieldsRules();
                bool playable = true;
                foreach (Fields f in ChosenFields)
                {
                    var vr = fr.Validate(f);
                    if (vr.IsValid == false)
                    {
                        playable = false;
                        foreach (var error in vr.Errors)
                        {
                            FieldsLeft.Text += $"{error.ErrorMessage}\n";
                        }
                    }
                }
                if (playable)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(ExcelAnother.Text);
                        if (!fi.Exists || fi.Extension != ".xlsx" || !Path.IsPathRooted(ExcelAnother.Text)) //
                        {
                            MessageBox.Show("Wrong file path or file type!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            List<Fields> another = await ea.GetOpponentsData(fi);
                            var res = (from Fields a in another select a.FieldName).ToList().Distinct();
                            var my = (from Fields m in ChosenFields select m.FieldName).ToList().Distinct();
                            foreach (string myChoice in my)
                            {
                                if (res.Contains(myChoice))
                                    score++;
                            }
                        }
                        MessageBox.Show($"Your result is {score}", "Your result", MessageBoxButton.OK, MessageBoxImage.Hand);
                        score = 0;
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);

                    }
                    try
                    {
                        FileInfo fi = new FileInfo(ExcelMy.Text);
                        if (!fi.Exists || fi.Extension != ".xlsx" || !Path.IsPathRooted(ExcelAnother.Text))
                        {
                            MessageBox.Show("Wrong file path or file type!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            int comp = await ea.CompGuess(fi);
                            MessageBox.Show($"The comp score is {comp}", "Your result", MessageBoxButton.OK, MessageBoxImage.Hand);

                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageBox.Show("There is something wrong with your fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }

        private void DataFields_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            FieldsLeft.Text = $"Fields left: {fieldsL - ChosenFields.Count} \nError:\n";
            if (ChosenFields.Count > 10)
            {
                MessageBox.Show("You can choose only 10 fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
