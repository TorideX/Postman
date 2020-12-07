using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Postman.Views
{
    public class MvvmTextEditor : TextEditor
    {
        public static readonly DependencyProperty BodyProperty =
        DependencyProperty.Register("Body", typeof(string), typeof(MvvmTextEditor));

        public string Body
		{
			get { return (string)GetValue(BodyProperty); }
			set { SetValue(BodyProperty, value); }
		}
        
        public MvvmTextEditor()
        {
            TextChanged += TextChangedEvent;
        }

        private void TextChangedEvent(object sender, EventArgs e)
        {
            Body = Text;
        }
    }
}
