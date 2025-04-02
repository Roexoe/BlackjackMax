using Blackjack.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        private Deck deck;
        private Dealer dealer;
        private List<Player> players;
        private bool gameStarted = false;
        private int currentPlayerIndex = 0;

        // Stap in het uitdelen van kaarten
        private enum DealingStep
        {
            DealerHidden,
            PlayersFirstCard,
            DealerSecondCard,
            PlayersSecondCard,
            CheckBlackjack,
            PlayersTurn,
            DealerTurn,
            EndGame
        }
        private DealingStep currentStep;

        public Form1()
        {
            InitializeComponent();
            deck = new Deck();
            dealer = new Dealer(deck);
            players = new List<Player>();

            // Zet knoppen uit bij start
            drawButton.Enabled = false;
            shuffleButton.Enabled = true;
            hitButton.Enabled = false;
            standButton.Enabled = false;
        }

        private void shuffleButton_Click(object sender, EventArgs e)
        {
            deck.Shuffle();
            statusLabel.Text = "Deck geschud!";
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            try
            {
                ProceedToNextDealingStep();
                UpdateGameDisplay();
            }
            catch (InvalidOperationException ex)
            {
                statusLabel.Text = ex.Message;
            }
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            // Controleer of er geldig aantal spelers is
            int numberOfPlayers = (int)playersNumericUpDown.Value;

            if (numberOfPlayers > 0)
            {
                // Start het spel
                gameStarted = true;

                // Maak de spelers aan
                players.Clear();
                for (int i = 1; i <= numberOfPlayers; i++)
                {
                    players.Add(new Player($"Speler {i}"));
                }

                // Reset en schud deck
                deck.ResetDeck();
                deck.Shuffle();

                // Reset de dealer
                dealer = new Dealer(deck);

                // Reset de huidige stap
                currentStep = DealingStep.DealerHidden;
                currentPlayerIndex = 0;

                // Update UI
                statusLabel.Text = $"Spel gestart met {numberOfPlayers} spelers! Deck is geschud. Klik op 'Deal Card' om met delen te beginnen.";
                drawButton.Enabled = true;
                startGameButton.Enabled = false;
                playersNumericUpDown.Enabled = false;
                hitButton.Enabled = false;
                standButton.Enabled = false;

                // Toon instructie
                cardLabel.Text = "Klik op 'Deal Card' om de verborgen kaart voor de dealer te delen.";
                dealerLabel.Text = "Dealer: Nog geen kaarten";
                playersInfoLabel.Text = "Spelers: Gereed om te starten";
            }
            else
            {
                statusLabel.Text = "Stel een geldig aantal spelers in (minimaal 1).";
            }
        }

        private void dealerHitButton_Click(object sender, EventArgs e)
        {
            dealer.TakeVisibleCard();
            UpdateGameDisplay();
            if (!dealer.ShouldHit())
            {
                dealerHitButton.Enabled = false;
                DetermineResults();
            }
        }

        // Gaat naar de volgende stap in het uitdelen van kaarten
        private bool AskDealerApproval(string message)
        {
            var result = MessageBox.Show(message, "Dealer Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void ProceedToNextDealingStep()
        {
            switch (currentStep)
            {
                case DealingStep.DealerHidden:
                    if (AskDealerApproval("Wilt u de verborgen kaart aan de dealer delen?"))
                    {
                        dealer.TakeHiddenCard();
                        currentStep = DealingStep.PlayersFirstCard;
                        statusLabel.Text = "Verborgen kaart gedeeld aan dealer. Deel nu kaarten aan spelers.";
                    }
                    break;

                case DealingStep.PlayersFirstCard:
                    if (AskDealerApproval("Wilt u de eerste kaart aan alle spelers delen?"))
                    {
                        foreach (var player in players)
                        {
                            player.AddCard(deck.DealCard());
                        }
                        currentStep = DealingStep.DealerSecondCard;
                        statusLabel.Text = "Eerste kaart gedeeld aan alle spelers. Deel nu tweede kaart aan dealer.";
                    }
                    break;

                case DealingStep.DealerSecondCard:
                    if (AskDealerApproval("Wilt u de tweede (zichtbare) kaart aan de dealer delen?"))
                    {
                        dealer.TakeVisibleCard();
                        currentStep = DealingStep.PlayersSecondCard;
                        statusLabel.Text = "Tweede kaart gedeeld aan dealer. Deel nu tweede kaart aan spelers.";
                    }
                    break;

                case DealingStep.PlayersSecondCard:
                    if (AskDealerApproval("Wilt u de tweede kaart aan alle spelers delen?"))
                    {
                        foreach (var player in players)
                        {
                            player.AddCard(deck.DealCard());
                        }
                        // Maak de verborgen kaart van de dealer zichtbaar
                        dealer.RevealHiddenCard();
                        currentStep = DealingStep.CheckBlackjack;
                        statusLabel.Text = "Tweede kaart gedeeld aan alle spelers. Verborgen kaart van de dealer is nu zichtbaar. Controleer nu op blackjack.";
                    }
                    break;

                case DealingStep.CheckBlackjack:
                    if (AskDealerApproval("Wilt u controleren op blackjack bij de dealer en spelers?"))
                    {
                        CheckForBlackjacks();
                    }
                    break;

                default:
                    statusLabel.Text = "Geen verdere acties mogelijk op dit moment.";
                    break;
            }
        }


        // Controleer op blackjacks
        private void CheckForBlackjacks()
        {
            string result = "";
            bool anyBlackjacks = false;

            // Controleer eerst of de dealer mogelijk blackjack heeft
            if (dealer.MightHaveBlackjack())
            {
                // Kijk of de dealer inderdaad blackjack heeft
                if (dealer.HasBlackjack())
                {
                    result += "Dealer heeft blackjack! ";
                    anyBlackjacks = true;

                    // Controleer welke spelers ook blackjack hebben (gelijkspel)
                    List<string> playersWithBlackjack = new List<string>();
                    foreach (var player in players)
                    {
                        if (player.HasBlackjack())
                        {
                            playersWithBlackjack.Add(player.Name);
                            player.Stand(); // Deze spelers doen niet meer mee
                        }
                    }

                    if (playersWithBlackjack.Count > 0)
                    {
                        result += $"Gelijkspel voor: {string.Join(", ", playersWithBlackjack)}. ";
                    }

                    result += "Overige spelers verliezen.";

                    // Als de dealer blackjack heeft, is het spel voorbij
                    currentStep = DealingStep.EndGame;
                    ResetGameControls();
                }
                else
                {
                    result = "Dealer heeft geen blackjack. Controleer spelers...";

                    // Controleer welke spelers blackjack hebben
                    List<string> playersWithBlackjack = new List<string>();
                    foreach (var player in players)
                    {
                        if (player.HasBlackjack())
                        {
                            playersWithBlackjack.Add(player.Name);
                            player.Stand(); // Deze spelers doen niet meer mee
                            anyBlackjacks = true;
                        }
                    }

                    if (playersWithBlackjack.Count > 0)
                    {
                        result += $" Blackjack voor: {string.Join(", ", playersWithBlackjack)}!";
                    }
                    else
                    {
                        result += " Geen spelers met blackjack.";
                    }
                }
            }
            else
            {
                // Dealer kan geen blackjack hebben, controleer spelers
                List<string> playersWithBlackjack = new List<string>();
                foreach (var player in players)
                {
                    if (player.HasBlackjack())
                    {
                        playersWithBlackjack.Add(player.Name);
                        player.Stand(); // Deze spelers doen niet meer mee
                        anyBlackjacks = true;
                    }
                }

                if (playersWithBlackjack.Count > 0)
                {
                    result = $"Blackjack voor: {string.Join(", ", playersWithBlackjack)}!";
                }
                else
                {
                    result = "Geen blackjacks gevonden. Het spel gaat verder.";
                }
            }

            statusLabel.Text = result;

            // Als er geen dealer blackjack is, ga door naar de beurt van de eerste speler
            if (!dealer.HasBlackjack())
            {
                currentStep = DealingStep.PlayersTurn;
                currentPlayerIndex = 0;

                // Zoek de eerste speler die nog geen blackjack heeft
                while (currentPlayerIndex < players.Count && players[currentPlayerIndex].IsStanding)
                {
                    currentPlayerIndex++;
                }

                if (currentPlayerIndex < players.Count)
                {
                    // Activeer hit/stand knoppen
                    hitButton.Enabled = true;
                    standButton.Enabled = true;
                    drawButton.Enabled = false;

                    // Update instructies
                    statusLabel.Text += $" {players[currentPlayerIndex].Name} is aan de beurt.";
                }
                else
                {
                    // Alle spelers hebben blackjack, ga naar dealer beurt
                    currentStep = DealingStep.DealerTurn;
                    DealerPlay();
                }
            }

            // Update de display
            UpdateGameDisplay();
        }

        // Speler kiest om nog een kaart te nemen
        private void hitButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Huidige speler neemt een kaart
                Player currentPlayer = players[currentPlayerIndex];
                currentPlayer.Hit(deck);

                // Update display
                UpdateGameDisplay();

                // Controleer of speler heeft verloren (busted)
                if (currentPlayer.IsBusted)
                {
                    statusLabel.Text = $"{currentPlayer.Name} is busted! (Waarde: {currentPlayer.CalculateHandValue()})";
                    // Automatisch standen voor deze speler en doorgaan naar de volgende
                    standButton_Click(sender, e);
                }
            }
            catch (InvalidOperationException ex)
            {
                statusLabel.Text = ex.Message;
            }
        }

        // Speler kiest om te passen (geen kaarten meer)
        private void standButton_Click(object sender, EventArgs e)
        {
            // Huidige speler past
            players[currentPlayerIndex].Stand();

            // Ga naar de volgende speler of naar de dealer als alle spelers klaar zijn
            currentPlayerIndex++;

            // Zoek de volgende speler die nog geen blackjack heeft en niet staat
            while (currentPlayerIndex < players.Count && players[currentPlayerIndex].IsStanding)
            {
                currentPlayerIndex++;
            }

            if (currentPlayerIndex < players.Count)
            {
                // Volgende speler is aan de beurt
                statusLabel.Text = $"{players[currentPlayerIndex].Name} is aan de beurt.";
            }
            else
            {
                // Alle spelers zijn geweest, nu is het de beurt aan de dealer
                currentStep = DealingStep.DealerTurn;
                hitButton.Enabled = false;
                standButton.Enabled = false;
                DealerPlay();
            }

            UpdateGameDisplay();
        }

        // Dealer speelt zijn beurt
        private bool AskDealerForCard()
        {
            var result = MessageBox.Show("Wilt u een kaart pakken?", "Dealer Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void DealerPlay()
        {
            // Dealer speelt zijn beurt
            statusLabel.Text = "De dealer speelt nu...";

            // Dealer blijft kaarten trekken tot minstens 17 of totdat hij besluit te stoppen
            while (dealer.ShouldHit() && AskDealerForCard())
            {
                dealer.TakeVisibleCard();
                UpdateGameDisplay();

                // Controleer of de dealer al 17 of hoger heeft bereikt
                if (dealer.CalculateHandValue() >= 17)
                {
                    break;
                }
            }

            // Update display
            UpdateGameDisplay();

            // Bepaal de resultaten
            DetermineResults();
        }

        // Bepaal de resultaten van het spel
        private void DetermineResults()
        {
            int dealerValue = dealer.CalculateHandValue();
            bool dealerBusted = dealerValue > 21;
            string results = dealerBusted ?
                $"Dealer is busted met {dealerValue}! " :
                $"Dealer eindigt met {dealerValue}. ";

            // Controleer uitslag voor elke speler
            foreach (var player in players)
            {
                int playerValue = player.CalculateHandValue();

                if (player.IsBusted)
                {
                    results += $"{player.Name} verliest (busted). ";
                }
                else if (dealerBusted)
                {
                    results += $"{player.Name} wint (dealer busted). ";
                }
                else if (playerValue > dealerValue)
                {
                    results += $"{player.Name} wint met {playerValue} tegen {dealerValue}. ";
                }
                else if (playerValue < dealerValue)
                {
                    results += $"{player.Name} verliest met {playerValue} tegen {dealerValue}. ";
                }
                else
                {
                    results += $"{player.Name} speelt gelijk met {playerValue}. ";
                }
            }

            statusLabel.Text = results;

            // Spel is klaar, reset controls
            currentStep = DealingStep.EndGame;
            ResetGameControls();
        }


        // Update het scherm om huidige spelstatus te tonen
        private void UpdateGameDisplay()
        {
            // Update dealer informatie
            string dealerInfo = "Dealer: ";
            if (currentStep == DealingStep.DealerHidden)
            {
                dealerInfo += "Geen kaarten";
            }
            else if (currentStep == DealingStep.EndGame || currentStep == DealingStep.DealerTurn)
            {
                dealerInfo += $"Zichtbare kaarten: {dealer.GetVisibleCardsString()} - Totale waarde: {dealer.CalculateHandValue()}";
            }
            else
            {
                dealerInfo += $" Zichtbare kaarten: {dealer.GetVisibleCardsString()}";
            }
            dealerLabel.Text = dealerInfo;

            // Update speler informatie
            string playerCardsInfo = "";
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];

                // Markeer de huidige speler
                if (currentStep == DealingStep.PlayersTurn && i == currentPlayerIndex)
                {
                    playerCardsInfo += "> ";  // Pijl om aan te geven wie aan de beurt is
                }

                playerCardsInfo += $"{player.Name}: {player.GetCardsString()}";

                // Toon hand waarde als er kaarten gedeeld zijn
                if (player.Cards.Count > 0)
                {
                    playerCardsInfo += $" (Waarde: {player.CalculateHandValue()})";

                    // Toon status (busted of standing)
                    if (player.IsBusted)
                    {
                        playerCardsInfo += " [BUSTED]";
                    }
                    else if (player.IsStanding)
                    {
                        playerCardsInfo += " [STAND]";
                    }
                }

                if (i < players.Count - 1)
                {
                    playerCardsInfo += "\n";
                }
            }
            cardLabel.Text = playerCardsInfo;

            // Update spelers label
            playersInfoLabel.Text = $"Spelers ({players.Count}):";
        }

        // Reset de controls voor een nieuw spel
        private void ResetGameControls()
        {
            startGameButton.Enabled = true;
            playersNumericUpDown.Enabled = true;
            drawButton.Enabled = false;
            hitButton.Enabled = false;
            standButton.Enabled = false;
        }
    }
}
