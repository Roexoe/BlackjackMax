using System;
using System.Windows.Forms;

namespace Blackjack
{
    public class InputBox : Form
    {
        public string InputText { get; private set; }
        private TextBox inputTextBox;
        private Button okButton;
        private Button cancelButton;

        public InputBox(string prompt, string title)
        {
            Text = title;
            Width = 400;
            Height = 200;

            Label promptLabel = new Label
            {
                Text = prompt,
                Left = 10,
                Top = 20,
                Width = 360
            };
            Controls.Add(promptLabel);

            inputTextBox = new TextBox
            {
                Left = 10,
                Top = 50,
                Width = 360
            };
            Controls.Add(inputTextBox);

            okButton = new Button
            {
                Text = "OK",
                Left = 220,
                Top = 100,
                DialogResult = DialogResult.OK
            };
            okButton.Click += (sender, e) => { InputText = inputTextBox.Text; Close(); };
            Controls.Add(okButton);

            cancelButton = new Button
            {
                Text = "Cancel",
                Left = 300,
                Top = 100,
                DialogResult = DialogResult.Cancel
            };
            cancelButton.Click += (sender, e) => { Close(); };
            Controls.Add(cancelButton);

            AcceptButton = okButton;
            CancelButton = cancelButton;
        }
    }
}
