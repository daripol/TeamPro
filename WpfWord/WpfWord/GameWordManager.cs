using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfWord
{
    public class GameValue<T>
    {
        private T value;

        public GameValue(T value)
        {
            this.value = value;
        }

        public event Action OnValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke();
            }
        }
    }

    public class Game
    {
        public event Action OnGameStarted;
        public event Action OnGameEnded;
        public event Action<string> OnNewGameEvent;

        private int a;

        private GameValue<int> attempts;
        private GameValue<int> combo;
        private GameValue<string> computerWord;

        public GameValue<int> Attemts => attempts;
        public GameValue<int> Combo => combo;
        public GameValue<string> Word => computerWord;

        private List<string> words;
        private List<string> usedWords = new List<string>();

        public Game()
        {
            words = GetWordsList();
            attempts = new GameValue<int>(5);
            combo = new GameValue<int>(0);
            computerWord = new GameValue<string>(GenerateWord());
            OnGameStarted?.Invoke();
        }

        public void Refresh()
        {
            words = GetWordsList();
            attempts.Value = 5;
            combo.Value = 0;
            computerWord.Value = GenerateWord();
            OnGameStarted?.Invoke();
            usedWords.Clear();
        }

        public void TryGuess(string playerWord)
        {
            if (!IsAvaliableWord(playerWord))
            {
                OnNewGameEvent?.Invoke($"Игрок ввел недопустимое слово \"{playerWord}\"");
                return;
            }
            else
            {
                usedWords.Add(playerWord);
            }

            OnNewGameEvent?.Invoke($"Игрок ввел слово \"{playerWord}\"");

            if (this.computerWord.Value.Length == playerWord.Length)
            {
                MoveEnded(true);
                OnNewGameEvent?.Invoke("Вы угадали!");
                computerWord.Value = GenerateWord();
            }
            else
            {
                MoveEnded(false);
                OnNewGameEvent?.Invoke("Не угадали");
            }
            GameOverCheck();
        }

        private void MoveEnded(bool guessed)
        {
            if (guessed)
            {
                combo.Value++;
                attempts.Value = 5;
                computerWord.Value = GenerateWord();
            }
            else
            {
                combo.Value = 0;
                attempts.Value--;
            }
        }

        private void GameOverCheck()
        {
            if (attempts.Value == 0 || combo.Value == 3)
                OnGameEnded?.Invoke();
        }

        private string GenerateWord()
        {
            int i = new Random().Next(0, words.Count);
            string word = words[i];
            if (IsAvaliableWord(word))
            {
                usedWords.Add(word);
                return word;
            }
            else
                return GenerateWord();
        }

        private bool IsAvaliableWord(string word)
        {
            return !usedWords.Contains(word) && words.Contains(word);
        }

        private List<string> GetWordsList()
        {
            List<string> tempList = new List<string>();

            using (StreamReader sr = new StreamReader("word.txt", System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    tempList.Add(line);
                }
            }

            return tempList;
        }
    }
}
