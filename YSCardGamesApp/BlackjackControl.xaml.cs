using System.Windows.Controls;
using System.Windows.Media;
using YSCardGamesLibrary;

namespace YSCardGamesApp
{
    /// <summary>
    /// Interaction logic for BlackjackControl.xaml
    /// </summary>
    public partial class BlackjackControl : UserControl
    {
        private List<Card> _playerCards = [];
        private List<Card4View> _playerCard4Views = [];
        private List<Card> _bankCards = [];
        private List<Card4View> _bankCard4Views = [];
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
            _playerCard4Views = [];
            _bankCards = [];
            _bankCard4Views = [];
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
                DisablePlayerButtons();
                ShowPlayerWiner();
                SetBankCardListVisible();
                PlayerScoreText.Text = $"{++_playerScore}";
            }
        }

        private void AddPlayerCard()
        {
            var nextCard = _cardDesk!.Next();
            _playerCards!.Add(nextCard!);
            _playerCard4Views.Add(new Card4View() { Name = nextCard!.Name, TextColor = GetCardTextColor(nextCard)});
            PlayerCardsListBox.ItemsSource = _playerCard4Views;
            PlayerCardsListBox.Items.Refresh();
            PlayerTotalTextBox.Text = $"{_playerCards.Sum(c => c.Rank)}";
        }

        private void AddBankCard()
        {
            var nextCard = _cardDesk!.Next();
            _bankCards.Add(nextCard!);
            _bankCard4Views.Add(new Card4View() { Name = nextCard!.Name, TextColor = GetCardTextColor(nextCard) });
            BankCardsListBox.ItemsSource = _bankCard4Views;
            BankCardsListBox.Items.Refresh();
            BankTotalTextBox.Text = $"{_bankCards.Sum(c => c.Rank)}";
        }

        private void MoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_cardDesk!.Cards.Count > 0)
            {
                AddPlayerCard();

                var playerSum = _playerCards.Sum(c => c.Rank);
                if (playerSum == 21)
                {
                    DisablePlayerButtons();

                    SetBankCardListVisible();

                    ShowPlayerWiner();

                    PlayerScoreText.Text = $"{++_playerScore}";
                }
                else if (playerSum > 21)
                {
                    DisablePlayerButtons();

                    SetBankCardListVisible();

                    ShowBankWiner();

                    BankScoreText.Text = $"{++_bankScore}";
                }
            }
        }

        private void ShowPlayerWiner()
        {
            PlayerResultLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowBankWiner()
        {
            BankResultLabel.Visibility = System.Windows.Visibility.Visible;
        }
        private void DisablePlayerButtons()
        {
            MoreButton.IsEnabled = false;
            CompletedButton.IsEnabled = false;
        }

        private void SetBankCardListVisible()
        {
            BankCardsListBox.Visibility = System.Windows.Visibility.Visible;
        }

        private async void CompletedButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Bank is playing.
            DisablePlayerButtons();

            SetBankCardListVisible();
            BankStatusPanel.Visibility = System.Windows.Visibility.Visible;

            // Check on 21 or two aces at start
            var bankSum = _bankCards.Sum(c => c.Rank);
            if (bankSum == 21 || bankSum == 22)
            {
                ShowBankWiner();
                BankScoreText.Text = $"{++_bankScore}";
                return;
            }

            while (_bankCards.Sum(c => c.Rank) < 17 && _cardDesk!.Cards.Count > 0)
            {
                await Task.Delay(750);
                AddBankCard();
                if (_bankCards.Sum(c => c.Rank) == 21)
                {
                    break;
                }
            }

            bankSum = _bankCards.Sum(c => c.Rank);
            var playerSum = _playerCards.Sum(c => c.Rank);

            if (bankSum > 21 || bankSum < playerSum)
            {
                ShowPlayerWiner();
                PlayerScoreText.Text = $"{++_playerScore}";
            }
            else
            {
                // bankSum = 21 or bankSum < 21 and bankSum >= playerSum
                ShowBankWiner();
                BankScoreText.Text = $"{++_bankScore}";
            }
        }

        private void NewGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrepareGame();
        }

        private static SolidColorBrush GetCardTextColor(Card card)
        {
            if (card.Suit == "Hearts" || card.Suit == "Diamonds")
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }
        }
    }
}
