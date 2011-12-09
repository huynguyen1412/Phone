using System;
using System.Windows;
using System.Windows.Controls;

namespace BorderTextControl {
    public partial class BorderedText : UserControl {

        #region @property string Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BorderedText), new PropertyMetadata(null));

        public string Text {
            set { SetValue(TextProperty, value); }
            get { return GetValue(TextProperty) as string; }
        }
        #endregion
        #region @property TextAlignment TextAlignment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(BorderedText), new PropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment {
            set { SetValue(TextAlignmentProperty, value); }
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
        }
        #endregion
        #region @property TextDecoration TextDecoration
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(BorderedText), new PropertyMetadata(null));

        public TextDecorationCollection TextDecorations {
            set { SetValue(TextDecorationsProperty, value); }
            get { return GetValue(TextDecorationsProperty) as TextDecorationCollection; }
        }
        #endregion
        #region @property TextWrapping TextWrapping
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(BorderedText), new PropertyMetadata(TextWrapping.NoWrap));

        public TextWrapping TextWrapping {
            set { SetValue(TextWrappingProperty, value); }
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
        }
        #endregion
        #region @property CornerRadius CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(BorderedText), new PropertyMetadata(new CornerRadius()));

        public CornerRadius CornerRadius {
            set { SetValue(CornerRadiusProperty, value); }
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        }
        #endregion

        public BorderedText() {
            //InitializeComponent();
        }

    }
}
