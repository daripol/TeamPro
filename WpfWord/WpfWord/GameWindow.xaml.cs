using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfWord
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Game game;

        public GameWindow()
        {
            InitializeComponent();
            this.game = new Game();

            UpdateAttempts();
            UpdateCombo();
            UpdateWord();

            game.Attemts.OnValueChanged += UpdateAttempts;
            game.Combo.OnValueChanged += UpdateCombo;
            game.Word.OnValueChanged += UpdateWord;
            game.OnGameEnded += TurnTryButtonOff;
            game.OnGameEnded += () => MessageBox.Show("Игра закончена!");
            game.OnGameStarted += TurnTryButtonOn;
            game.OnGameStarted += () => log.Items.Clear();
            game.OnNewGameEvent += LogEvent;
        }

        private void UpdateAttempts()
        {
            attempts.Content = game.Attemts.Value;
        }

        private void UpdateCombo()
        {
            combo.Content = game.Combo.Value;
        }

        private void UpdateWord()
        {
            word.Content = game.Word.Value;
        }

        private void TurnTryButtonOff()
        {
            tryButton.IsEnabled = false;
        }

        private void PrintEndMessage()
        {
            MessageBox.Show("Игра закончилась!");
        }

        private void TurnTryButtonOn()
        {
            tryButton.IsEnabled = true;
        }

        private void LogEvent(string eventInfo)
        {
            log.Items.Add(eventInfo);
        }

        private void Try_Click(object sender, RoutedEventArgs e)
        {
            game.TryGuess(playerWord.Text);
        }

        private void Again_Click_1(object sender, RoutedEventArgs e)
        {
            game.Refresh();
        }
    }
}
