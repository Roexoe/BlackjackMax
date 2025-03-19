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

        // Gaat naar de volgende stap in het uitdelen van kaarten
        private void ProceedToNextDealingStep()
        {
            switch (currentStep)
            {
                case DealingStep.DealerHidden:
                    // Deel verborgen kaart aan dealer
                    dealer.TakeHiddenCard();
                    currentStep = DealingStep.PlayersFirstCard;
                    statusLabel.Text = "Verborgen kaart gedeeld aan dealer. Deel nu kaarten aan spelers.";
                    break;

                case DealingStep.PlayersFirstCard:
                    // Deel eerste kaart aan alle spelers
                    foreach (var player in players)
                    {
                        player.AddCard(deck.DealCard());
                    }
                    currentStep = DealingStep.DealerSecondCard;
                    statusLabel.Text = "Eerste kaart gedeeld aan alle spelers. Deel nu tweede kaart aan dealer.";
                    break;

                case DealingStep.DealerSecondCard:
                    // Deel tweede (zichtbare) kaart aan dealer
                    dealer.TakeVisibleCard();
                    currentStep = DealingStep.PlayersSecondCard;
                    statusLabel.Text = "Tweede kaart gedeeld aan dealer. Deel nu tweede kaart aan spelers.";
                    break;

                case DealingStep.PlayersSecondCard:
                    // Deel tweede kaart aan alle spelers
                    foreach (var player in players)
                    {
                        player.AddCard(deck.DealCard());
                    }
                    currentStep = DealingStep.CheckBlackjack;
                    statusLabel.Text = "Tweede kaart gedeeld aan alle spelers. Controleer nu op blackjack.";
                    break;

                case DealingStep.CheckBlackjack:
                    // Controleer op blackjack bij de dealer en spelers
                    CheckForBlackjacks();
                    break;

                // Hier zouden meer stappen kunnen komen voor het spel zelf, dealer's beurt, etc.
                default:
                    statusLabel.Text = "Geen verdere acties mogelijk op dit moment.";
                    break;
            }
        }

        // Controleer op blackjacks
        private void CheckForBlackjacks()
        {
            string result = "";

            // Controleer eerst of de dealer mogelijk blackjack heeft
            if (dealer.MightHaveBlackjack())
            {
                // Kijk of de dealer inderdaad blackjack heeft
                if (dealer.HasBlackjack())
                {
                    result += "Dealer heeft blackjack! ";

                    // Controleer welke spelers ook blackjack hebben (gelijkspel)
                    List<string> playersWithBlackjack = new List<string>();
                    foreach (var player in players)
                    {
                        if (player.HasBlackjack())
                        {
                            playersWithBlackjack.Add(player.Name);
                        }
                    }

                    if (playersWithBlackjack.Count > 0)
                    {
                        result += $"Gelijkspel voor: {string.Join(", ", playersWithBlackjack)}. ";
                    }

                    result += "Overige spelers verliezen.";
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

            // Voor nu eindigen we het spel na de blackjack check
            currentStep = DealingStep.EndGame;
            ResetGameControls();

            // Toon alle kaarten, inclusief de verborgen kaart van de dealer
            UpdateGameDisplay();
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
            else if (currentStep == DealingStep.EndGame)
            {
                dealerInfo += $"Verborgen kaart: {dealer.RevealHiddenCard()}, Zichtbare kaarten: {dealer.GetVisibleCardsString()} - Totale waarde: {dealer.CalculateHandValue()}";
            }
            else
            {
                dealerInfo += $"[Verborgen], Zichtbare kaarten: {dealer.GetVisibleCardsString()}";
            }
            dealerLabel.Text = dealerInfo;

            // Update speler informatie
            string playerCardsInfo = "";
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                playerCardsInfo += $"{player.Name}: {player.GetCardsString()}";

                // Toon hand waarde als er kaarten gedeeld zijn
                if (player.Cards.Count > 0)
                {
                    playerCardsInfo += $" (Waarde: {player.CalculateHandValue()})";
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
        }
    }
}
