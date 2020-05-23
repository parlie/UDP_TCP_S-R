using System.Windows.Controls;

namespace UDP_TCP_S_R
{
    class Placeholder
    {
        public Label label;
        public TextBox textBox;


        public Placeholder(Label _label, TextBox _textBox)
        {
            label = _label;
            textBox = _textBox;
            textBox.GotFocus += LocalGotFocus;
            textBox.LostFocus += LocalLostFocus;
            textBox.TextChanged += LocalTextChange;
            textBox.TargetUpdated += LocalTextChange;
        }

        void LocalTextChange(object sender, System.EventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                label.Visibility = System.Windows.Visibility.Visible;   
            }
            else
            {
                label.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        void LocalGotFocus(object sender, System.EventArgs e)
        {
            label.Visibility = System.Windows.Visibility.Hidden;
        }

        void LocalLostFocus(object sender, System.EventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                label.Visibility = System.Windows.Visibility.Visible;
            }
        }

    }
}
