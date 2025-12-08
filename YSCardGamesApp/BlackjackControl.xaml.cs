using System.Windows.Controls;
using YSCardGamesLibrary;

namespace YSCardGamesApp
{
    /// <summary>
    /// Interaction logic for BlackjackControl.xaml
    /// </summary>
    public partial class BlackjackControl : UserControl
    {
        private List<Card> _playerCards = [];
        private List<string> _playerCardTexts =[];
        private List<Card> _bankCards = [];
        private List<string> _bankCardTexts = [];
        private CardDesk? _cardDesk;

        private int _playerScore = 0;
        private int _bankScore = 0;

        public BlackjackControl()
        {
            InitializeComponent();
            PlayerScoreText.Text = "0";
            BankScoreText.Text = "0";
            PrepareGame();
        }

        private void PrepareGame()
        {
            _playerCards = [];
            _playerCardTexts = [];
            _bankCards = [];
            _bankCardTexts = [];
            _cardDesk = new();

            PlayerResultLabel.Visibility = System.Windows.Visibility.Hidden;
            MoreButton.IsEnabled = true;
            CompletedButton.IsEnabled = true;

            BankStatusPanel.Visibility = System.Windows.Visibility.Hidden;
            BankResultLabel.Visibility = System.Windows.Visibility.Hidden;
            BankCardsListBox.Visibility = System.Windows.Visibility.Hidden;

            // Initialize the player and bank cards
            for (int i = 0; i < 2; i++)
            {
                AddPlayerCard();
                AddBankCard();
            }

            // Check on blackjack or two aces at start.
            var playerCardsSum = _playerCards.Sum(c => c.Rank);
            if (playerCardsSum == 21 || playerCardsSum == 22)
            {
                MoreButton.IsEnabled = false;
                CompletedButton.IsEnabled = false;
                BankCardsListBox.Visibility = System.Windows.Visibility.Visible;
                PlayerResultLabel.Visibility = System.Windows.Visibility.Visible;
                PlayerScoreText.Text = $"{++_playerScore}";
            }
        }

        private void AddPlayerCard()
        {
            var nextCard = _cardDesk!.Next();
            _playerCards!.Add(nextCard!);
            _playerCardTexts.Add($"{nextCard!.Name}");
            PlayerCardsListBox.ItemsSource = _playerCardTexts;
            PlayerCardsListBox.Items.Refresh();
            PlayerTotalTextBox.Text = $"{_playerCards.Sum(c => c.Rank)}";
        }

        private void AddBankCard()
        {
            var nextCard = _cardDesk!.Next();
            _bankCards.Add(nextCard!);
            _bankCardTexts.Add($"{nextCard!.Name}");
            BankCardsListBox.ItemsSource = _bankCardTexts;
            BankCardsListBox.Items.Refresh();
            BankTotalTextBox.Text = $"{_bankCards.Sum(c => c.Rank)}";
        }

        private void MoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_cardDesk.Cards.Count > 0)
            {
                AddPlayerCard();

                if (_playerCards.Sum(c => c.Rank) == 21)
                {
                    MoreButton.IsEnabled = false;
                    CompletedButton.IsEnabled = false;

                    BankCardsListBox.Visibility = System.Windows.Visibility.Visible;
                    PlayerResultLabel.Visibility = System.Windows.Visibility.Visible;

                    PlayerScoreText.Text = $"{++_playerScore}";
                }
                else if (_playerCards.Sum(c => c.Rank) > 21)
                {
                    MoreButton.IsEnabled = false;
                    CompletedButton.IsEnabled = false;

                    BankCardsListBox.Visibility = System.Windows.Visibility.Visible;
                    BankStatusPanel.Visibility = System.Windows.Visibility.Visible;

                    BankResultLabel.Visibility = System.Windows.Visibility.Visible;

                    BankScoreText.Text = $"{++_bankScore}";
                }
            }
        }

        private async void CompletedButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Bank is playing.
            MoreButton.IsEnabled = false;
            CompletedButton.IsEnabled = false;

            BankCardsListBox.Visibility = System.Windows.Visibility.Visible;
            BankStatusPanel.Visibility = System.Windows.Visibility.Visible;

            // Check on 21 or two aces at start
            var bankSum= _bankCards.Sum(c => c.Rank);
            if (bankSum == 21 || bankSum == 22)
            {
                BankResultLabel.Visibility = System.Windows.Visibility.Visible;
                BankScoreText.Text = $"{++_bankScore}";
                return;
            }

            while (_bankCards.Sum(c => c.Rank) < 17 && _cardDesk.Cards.Count > 0)
            {
                await Task.Delay(750);
                AddBankCard();
                if (_bankCards.Sum(c => c.Rank) == 21)
                {
                    break;
                }
            }

            if (_bankCards.Sum(c => c.Rank) > 21 || _playerCards.Sum(c => c.Rank) > _bankCards.Sum(c => c.Rank))
            {
                PlayerResultLabel.Visibility = System.Windows.Visibility.Visible;
                PlayerScoreText.Text = $"{++_playerScore}";
            }
            else if (_playerCards.Sum(c => c.Rank) < _bankCards.Sum(c => c.Rank))
            {
                BankResultLabel.Visibility = System.Windows.Visibility.Visible;
                BankScoreText.Text = $"{++_bankScore}";
            }
            else
            {
                BankResultLabel.Visibility = System.Windows.Visibility.Visible;
                BankScoreText.Text = $"{++_bankScore}";
            }
        }

        private void NewGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrepareGame();
        }
    }
}
