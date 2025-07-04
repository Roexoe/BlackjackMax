using Blackjack.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        private Shoe shoe;
        private Dealer dealer;
        private List<Player> players;
        private bool gameStarted = false;
        private int currentPlayerIndex = 0;
        private int score = 0; 

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
            shoe = new Shoe(1); // Default 1 deck
            dealer = new Dealer(shoe); // Pas Dealer constructor aan om Shoe te accepteren
            players = new List<Player>();

            // Zet knoppen uit bij start
            drawButton.Enabled = false;
            shuffleButton.Enabled = true;
            hitButton.Enabled = false;
            standButton.Enabled = false;
        }

        private void shuffleButton_Click(object sender, EventArgs e)
        {
            int numberOfDecks = (int)decksNumericUpDown.Value;
            shoe = new Shoe(numberOfDecks);
            statusLabel.Text = $"Nieuwe shoe gemaakt met {numberOfDecks} deck(s)!";

            // Reset de spelstroom
            ResetGameControls();

            // Schakel de shuffle-knop uit
            shuffleButton.Enabled = false;
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
                // Toon de foutmelding
                statusLabel.Text = ex.Message;

                // Schakel de shuffle-knop weer in zodat de gebruiker een nieuwe shoe kan maken
                shuffleButton.Enabled = true;

                // Schakel andere knoppen uit om te voorkomen dat het spel doorgaat
                drawButton.Enabled = false;
                hitButton.Enabled = false;
                standButton.Enabled = false;
            }
        }


        private void startGameButton_Click(object sender, EventArgs e)
        {
            dealer.ResetHand();
            UpdateDealerScore();
            // Controleer of het deck nog voldoende kaarten heeft
            if (shoe.RemainingPercentage <= 25)
            {
                MessageBox.Show("Het deck is onder de 25%. Voeg een nieuw deck toe voordat je een nieuw spel start.",
                                "Deck bijna leeg", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Controleer of er een geldig aantal spelers is
            int numberOfPlayers = (int)playersNumericUpDown.Value;

            if (numberOfPlayers > 0)
            {
                // Controleer of er al spelersnamen zijn opgeslagen
                if (playerNames.Count == 0)
                {
                    // Vraag om nieuwe spelersnamen
                    for (int i = 1; i <= numberOfPlayers; i++)
                    {
                        string playerName = PromptForPlayerName($"Speler {i}");
                        playerNames.Add(playerName);
                    }
                }

                // Maak de spelers aan op basis van de opgeslagen namen
                players.Clear();
                foreach (var name in playerNames)
                {
                    players.Add(new Player(name));
                }

                // Reset de dealer
                dealer.ResetHand();

                // Reset de huidige stap
                currentStep = DealingStep.DealerHidden;
                currentPlayerIndex = 0;

                // Update UI
                statusLabel.Text = $"Spel gestart met {numberOfPlayers} spelers! Klik op 'Deal Card' om met delen te beginnen.";
                drawButton.Enabled = true;
                startGameButton.Enabled = false;
                playersNumericUpDown.Enabled = false;
                decksNumericUpDown.Enabled = false;
                hitButton.Enabled = false;
                standButton.Enabled = false;

                // Schakel de shuffle-knop uit
                shuffleButton.Enabled = false;

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
            UpdateDealerScore(); // Update de score in de UI
            if (!dealer.ShouldHit(true)) // Pass a boolean value for 'userDecision'
            {
                dealerHitButton.Enabled = false;
                DetermineResults();
            }
        }

        // Gaat naar de volgende stap in het uitdelen van kaarten
        private bool AskDealerApproval(string message)
        {
            var result = MessageBox.Show(message, "Dealer Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                score += 1; // Correcte keuze, 1 punt erbij
            }
            else
            {
                score -= 1; // Foute keuze, 1 strafpunt
            }
            UpdateScoreDisplay(); // Update de score in de UI
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
                            player.AddCard(shoe.DealCard());
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
                            player.AddCard(shoe.DealCard());
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
                        currentStep = DealingStep.PlayersTurn; // Ga pas naar PlayersTurn na de check
                        statusLabel.Text = "Blackjack-check voltooid. Spelers kunnen nu hun beurt spelen.";
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
                if (dealer.HiddenCardMakesBlackjack())
                {
                    dealer.RevealHiddenCard();
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
                currentPlayer.Hit(shoe);

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

            // Zorg ervoor dat currentPlayerIndex niet buiten de grenzen gaat
            if (currentPlayerIndex >= players.Count)
            {
                // Alle spelers zijn geweest, nu is het de beurt aan de dealer
                currentStep = DealingStep.DealerTurn;
                hitButton.Enabled = false;
                standButton.Enabled = false;
                DealerPlay();
                return;
            }

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

        private void splitButton_Click(object sender, EventArgs e)
        {
            // Controleer of de huidige beurt van de dealer is
            if (currentStep == DealingStep.DealerTurn)
            {
                statusLabel.Text = "Dealer mag niet splitsen.";
                return;
            }

            Player currentPlayer = players[currentPlayerIndex];

            if (currentPlayer.CanSplit())
            {
                // Voeg de nieuwe hand toe aan de lijst van spelers
                Player newHand = currentPlayer.Split(shoe);
                players.Insert(currentPlayerIndex + 1, newHand);

                statusLabel.Text = $"{currentPlayer.Name} heeft gesplit!";

                // Update de UI
                UpdateGameDisplay();
            }
            else
            {
                statusLabel.Text = "Splitten is niet mogelijk voor deze hand.";
            }
        }


        private void doubleDownButton_Click(object sender, EventArgs e)
        {
            // Controleer of de huidige beurt van de dealer is
            if (currentStep == DealingStep.DealerTurn)
            {
                statusLabel.Text = "Dealer mag geen Double Down uitvoeren.";
                return;
            }

            // Controleer of currentPlayerIndex binnen de grenzen ligt
            if (currentPlayerIndex < 0 || currentPlayerIndex >= players.Count)
            {
                statusLabel.Text = "Ongeldige spelerindex. Double Down kan niet worden uitgevoerd.";
                return;
            }

            Player currentPlayer = players[currentPlayerIndex];

            if (currentPlayer.CanDoubleDown())
            {
                currentPlayer.DoubleDown(shoe);
                statusLabel.Text = $"{currentPlayer.Name} heeft Double Down uitgevoerd!";

                // Automatisch Stand na Double Down
                standButton_Click(sender, e);
            }
            else
            {
                statusLabel.Text = "Double Down is niet mogelijk voor deze hand.";
            }
        }
        // Voor het toevoegen van een speler
        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            if (gameStarted)
            {
                MessageBox.Show("Je kunt geen nieuwe spelers toevoegen tijdens een spel.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string playerName = PromptForPlayerName();
            if (!string.IsNullOrWhiteSpace(playerName))
            {
                players.Add(new Player(playerName));
                statusLabel.Text = $"Speler '{playerName}' toegevoegd!";
                UpdateGameDisplay();
            }
            else
            {
                MessageBox.Show("Naam van de speler mag niet leeg zijn.", "Ongeldige invoer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private List<string> playerNames = new List<string>();

        // Voor het verwijderen van een speler
        private void removePlayerButton_Click(object sender, EventArgs e)
        {
            if (playerNames.Count == 0)
            {
                MessageBox.Show("Er zijn geen spelers om te verwijderen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var inputDialog = new InputBox("Voer de naam in van de speler die je wilt verwijderen:", "Verwijder Speler"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    string nameToRemove = inputDialog.InputText;
                    if (playerNames.Remove(nameToRemove))
                    {
                        MessageBox.Show($"Speler '{nameToRemove}' is verwijderd.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Speler '{nameToRemove}' is niet gevonden.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // Dealer speelt zijn beurt
        private bool AskDealerForCard()
        {
            var result = MessageBox.Show("Wilt u een kaart pakken?", "Dealer Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                score += 1; // Correcte keuze, 1 punt erbij
            }
            else
            {
                score -= 1; // Foute keuze, 1 strafpunt
            }
            UpdateScoreDisplay(); // Update de score in de UI
            return result == DialogResult.Yes;
        }

        private void DealerPlay()
        {
            statusLabel.Text = "De dealer speelt nu...";

            // Onthul de verborgen kaart als deze nog niet is onthuld
            if (dealer.HasHiddenCard())
            {
                if (AskDealerApproval("Wilt u de verborgen kaart onthullen?"))
                {
                    dealer.RevealHiddenCard();
                    UpdateGameDisplay();
                    statusLabel.Text = "Dealer heeft verborgen kaart onthuld.";
                }
            }

            // Dealer blijft kaarten trekken tot minstens 17 of totdat hij besluit te stoppen
            while (true)
            {
                bool userDecision = AskDealerForCard(); // Vraag de gebruiker of de dealer moet hitten
                if (!userDecision || !dealer.ShouldHit(userDecision)) // Controleer of de dealer moet stoppen
                {
                    break; // Stop als de dealer niet meer hoeft te hitten
                }

                dealer.TakeVisibleCard();
                UpdateGameDisplay();
                statusLabel.Text = $"Dealer heeft een kaart gepakt. Totale waarde: {dealer.CalculateHandValue()}";

                UpdateDealerScore(); // Werk de score bij
            }

            // Bepaal de resultaten
            DetermineResults();
        }


        // Bepaal de resultaten van het spel
        // Bepaal de resultaten van het spel
        private void DetermineResults()
        {
            int dealerValue = dealer.CalculateHandValue();
            bool dealerBusted = dealerValue > 21;
            string results = dealerBusted ?
                $"Dealer is busted met {dealerValue}! " :
                $"Dealer eindigt met {dealerValue}. ";

            foreach (var player in players)
            {
                int playerValue = player.CalculateHandValue();

                if (player.IsBusted)
                {
                    results += $"{player.Name} verliest (busted). ";
                    score -= 1; // Strafpunt voor verliezen
                }
                else if (dealerBusted)
                {
                    results += $"{player.Name} wint (dealer busted). ";
                    score += 1; // Punt voor winnen
                }
                else if (playerValue > dealerValue)
                {
                    results += $"{player.Name} wint met {playerValue} tegen {dealerValue}. ";
                    score += 1; // Punt voor winnen
                }
                else if (playerValue < dealerValue)
                {
                    results += $"{player.Name} verliest met {playerValue} tegen {dealerValue}. ";
                    score -= 1; // Strafpunt voor verliezen
                }
                else
                {
                    results += $"{player.Name} speelt gelijk met {playerValue}. ";
                }
            }

            statusLabel.Text = results;
            UpdateScoreDisplay(); // Update de score in de UI
        


            // Controleer of de shoe minder dan 25% kaarten bevat
            if (shoe.IsLastDeckLowOnCards)
            {
                shuffleButton.Enabled = true;
                string warning = "Het laatste deck heeft minder dan 25% kaarten over. Voeg een nieuwe shoe toe.";
                statusLabel.Text += $" | {warning}";
            }

            // Spel is klaar, reset controls maar behoud het resultaat
            currentStep = DealingStep.EndGame;
            ResetGameControls(false); // Gebruik de parameter 'false' om aan te geven dat de statustext niet moet worden bijgewerkt
        }


        // Update het scherm om huidige spelstatus te tonen
        private void UpdateGameDisplay()
        {
            // Voeg informatie over shoe en het huidige deck toe bovenaan
            string shoeInfo = $"Shoe: {shoe.RemainingCards} kaarten over, {shoe.RemainingDecks} deck(s) over";
            string deckInfo = $"Huidig deck: {shoe.CurrentDeckRemainingPercentage:F1}% over";
            string displayInfo = $"{shoeInfo} | {deckInfo}";

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
                // Add indication of hidden card
                string hiddenCardInfo = dealer.HasHiddenCard() ? " [+ 1 verborgen kaart]" : "";
                dealerInfo += $"Zichtbare kaarten: {dealer.GetVisibleCardsString()}{hiddenCardInfo}";
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
            playersInfoLabel.Text = $"Spelers ({players.Count}): {displayInfo}";

            // Controleer of het laatste deck minder dan 25% kaarten heeft
            if (shoe.IsLastDeckLowOnCards)
            {
                shuffleButton.Enabled = true;
                statusLabel.Text += " | Het laatste deck heeft minder dan 25% kaarten over. Voeg een nieuwe shoe toe.";
            }

            // Alternatief: voeg shoe informatie toe aan bestaande statusLabel tekst als die er nog niet is
            if (!statusLabel.Text.Contains("Shoe:") && !statusLabel.Text.Contains("Huidig deck:"))
            {
                statusLabel.Text = $"{statusLabel.Text} | {displayInfo}";
            }

            // Update Split en Double Down knoppen
            if (currentStep == DealingStep.PlayersTurn && currentPlayerIndex >= 0 && currentPlayerIndex < players.Count)
            {
                Player currentPlayer = players[currentPlayerIndex];
                splitButton.Enabled = currentPlayer.CanSplit();
                doubleDownButton.Enabled = currentPlayer.CanDoubleDown();
                hitButton.Enabled = true;
                standButton.Enabled = true;
            }
            else
            {
                splitButton.Enabled = false;
                doubleDownButton.Enabled = false;
                hitButton.Enabled = false;
                standButton.Enabled = false;
            }
        }

        // Reset de controls voor een nieuw spel
        private void ResetGameControls(bool updateStatusText = true)
        {
            startGameButton.Enabled = true;
            playersNumericUpDown.Enabled = true;
            decksNumericUpDown.Enabled = true;
            drawButton.Enabled = false;
            hitButton.Enabled = false;
            standButton.Enabled = false;

            // Update de statustext alleen als dat nodig is
            if (updateStatusText)
            {
                statusLabel.Text = "Klik op 'Start Game' om een nieuw spel te beginnen.";
            }
        }
        private string PromptForPlayerName(string defaultName = "Speler")
        {
            using (var inputDialog = new InputBox($"Voer de naam in voor {defaultName}:", "Speler Naam"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    return inputDialog.InputText;
                }
            }
            return defaultName;
        }


        private void statusLabel_Click(object sender, EventArgs e)
        {

        }
        private void UpdateDealerScore()
        {
            dealerScoreLabel.Text = $"Dealer Score: {dealer.Score}";
        }

        private void dealerScoreLabel_Click(object sender, EventArgs e)
        {

        }
        private void UpdateScoreDisplay()
        {
            scoreLabel.Text = $"Score: {score}"; // Zorg ervoor dat er een `scoreLabel` in de UI is
        }
    }
}
