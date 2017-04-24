using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Aurochs.Desktop.Views.Behaviors
{
    public class ShortcutKeyBehavior : Behavior<TextBox>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ShortcutKeyBehavior), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ShortcutKeyBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.KeyDown -= OnKeyDown;
            base.OnDetaching();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Return) && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if(Command is ICommand command)
                {
                    if (command.CanExecute(CommandParameter))
                    {
                        command.Execute(CommandParameter);
                    }
                }
            }
        }
    }
}
