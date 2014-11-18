using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace VirtualSales.iOS
{

    public static class NumberFormattingHelper
    {
        public static bool IsEmptyValue(string text) {
            var temp = StripCurrency(text);
            return temp.Length == 0;
        }

        public static string StripCurrency(string text) {
            var temp = text.Replace("$", "").Replace(",", "").Replace(" ", "").Trim();
            return temp;
        }
    }

        public class TextFieldCurrencyDelegate : UITextFieldDelegate
        {
            private int _digitLimit;
            private Action<string> _postChangeAction;
            public TextFieldCurrencyDelegate(int digitLimit, Action<string> postChangeAction)
            {
                _digitLimit = digitLimit;
                _postChangeAction = postChangeAction;
            }

            public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
            {
                var text = textField.Text;
                string newText = text;
                if (range.Location >= text.Length)
                {
                    newText = text + replacementString;
                }
                else
                {
                    var substr = text.Substring(0, range.Location);
                    var finalIndex = range.Location + range.Length - 1;
                    if(finalIndex >= text.Length - 1) {
                        newText = substr + replacementString;
                    }
                    else
                    {
                        var substr2 = text.Substring(finalIndex + 1);
                        newText = substr + replacementString + substr2;
                    }
                }

                var numText = NumberFormattingHelper.StripCurrency(newText);
                if (numText.Length > _digitLimit) return false;

                string result = string.Empty;
                if (numText.Length == 0)
                {
                    result = "$0";
                }
                else
                {
                    int number;
                    if (Int32.TryParse(numText, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out number))
                    {
                        result = string.Format("{0:C0}", number);
                    }
                }

                if (_postChangeAction != null)
                {
                    _postChangeAction(result);
                }
                
                textField.Text = result;
                
                return false;
            }
        }
}